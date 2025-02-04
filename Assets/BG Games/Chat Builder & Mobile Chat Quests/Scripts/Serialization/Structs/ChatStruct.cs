using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs
{
    public class ChatStruct
    {
        public string Id { get; set; }
        public string[] FirstMessagesIds{ get; set; }
        public string[] LastMessagesIds { get; set; }
        public List<ChatMessageStruct> Messages { get; set; }
    }
}