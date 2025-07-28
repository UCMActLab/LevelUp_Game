using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
    string userID = "admin";
    [SerializeField]
    string userPassword = "j8$K2!";


    [SerializeField]
    private string urihttps = "https://159.69.190.201";
    [SerializeField]
    private string urihttp = "http://159.69.190.201:8079";
    [SerializeField]
    bool isHttps= false;

    LoginData serverLoginInfo;
    [HideInInspector]
    public RootObject serverAnswer;

    [HideInInspector]
    public string inkText = "";

    // Start is called before the first frame update
    void Start()
    {
        processServerAnswer();
        StartCoroutine(serverLogin());
    }

    IEnumerator serverLogin()
    {
        string message = "{\n\"user\": \"" + userID + "\",\n\"password\":\"" + userPassword + "\"\n}";

        string uri = urihttp;
        if (isHttps)
        {
            uri = urihttps;
        }
        Debug.Log(uri + "/login/" ) ;
        using (UnityWebRequest www = UnityWebRequest.Post(uri + "/login/", message, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
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
        string uri = urihttp;
        if (isHttps)
        {
            uri = urihttps;
        }
        UnityWebRequest www = UnityWebRequest.Get(uri + "/resources/");

        www.SetRequestHeader("Authorization", serverLoginInfo.data.token);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            serverAnswer = JsonUtility.FromJson<RootObject>(www.downloadHandler.text);

            Debug.Log(www.downloadHandler.text);

            StartCoroutine(downloadResources());
            processServerAnswer();
        }

    }

    IEnumerator downloadResources()
    {
        foreach (ItemData i in serverAnswer.data.data)
        {
            foreach(Resource ir in i.resources) 
            {
                //download a Resources
            }
            if (i.answers.Count > 0)
            {
                foreach(Answer a in i.answers)
                {
                    foreach (Resource ar in a.resources)
                    {
                        //download a Resources
                    }
                }
            }
        }

        processServerAnswer();

        // para que no falle
        yield return true;
    }

    void processServerAnswer()
    {
        TextAsset inkAsset = Resources.Load<TextAsset>("Languages/" + LanguageSelection.chosenLanguage + "/main");
        inkText = inkAsset.text;
                                                                          
        inkText = inkText.Replace("\"^WELCOME TO OUR GAME\"", "\"^chao "+ serverAnswer.data.data.Count +"\",\"#\",\"^image:image2\",\"/#\"");
        Debug.Log("Server conected" + serverAnswer.data.data.Count);
        for (int i = 0; i < serverAnswer.data.data.Count; ++i)
        {
            // hacer un diccionario o alguna equivalencia o cambiar lo del server a los idiomas de aqui
            if (serverAnswer.data.data[i].country == LanguageSelection.chosenLanguage.ToString())
            {
                foreach (Resource r in serverAnswer.data.data[i].resources)
                {
                    //if (r.type == "PHOTO") 
                    //    inkString = inkString.Replace(/*el codigo de la noticia en formato "^XXX"*/, /*"^XXX","#","^image:image1","/#" */);
                    //else if (r.type == "AUDIO") 
                    //    inkString = inkString.Replace(/*el codigo de la noticia en formato "^XXX"*/, /*"^XXX","#","^audio:audio","/#" */);
                    //else if (r.type == "VIDEO") 
                    //    inkString = inkString.Replace(/*el codigo de la noticia en formato "^XXX"*/, /*"^XXX","#","^video:video","/#" */);
                }
                foreach (Answer a in serverAnswer.data.data[i].answers)
                {
                    foreach (Resource r in a.resources)
                    {
                        //if (r.type == "PHOTO") 
                        //    inkString = inkString.Replace(/*el codigo de la noticia en formato "^XXX"*/, /*"^XXX","#","^image:image1","/#" */);
                        //else if (r.type == "AUDIO") 
                        //    inkString = inkString.Replace(/*el codigo de la noticia en formato "^XXX"*/, /*"^XXX","#","^audio:audio","/#" */);
                        //else if (r.type == "VIDEO") 
                        //    inkString = inkString.Replace(/*el codigo de la noticia en formato "^XXX"*/, /*"^XXX","#","^video:video","/#" */);
                    }
                }
                // Replace cada uno de los campos: headline, sources, body, Theme, verified
                //inkString = inkString.Replace(/*campo que sea*/, /*serverAnswer.data.data[i].campo que sea*/);
            }
        }
    }
}


