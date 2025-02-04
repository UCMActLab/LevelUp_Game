using System.Collections.Generic;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs
{
    public class MessageStruct
    {
        public string Id { get; set; }
        public UserType UserType { get; set; }
        public PriceType PriceType { get; set; }

        public BlurType BlurMode { get; set; }
        public int ImagePrice { get; set; }

        public object Content { get; set; }

        public List<string> NextMessages { get; set; } = new();
        public List<string> PreviousMessages { get; set; } = new();
    }

    public enum BlurType 
    {
        NoBlur = 0,
        Blur = 1
    }
}