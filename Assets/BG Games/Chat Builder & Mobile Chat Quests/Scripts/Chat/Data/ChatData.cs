using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data
{
    [CreateAssetMenu(fileName = "NewChatData", menuName = "Chat/ChatData", order = 0)] [Serializable]
    public class ChatData : ScriptableObject
    {
        public string ChatId;
        public string StartPointLinkId;
        public MessageSolution[] MessageSolutionInfos;
    }

    [Serializable]
    public class LocalisationEntry
    {
        public LanguageType Key;
        public string Value;
    }
}