using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using UnityEngine;

public class MessageParser : MonoBehaviour
{
    [SerializeField]
    ChatData chatData;

    string sourceJSON = "messagesData";

    void Awake()
    {
        Resources.Load(sourceJSON);
        chatData.MessageSolutionInfos[0].LocalisationDictionary[0].Value = ;
    }
}
