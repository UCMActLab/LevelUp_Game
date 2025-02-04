using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    public class MessageBlockHandler
    {
        private readonly MessageBlockFactory _messageBlockFactory;

        public MessageBlockHandler(MessageBlockFactory messageBlockFactory)
        {
            _messageBlockFactory = messageBlockFactory;
        }

        public ChatMessageBlock GetMessageBlock(string id)
        {
            return _messageBlockFactory.AvailableMessages.TryGetValue(id, out var availableMessage) ? availableMessage : CreateMessageBlock(id);
        }

        private ChatMessageBlock CreateMessageBlock(string id)
        {
            ChatMessageBlock messageBlock = _messageBlockFactory.Create(id);
            _messageBlockFactory.AddAvailableMessage(messageBlock);
            messageBlock.Disabled += _messageBlockFactory.RemoveAvailableMessage;
            return messageBlock;
        }
    }

}
