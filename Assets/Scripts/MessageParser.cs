using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MessageParser : MonoBehaviour
{
    [SerializeField]
    ConversationManager chatManager;

    [SerializeField]
    ChatData sourceChat;

    [SerializeField]
    TextAsset sourceJSON;


    void Awake()
    {
        string json = System.IO.File.ReadAllText("Assets/Resources/" + sourceJSON.name + ".json");
        JSONData data = JsonUtility.FromJson<JSONData>(json);

        for (int i = 0, j = 0; i < sourceChat.MessageSolutionInfos.Length; ++i)
        {
            for(int k = 0; k < data.numMessages; ++k)
            {
                if (sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value == data.messages[k].id)
                {
                    sourceChat.MessageSolutionInfos[i].LocalisationDictionary[0].Value = data.messages[k].value;
                }
            }
            for (int k = 0; k < sourceChat.MessageSolutionInfos[i].AnswerInfos.Length; ++k)
            {
                for (int l = 0; l < data.numAnswers; ++l)
                {
                    if(sourceChat.MessageSolutionInfos[i].AnswerInfos[k].LocalisationDictionary[0].Value == data.answers[l].id)
                    {
                        sourceChat.MessageSolutionInfos[i].AnswerInfos[k].LocalisationDictionary[0].Value = data.answers[l].value;
                    }
                }
            }
            if (sourceChat.MessageSolutionInfos[i].Texture2D != null)
            {
                string filename = data.pictures[j].path;
                var rawData = System.IO.File.ReadAllBytes(filename);
                Texture2D tex = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
                tex.LoadImage(rawData);
                sourceChat.MessageSolutionInfos[i].Texture2D = tex;
                j++;
            }
        }


        chatManager._chatData = sourceChat;

    }
}
