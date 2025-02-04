using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Enums;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{    [Serializable]

    public class FirstPersonSolutionInfo:BlockSolutionInfo
    {
        public bool HaveText;
        public string NextMessageId;
        public AnswerSolutionInfo[] AnswerInfos;
        public string ImageId;
        public ImageFormat ImageFormat;
        public UserType UserType;
        public ForegroundState ForegroundState;

        public Dictionary<LanguageType, string> LocalisationDictionary;
    }
}