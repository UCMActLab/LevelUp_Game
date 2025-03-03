using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class MessageParser : MonoBehaviour
{
    // Referencia al ConversationManager
    [SerializeField]
    ConversationManager chatManager;

    // Referencia al chat "plantilla"
    [SerializeField]
    ChatData sourceChat;

    // Referencia al JSON con la info real de los mensajes
    [SerializeField]
    TextAsset sourceJSON;

    void Awake()
    {
        // Leemos y serializamos la informacion del JSON
        string json = System.IO.File.ReadAllText("Assets/Resources/" + sourceJSON.name + ".json");
        JSONData data = JsonUtility.FromJson<JSONData>(json);

        // Recorremos el chat plantilla sustituyendo los placeholders con la informacion real del json
        for (int i = 0; i < sourceChat.MessageSolutionInfos.Length; ++i)
        {
            // Sustituimos los mensajes de texto (formato original: message1 -> "hola")
            for(int k = 0; k < data.numMessages; ++k)
            {
                if (sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value == data.messages[k].id)
                {
                    sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value = data.messages[k].value;
                    break;
                }
            }
            // Sustituimos las respuestas (formato original: answer1 -> "hola")
            for (int k = 0; k < sourceChat.MessageSolutionInfos[i].AnswerInfos.Length; ++k)
            {
                for (int l = 0; l < data.numAnswers; ++l)
                {
                    if(sourceChat.MessageSolutionInfos[i].AnswerInfos[k].LocalisationDictionary[0].Value == data.answers[l].id)
                    {
                        sourceChat.MessageSolutionInfos[i].AnswerInfos[k].LocalisationDictionary[0].Value = data.answers[l].value;
                        break;
                    }
                }
            }
            // Sustituimos las imagenes (formato original: picture1 -> (cargamos la imagen gracias al path del json))
            for (int k = 0; k < data.numPictures; ++k)
            {
                if (sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value == data.pictures[k].id)
                {
                    sourceChat.MessageSolutionInfos[i].Texture2D = Resources.Load(data.pictures[k].path) as Texture2D;
                    break;
                }
            }
            // Sustituimos los videos (formato original: video1 -> (cargamos el VideoClip gracias al path del json))
            for (int k = 0; k < data.numVideos; ++k)
            {
                if (sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value == data.videos[k].id)
                {
                    sourceChat.MessageSolutionInfos[i].VideoClip = Resources.Load(data.videos[k].path) as VideoClip;
                    break;                
                }
            }
            // Sustituimos los audios (formato original: video1 -> (cargamos el audio gracias al path del json))
            for (int k = 0; k < data.numAudios; ++k)
            {
                if (sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value == data.audios[k].id)
                {
                    sourceChat.MessageSolutionInfos[i].AudioClip = Resources.Load(data.audios[k].path) as AudioClip;
                    break;
                }
            }
        }

        // Asignamos al ConversationManager el chat ya completo
        chatManager._chatData = sourceChat;
    }
}
