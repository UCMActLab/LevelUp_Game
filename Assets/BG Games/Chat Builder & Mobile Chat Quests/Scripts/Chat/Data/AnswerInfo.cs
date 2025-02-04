using System;
using System.Collections.Generic;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data
{
    [Serializable]
    public class AnswerInfo
    {
        public string NextMessageId;
        public List<LocalisationEntry> LocalisationDictionary;
        public string Id;
        public bool Free;
    }
}
