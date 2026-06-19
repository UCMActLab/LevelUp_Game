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

    bool _isLoggedIn = false;
    bool _tryingToLogIn = true;

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
        // List<string> json = new List<string>();
        // json.Add(Resources.Load<TextAsset>("Backup/articles").text);

        OnJsonReceived.Invoke(ArticleManager.Instance.LoadAllJsons());
    }

    IEnumerator serverLogin()
    {
        _tryingToLogIn = true;

        string message = "{\n\"user\": \"" + userID + "\",\n\"password\":\"" + userPassword + "\"\n}";

        using (UnityWebRequest www = UnityWebRequest.Post("https://levelup-game.fundacionmaldita.es/api/login", message, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                ConnectionFailed();
                _tryingToLogIn = false;
            }
            else
            {
                serverLoginInfo = JsonUtility.FromJson<LoginData>(www.downloadHandler.text);

                Debug.Log(www.downloadHandler.text);
                Debug.Log("Register complete!");

                _isLoggedIn = true;

                StartCoroutine(serverRequest());
            }
        }

        _tryingToLogIn = false;
    }

    IEnumerator serverRequest()
    {
        int nPage = 1;
        int maxPages = int.MaxValue;
        List<string> jsons = new List<string>();

        bool connectionFailed = false;

        while (nPage <= maxPages)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://levelup-game.fundacionmaldita.es/api/resources?page=" + nPage);

            try
            {
                www.SetRequestHeader("Authorization", serverLoginInfo.data.token);

            }
            catch(Exception e)
            {
                Debug.Log(www.error);
                connectionFailed = true;
                break;
            }

            yield return www.SendWebRequest();
            string[] split = www.downloadHandler.text.Split("\"totalPages\":");
            maxPages = Int32.Parse(split[1].Split(',')[0]);

            if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text.Contains("\"data\":[]"))
            {
                connectionFailed = true;
            }
            else
            {
                jsons.Add(www.downloadHandler.text);
            }
            nPage++;
        }

        if (!connectionFailed) OnJsonReceived.Invoke(jsons);
        else
        {
            ConnectionFailed();
        }
    }

    public IEnumerator PostScoreToDatabase(int score, string countryLabel)
    {
        yield return new WaitUntil(() => _isLoggedIn);
        
        string message = "{\n    \"score\":" + score+",\r\n    \"country\":\""+countryLabel+"\"\r\n}";
        
        // UnityWebRequest.Get("https://levelup-game.fundacionmaldita.es/api/resources?page=" + nPage);
        using (UnityWebRequest www = UnityWebRequest.Post(
            "https://levelup-game.fundacionmaldita.es/api/scores/",
            message, "application/json"))
        {
            try
            {
                www.SetRequestHeader("Authorization", serverLoginInfo.data.token);

            }
            catch (Exception e)
            {
                Debug.Log(www.error);
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    public IEnumerator PostUserToDatabase(string countryLabel)
    {
        yield return new WaitUntil(() => _isLoggedIn);

        string message = "{\"score\":"+1+ ",\r\n\"country\":\"CNT_" + countryLabel + "\"}";

        using (UnityWebRequest www = UnityWebRequest.Post(
            "https://levelup-game.fundacionmaldita.es/api/scores/",
            message, "application/json"))
        {
            try
            {
                www.SetRequestHeader("Authorization", serverLoginInfo.data.token);

            }
            catch (Exception e)
            {
                Debug.Log(www.error);
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}


