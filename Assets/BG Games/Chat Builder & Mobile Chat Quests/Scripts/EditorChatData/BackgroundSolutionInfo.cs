using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    [Serializable]
    public class BackgroundSolutionInfo : BlockSolutionInfo
    {
        public string ImageId;
        public ImageFormat ImageFormat;


        public string NextMessageId;
    }
}