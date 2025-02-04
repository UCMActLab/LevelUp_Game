using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.CustomVector;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    [Serializable]
    public class ChatSolutionInfo
    {
        public string ChatId;

        public string StartPointLinkId;
        public CustomVector3 StartPosition;

        public CustomVector3 EndPosition;
        public MessageSolutionInfo[] MessageSolutionInfos;
    }
}