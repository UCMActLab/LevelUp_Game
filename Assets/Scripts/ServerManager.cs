using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// TODO ESTO EN UNA PANTALLA DE CARGA ENTRE IDIOMAS Y EL JUEGO
public class ServerManager : MonoBehaviour
{
    // Codigo de Singleton
    #region Singleton
    private static ServerManager _instance = null;

    public static ServerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ServerManager not present in scene");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField]
    private bool _debugEnabled = false;

    [SerializeField]
    string userID = "admin";
    [SerializeField]
    string userPassword = "j8$K2!";

    LoginData serverLoginInfo;
    [HideInInspector]
    public RootObject serverAnswer;

    [SerializeField]
    public string inkPath = "/Prototipo-27-agosto/Prototipo Agosto";

    [HideInInspector]
    public string inkText = "";

    public UnityEvent<List<string>> OnJsonReceived;

    // Start is called before the first frame update
    void Start()
    {

        if (_debugEnabled)
        {
            ConnectionFailed();
        }
        else
        {
            StartCoroutine(serverLogin());
        }
    }

    private void ConnectionFailed()
    {
        List<string> json = new List<string>();
        json.Add(Resources.Load<TextAsset>("Backup/articles").text);
        OnJsonReceived.Invoke(json);
    }

    IEnumerator serverLogin()
    {
        string message = "{\n\"user\": \"" + userID + "\",\n\"password\":\"" + userPassword + "\"\n}";

        using (UnityWebRequest www = UnityWebRequest.Post("https://levelup.fundacionmaldita.es/api/login", message, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                ConnectionFailed();
            }
            else
            {
                serverLoginInfo = JsonUtility.FromJson<LoginData>(www.downloadHandler.text);

                Debug.Log(www.downloadHandler.text);
                Debug.Log("Register complete!");

                StartCoroutine(serverRequest());
            }
        }
    }

    IEnumerator serverRequest()
    {
        int nPage = 1;
        int maxPages = int.MaxValue;
        List<string> jsons = new List<string>();
        while (nPage <= maxPages)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://levelup.fundacionmaldita.es/api/resources?page=" + nPage);

            www.SetRequestHeader("Authorization", serverLoginInfo.data.token);

            yield return www.SendWebRequest();

            maxPages = Int32.Parse(www.downloadHandler.text.Split("\"totalPages\":")[1].Substring(0, 1));

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                ConnectionFailed();
            }
            else
            {
                jsons.Add(www.downloadHandler.text);
            }
            nPage++;
        }

        OnJsonReceived.Invoke(jsons);
    }

    void processServerAnswer()
    {
        Debug.Log("no he explotado del todo");

        TextAsset jsonAsset = Resources.Load<TextAsset>("FakeNewsVideogame");
        serverAnswer = JsonUtility.FromJson<RootObject>(jsonAsset.text);

        TextAsset inkAsset = Resources.Load<TextAsset>("Languages/" + LanguageSelection.chosenLanguage + inkPath);
        inkText = inkAsset.text;

        for (int i = 0; i < serverAnswer.Articles.Count; ++i)
        {            
            var art = serverAnswer.Articles[i];
            // Replace de los campos que son independientes al idioma escogido
            inkText = inkText.Replace("{\"VAR?\":\"news\"},{ \"VAR =\":\"" + art.ConversationRef + "_theme\"}", "{\"VAR?\":\"" + art.Themes + "\"},{ \"VAR =\":\"" + art.ConversationRef + "_theme\"}");
            inkText = inkText.Replace("true,{\"VAR=\":\"" + art.ConversationRef + "_key\"}", art.Key.ToString().ToLower() + ",{\"VAR=\":\"" + art.ConversationRef + "_key\"}");
            inkText = inkText.Replace("true,{\"VAR=\":\"" + art.ConversationRef + "_true\"}", art.IsFake.ToString().ToLower() + ",{\"VAR=\":\"" + art.ConversationRef + "_true\"}");

            var lang = serverAnswer.Articles[i].ES;
            // Depende del idioma escogido en el menú se sustituye la informacion correspondiente
            if (LanguageSelection.chosenLanguage == Language.czech) lang = serverAnswer.Articles[i].CR;
            else if (LanguageSelection.chosenLanguage == Language.bulgarian) lang = serverAnswer.Articles[i].B;

            // Replace cada uno de los campos: headline, multimedia, source, body, reactions
            inkText = inkText.Replace(art.ConversationRef + "_headline", lang.Headline);
            inkText = inkText.Replace(art.ConversationRef + "_multimedia", lang.Multimedia);
            inkText = inkText.Replace(art.ConversationRef + "_source", lang.Source);
            inkText = inkText.Replace(art.ConversationRef + "_body", lang.Body);
            inkText = inkText.Replace(art.ConversationRef + "_ReactionG1", lang.Reaction_G1);
            inkText = inkText.Replace(art.ConversationRef + "_ReactionG2", lang.Reaction_G2);
            inkText = inkText.Replace(art.ConversationRef + "_ReactionG3", lang.Reaction_G3);
        }
        Debug.Log("hecho :)");
        //SceneManager.LoadScene("ChatDemo");
    }
}


