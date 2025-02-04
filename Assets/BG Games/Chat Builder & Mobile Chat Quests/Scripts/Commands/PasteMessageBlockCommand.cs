using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class PasteMessageBlockCommand : ICommand
    {
        private readonly ChatMessageBlock _chatMessageBlock;
        private MessageSolutionInfo _messageSolutionInfo;
        private readonly Vector3 _offsetRange = new Vector2(16, 0);

        public PasteMessageBlockCommand(MessageBlockFactory messageBlockFactory, MessageSolutionInfo messageSolutionInfo)
        {
            _chatMessageBlock = messageBlockFactory.Create();
            _chatMessageBlock.Enabled += messageBlockFactory.AddAvailableMessage;
            _chatMessageBlock.Disabled += messageBlockFactory.RemoveAvailableMessage;
            _chatMessageBlock.LoadData(messageSolutionInfo);
            _chatMessageBlock.transform.position += _offsetRange;
        }

        public void Execute()
        {
            _chatMessageBlock.gameObject.SetActive(true);
            _chatMessageBlock.OnEnabled();
        }

        public void Undo()
        {
            _chatMessageBlock.gameObject.SetActive(false);
            _chatMessageBlock.OnDisabled();
        }
    }
}
