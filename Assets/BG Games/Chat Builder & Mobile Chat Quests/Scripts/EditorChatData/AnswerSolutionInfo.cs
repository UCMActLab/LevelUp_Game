using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    [Serializable]
    public class AnswerSolutionInfo
    {
        public string Id;
        public bool Free;
        public string NextMessageId;
        public Dictionary<LanguageType, string> LocalisationDictionary;
    }
}