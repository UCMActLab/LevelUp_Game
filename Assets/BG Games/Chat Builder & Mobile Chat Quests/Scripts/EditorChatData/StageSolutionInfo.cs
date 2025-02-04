using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    [Serializable]

    public class StageSolutionInfo:BlockSolutionInfo
    {
        public string ImageId;
        public ImageFormat ImageFormat;

        
        public string NextMessageId;
    }
}