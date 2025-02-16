using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data
{
    [Serializable]
    public class MessageSolution
    {
        public List<LocalisationEntry> LocalisationDictionary;
        public AnswerInfo[] AnswerInfos;
        public string Id;
        public bool BlurType;
        public string NextMessageId;
        public bool IsText;
        public bool IsImage;
        public string ImageId;
        public int ImageFormat;
        public int ImagePrice;
        public Texture2D Texture2D;
        public VideoClip VideoClip;
    }
}
