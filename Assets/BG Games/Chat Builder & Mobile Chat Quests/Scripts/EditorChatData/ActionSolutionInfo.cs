using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    [Serializable]
    public class ActionSolutionInfo : BlockSolutionInfo
    {
        public ActionType ActionType;
        public string NextMessageId;
    }
}