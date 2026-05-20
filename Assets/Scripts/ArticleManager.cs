using DA_Assets.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public string themes;
    public string[] ImmediateFeedbacks;
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

    Dictionary<int, List<ArticleData>> _trueArticlesByLanguage = new Dictionary<int, List<ArticleData>>();
    Dictionary<int, List<ArticleData>> _falseArticlesByLanguage = new Dictionary<int, List<ArticleData>>();

    public bool ArticlesCreated { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        ArticlesCreated = false;

#if !UNITY_EDITOR
        _loadImages = true;
#endif
    }

    public void CreateArticles(List<ArticleJSONRoot> articles)
    {
        StartCoroutine(CreateArticles_Coroutine(articles));
    }

    private IEnumerator CreateArticles_Coroutine(List<ArticleJSONRoot> articles)
    {
        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        int i = 0;

        Dictionary<string, List<ArticleJSONData>> unprocessedArticlesByLanguage = new Dictionary<string, List<ArticleJSONData>>();
        foreach (ArticleJSONRoot root in articles)
        {
            List<ArticleJSONData> data = root.data.data;
            if (data == null) continue;
            
            foreach (ArticleJSONData article in data)
            {
                if (!unprocessedArticlesByLanguage.ContainsKey(article.Language))
                {
                    unprocessedArticlesByLanguage.Add(article.Language, new List<ArticleJSONData>());
                }
                unprocessedArticlesByLanguage[article.Language].Add(article);
            }
        }

        Queue<KeyValuePair<string, List<ArticleJSONData>>> queue = new Queue<KeyValuePair<string, List<ArticleJSONData>>>(unprocessedArticlesByLanguage);

        bool processedPrioritaryArticles = false;
        while (queue.Count > 0)
        {
            KeyValuePair<string, List<ArticleJSONData>> pair = queue.Dequeue();
            string language = pair.Key;
            int parsedLanguage = 0;
            if (language == "es") { parsedLanguage = 3; }
            else if (language == "cz") { parsedLanguage = 1; }
            else if (language == "bg") { parsedLanguage = 0; }
            else if (language == "en") { parsedLanguage = 2; }
            else continue; // quitamos idiomas no reconocidos

            if (!processedPrioritaryArticles && parsedLanguage != (int)LanguageSelection.chosenLanguage)
            {
                queue.Enqueue(pair);
                continue;
            }

            pair.Value.Shuffle();

            Debug.Log(pair.Value.Count + " Artículos '" + language + "' están siendo procesados. ");

            foreach (ArticleJSONData data in pair.Value)
            {
                yield return new WaitForEndOfFrame();

                if (parsedLanguage !=  (int)LanguageSelection.chosenLanguage && !processedPrioritaryArticles)
                {
                    break;
                }

                ArticleData article = ScriptableObject.CreateInstance("ArticleData") as ArticleData;

                article.needsTranslation = false;

                string headline = data.Headline.Trim(' ');
                string body = data.Body.Trim(' ');

                if (headline == string.Empty && body == string.Empty && data.Multimedia == string.Empty) { continue; }

                article.ID = "art_" + i.ToString();
                article.isTrue = data.isTrue;
                article.articleTitle = headline;
                article.articleBody = data.Body.Trim(' ');
                article.theme = data.themes;

                article.feedback = data.ImmediateFeedbacks;

                // article.image = data.Multimedia; TODO: Tratamiento de imágenes
                Debug.Log("Load Images: " + _loadImages + data.Multimedia);
                if (_loadImages && data.Multimedia != null && data.Multimedia != "")
                {
                    if (sprites.ContainsKey(data.Multimedia))
                    {
                        article.articleImage = sprites[data.Multimedia];
                    }
                    else
                    {
                        string finalURL = null;

                        if (data.Multimedia.Contains("drive.google.com/file/d/"))
                        {
                            string imageID = data.Multimedia.Replace("https://drive.google.com/file/d/", "");
                            imageID = imageID.Replace("/view?usp=sharing", "");
                            imageID = imageID.Replace("/view?usp=drive_link", "");
                            imageID = imageID.Replace("image:", "");
                            finalURL = "https://levelup-game.fundacionmaldita.es/api/proxy/gdrive?fileId=" + imageID;
                        }
                        else
                        {
                            finalURL = data.Multimedia;
                        }

                        UnityWebRequest request = UnityWebRequestTexture.GetTexture(finalURL);

                        yield return request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.Success)
                        {
                            yield return new WaitUntil(() => request.downloadHandler.isDone);
                            Texture2D loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                            Sprite spr = Sprite.Create(loadedTexture, new Rect(0.0f, 0.0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
                            article.articleImage = spr;
                            sprites.Add(data.Multimedia, spr);
                        }
                        else
                        {
                            Debug.LogError("No se ha podido cargar la textura: " + request.result);
                            continue;
                        }
                    }
                }

                //string source = data.Source;
                //if (source.Length < 3)
                //{
                //    if (data.isTrue) { source = "newspaper"; }
                //    else source = "social";
                //}

                article.companyName = data.Source;
                if (data.Conversation.Count != 0)
                {
                    article.conversation = new List<Conversation>();
                    foreach (ConversationJSON conversationJSON in data.Conversation)
                    {
                        Conversation conversation = ScriptableObject.CreateInstance("Conversation") as Conversation;

                        conversation.Type = ConversationType.NONE;
                        conversation.Messages = new List<Messages>();

                        foreach (MessagesJSON message in conversationJSON.Messages)
                        {
                            Messages msg = ScriptableObject.CreateInstance("Messages") as Messages;
                            msg.NeedsTranslation = false;
                            msg.Name = message.Sender;
                            msg.MessageList = message.MessageList;
                            conversation.Messages.Add(msg);
                        }

                        article.conversation.Add(conversation);
                    }
                }
                else
                {
                    article.convType = article.isTrue ? ConversationType.REACTION_GOOD_ARTICLE : ConversationType.REACTION_BAD_ARTICLE;
                }

                if (!_articlesByLanguage.ContainsKey(parsedLanguage))
                {
                    _trueArticlesByLanguage.Add(parsedLanguage, new List<ArticleData>());
                    _falseArticlesByLanguage.Add(parsedLanguage, new List<ArticleData>());
                    _articlesByLanguage.Add(parsedLanguage, new List<ArticleData>());
                }

                _articlesByLanguage[parsedLanguage].Add(article);

                if (article.isTrue) _trueArticlesByLanguage[parsedLanguage].Add(article);
                else _falseArticlesByLanguage[parsedLanguage].Add(article);

                ++i;
            }

            Debug.Log(_articlesByLanguage[parsedLanguage].Count + " Artículos '" + language + "' procesados. ");

            if (parsedLanguage == (int)LanguageSelection.chosenLanguage) processedPrioritaryArticles = true;

            if (processedPrioritaryArticles) ArticlesCreated = true;
        }

        foreach (List<ArticleData> articleList in _trueArticlesByLanguage.Values)
        {
            articleList.Shuffle();
        }

        foreach (List<ArticleData> articleList in _falseArticlesByLanguage.Values)
        {
            articleList.Shuffle();
        }

        ArticlesCreated = true;
    }

    /// <summary>
    /// Guarda una lista de strings (JSON) en archivos individuales.
    /// </summary>
    public void SaveJsonList(List<string> jsonStrings)
    {
        for (int i = 0; i < jsonStrings.Count; i++)
        {
            // Creamos un nombre único, por ejemplo: data_0.json, data_1.json...
            string fileName = $"data_{i}.json";
            string fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(fullPath, jsonStrings[i]);
                Debug.Log($"Archivo guardado en: {fullPath}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Error al guardar el archivo {fileName}: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Lee todos los archivos .json que encuentre en el persistentDataPath.
    /// </summary>
    public List<string> LoadAllJsons()
    {
        List<string> loadedJsons = new List<string>();
        string path = Application.persistentDataPath;

        // Obtenemos solo los archivos que terminan en .json
        string[] files = Directory.GetFiles(path, "*.json");

        foreach (string file in files)
        {
            try
            {
                string content = File.ReadAllText(file);
                loadedJsons.Add(content);
            }
            catch (IOException e)
            {
                Debug.LogError($"Error al leer el archivo {file}: {e.Message}");
            }
        }

        return loadedJsons;
    }

    public void ParseArticles(List<string> jsonData)
    {
        SaveJsonList(jsonData);

        for (int i = 0; i < jsonData.Count; ++i)
        {
            jsonData[i] = jsonData[i].Replace("\"Conversation\":{}", "\"Conversation\":[]");
            jsonData[i] = jsonData[i].Replace("\"Conversation\":\"{}\"", "\"Conversation\":[]");
            jsonData[i] = jsonData[i].Replace("\"Conversation\":\"[]\"", "\"Conversation\":[]");
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
            TextAsset json = Resources.Load<TextAsset>("Backup/articles_25.11");
            data = new List<ArticleJSONRoot>();
            data.Add(JsonConvert.DeserializeObject<ArticleJSONRoot>(json.text));
        }

        CreateArticles(data);
    }

    public ArticleData GetArticleByLanguage(int id)
    {
        return _articlesByLanguage[id][UnityEngine.Random.Range(0, _articlesByLanguage[id].Count)];
    }

    public Queue<ArticleData> GetAllArticlesByLanguage(int id)
    {
        _articlesByLanguage[id].Shuffle();
        return new Queue<ArticleData>(_articlesByLanguage[id]);
    }

    public Queue<ArticleData> GetTrueArticlesByLanguage(int id)
    {
        _trueArticlesByLanguage[id].Shuffle();
        return new Queue<ArticleData>(_trueArticlesByLanguage[id]);
    }

    public Queue<ArticleData> GetFalseArticlesByLanguage(int id)
    {
        _falseArticlesByLanguage[id].Shuffle();
        return new Queue<ArticleData>(_falseArticlesByLanguage[id]);
    }
}
