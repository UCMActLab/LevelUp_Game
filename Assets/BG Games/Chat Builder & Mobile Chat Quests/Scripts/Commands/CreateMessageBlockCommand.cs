using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class CreateMessageBlockCommand : ICommand
    {
        private ChatMessageBlock _createdBlock;

        public CreateMessageBlockCommand(MessageBlockFactory messageBlockFactory, Vector3 position)
        {
            _createdBlock = messageBlockFactory.Create();
            _createdBlock.transform.position = position;
            _createdBlock.Enabled += messageBlockFactory.AddAvailableMessage;
            _createdBlock.Disabled += messageBlockFactory.RemoveAvailableMessage;
        }

        public void Execute()
        {
            _createdBlock.gameObject.SetActive(true);
            _createdBlock.OnEnabled();
        }

        public void Undo()
        {
            _createdBlock.gameObject.SetActive(false);
            _createdBlock.OnDisabled();
        }
    }
}