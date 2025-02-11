using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using UnityEngine;

public class MessageParser : MonoBehaviour
{
    [SerializeField]
    string chatData = "Assets/BG Games/Chat Builder & Mobile Chat Quests/ChatDataObjects/PruebaChat1.asset";

    [SerializeField]
    string sourceJSON = "Assets/Resources/messagesData.json";

    void Awake()
    {
        string json = System.IO.File.ReadAllText(sourceJSON);
        JSONData data = JsonUtility.FromJson<JSONData>(json);

        string chat = System.IO.File.ReadAllText(chatData);

        for (int i = 0; i < data.numMessages; ++i)
        {
            chat = chat.Replace("message" + (i + 1), data.messages[i].value);
        }
        for (int i = 0; i < data.numAnswers; ++i)
        {
            chat = chat.Replace("answer" + (i + 1), data.answers[i].value);
        }

        System.IO.File.WriteAllText(chatData, chat);
    }
}
