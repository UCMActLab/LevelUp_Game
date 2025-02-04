using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Dictionaries
{
    public class ChatDictionary
    {
        public string Id { get; set; }
        public string[] FirstMessagesIds { get; set; }
        public string[] LastMessagesIds { get; set; }
        public Dictionary<string,ChatMessageStruct> Messages { get; set; }

        public ChatDictionary(ChatStruct copy)
        {
            Id = copy.Id;
            FirstMessagesIds = copy.FirstMessagesIds;
            LastMessagesIds = copy.LastMessagesIds;
            Messages = copy.Messages.ToDictionary(message => message.Id);
        }
    }
}