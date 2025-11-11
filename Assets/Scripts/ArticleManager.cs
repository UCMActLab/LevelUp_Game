using DA_Assets.FCU.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public struct ArticleJSONData {
    public string Language;
    public bool isTrue;
    public string Headline;
    public string Body;
    public string Multimedia;
    public string Source;
    public string Links;
    public List<ConversationJSON> Conversation;
}

[Serializable]
public struct ConversationJSON
{
    public List<MessagesJSON> Messages;
}

[Serializable]
public struct MessagesJSON
{
    public string Sender;
    public List<string> MessageList;
}

// este es necesario porque la base de datos es tal que así:
// {data: {data:[artículos]}}
[Serializable]
public struct ArticleJSONRoot
{
    public ArticlesJSONRoot data;
}

[Serializable]
public struct ArticlesJSONRoot
{
    public List<ArticleJSONData> data;

}

public class ArticleManager : Singleton<ArticleManager>
{
    [SerializeField] bool _loadImages = true;

    Dictionary<int, List<ArticleData>> _articlesByLanguage = new Dictionary<int, List<ArticleData>>();

    public bool ArticlesCreated { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        ArticlesCreated = false;
    }

    public void CreateArticles(List<ArticleJSONRoot> articles)
    {
        StartCoroutine(CreateArticles_Coroutine(articles));
    }

    private IEnumerator CreateArticles_Coroutine(List<ArticleJSONRoot> articles)
    {
        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        // TODO: Guardar las imágenes descargadas en streaming assets o algo así
        //Texture2D[] textures = Resources.LoadAll<Texture2D>("Articles/Images");

        //foreach(Texture2D texture in textures)
        //{
        //    sprites.Add(texture.name, Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero));
        //}

        foreach(ArticleJSONRoot root in articles)
        {
            if(root.data.data == null)
            {
                continue;
            }

            foreach (ArticleJSONData data in root.data.data)
            {
                ArticleData article = ScriptableObject.CreateInstance("ArticleData") as ArticleData;

                article.isTrue = data.isTrue;
                article.articleTitle = data.Headline;
                article.articleBody = data.Body;

                // article.image = data.Multimedia; TODO: Tratamiento de imágenes
                if(_loadImages && data.Multimedia != null && data.Multimedia != "")
                {
                    if(sprites.ContainsKey(data.Multimedia))
                    {
                        article.articleImage = sprites[data.Multimedia];
                    }
                    else
                    {
                        string driveURL = "https://drive.google.com/uc?export=download&id=" + data.Multimedia.Replace("image:", "");
                        UnityWebRequest request = UnityWebRequestTexture.GetTexture(driveURL);

                        yield return request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.Success)
                        {
                            yield return new WaitUntil(() => request.downloadHandler.isDone);
                            Texture2D loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                            Sprite spr = Sprite.Create(loadedTexture, new Rect(0.0f, 0.0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
                            article.articleImage = spr;
                            sprites.Add(data.Multimedia, spr);
                        }
                        else { 
                            Debug.LogError("No se ha podido cargar la textura: " + request.result);
                        }
                    }
                }

                article.companyName = data.Source;
                if (data.Conversation.Count != 0) {
                    foreach (ConversationJSON conversationJSON in data.Conversation)
                    {
                        article.conversation = new List<Conversation>();
                        Conversation conversation = ScriptableObject.CreateInstance("Conversation") as Conversation;

                        conversation.Type = ConversationType.NONE;
                        conversation.Messages = new List<Messages>();

                        foreach (MessagesJSON message in conversationJSON.Messages)
                        {
                            Messages msg = ScriptableObject.CreateInstance("Messages") as Messages;
                            msg.Name = message.Sender;
                            msg.MessageList = message.MessageList;
                            conversation.Messages.Add(msg);
                        }

                        // Al final sí que hacemos distinción por grupos... hihi, tengo que decírselo a andrea
                        article.conversation.Add(conversation);
                    }
                } 
                else
                {
                    article.convType = article.isTrue ? ConversationType.REACTION_GOOD_ARTICLE : ConversationType.REACTION_BAD_ARTICLE;
                }

                int parsedLanguage = 0;
                if (data.Language == "es") { parsedLanguage = 3; }
                else if(data.Language == "cz") { parsedLanguage = 1; }
                else if(data.Language == "bg") { parsedLanguage = 0; }
                else if(data.Language == "en") { parsedLanguage = 2; }

                if (!_articlesByLanguage.ContainsKey(parsedLanguage)) 
                {
                    _articlesByLanguage.Add(parsedLanguage, new List<ArticleData>());
                }

                _articlesByLanguage[parsedLanguage].Add(article);
            }
        }

        ArticlesCreated = true;
    }

    public void ParseArticles(List<string> jsonData)
    {
        for (int i = 0; i < jsonData.Count; ++i)
        {
            jsonData[i] = jsonData[i].Replace("\"Conversation\":{}", "\"Conversation\":[]");
            jsonData[i] = jsonData[i].Replace("\"Conversation\":{\"Messages\":[]}", "\"Conversation\":[]");
        }

        List<ArticleJSONRoot> data = new List<ArticleJSONRoot>();

        try
        {
            for (int i = 0; i < jsonData.Count; ++i)
            {
                data.Add(JsonConvert.DeserializeObject<ArticleJSONRoot>(jsonData[i]));
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("COULD NOT LOAD JSON FROM DATA BASE. LOADING BACK UP.\nEXCEPTION: " + e.Message);
            TextAsset json = Resources.Load<TextAsset>("Backup/articles");
            data = new List<ArticleJSONRoot>();
            data.Add(JsonConvert.DeserializeObject<ArticleJSONRoot>(json.text));
        }

        CreateArticles(data);
    }

    public ArticleData GetArticleByLanguage(int id)
    {
        return _articlesByLanguage[id][UnityEngine.Random.Range(0, _articlesByLanguage[id].Count)];
    }

    public List<ArticleData> GetAllArticlesByLanguage(int id)
    {
        return _articlesByLanguage[id];
    }
}
