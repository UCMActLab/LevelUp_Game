using System;
using System.Collections;
using System.Collections.Generic;
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

[Serializable]
public class _user {
    public string username;
    public string role;
}

[Serializable]
public class _data
{
    public bool success;
    public string token;
    public _user user;
}

[Serializable]
public class serverData
{
    public _data data;
}

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

    string userID = "admin";
    string userPassword = "j8$K2!";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(serverLogin());
    }

    IEnumerator serverLogin()
    {
        //WWWForm form = new WWWForm();
        //form.AddField("username", userID);
        //form.AddField("password", userPassword); 

        string message = "{\n\"user\": \"" + userID + "\",\n\"password\":\"" + userPassword + "\"\n}";

        using (UnityWebRequest www = UnityWebRequest.Post("http://159.69.190.201:8080/login", /*form*/ message, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else 
            {
                serverData serverAnswer = JsonUtility.FromJson<serverData>(www.downloadHandler.text);

                Debug.Log(www.downloadHandler.text);
                Debug.Log("Register complete!");

                
                UnityWebRequest www2 = UnityWebRequest.Get("http://159.69.190.201:8080/resources/");
                
                www2.SetRequestHeader("Authorization", serverAnswer.data.token);
                
                yield return www2.SendWebRequest();

                if (www2.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www2.error);
                    Debug.Log(www2.downloadHandler.text);
                }
                else
                {
                    Debug.Log(www2.downloadHandler.text);
                }
            }
        }
    }
}


