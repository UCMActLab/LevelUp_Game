using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    [Serializable]
    public class MessageSolutionInfo:BlockSolutionInfo
    {
        public bool IsText;

        public bool IsImage;
        public string ImageId;
        public ImageFormat ImageFormat;

        public string NextMessageId;
        public bool BlurType;
        public int ImagePrice;
        public AnswerSolutionInfo[] AnswerInfos;
        public Dictionary<LanguageType, string> LocalisationDictionary;
    }
}