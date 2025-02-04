using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.Popups;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils.MessageBlockUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class ChatBlockCreationPopup : BlockCreatingPopup
    {
        [SerializeField] protected BlockCreationButton _creationButton;
        [SerializeField] protected BlockPastingButton _pastingButton;

        public void Initialize(MessageBlockFactory messageBlockFactory, CommandsHandler commandsHandler,
            MessageBlockHandler messageBlockHandler)
        {
            _creationButton.AssignAction(() => SpawnBlock(messageBlockFactory, commandsHandler));
            _pastingButton.AssignAction(() => PasteBlock(messageBlockHandler));
            
            _pastingButton.SetAvailability(messageBlockHandler.CanPaste);
        }

        private void SpawnBlock(MessageBlockFactory messageBlockFactory, CommandsHandler commandsHandler)
        {
            Vector3 position = CameraProvider.MousePositionToWorldPoint();
            ICommand command = new CreateMessageBlockCommand(messageBlockFactory, position);

            command.Execute();
            commandsHandler.AddCommand(command);

            Created?.Invoke();
        }

        private void PasteBlock(MessageBlockHandler messageBlockHandler)
        {
            messageBlockHandler.PasteMessageBlock();

            Created?.Invoke();
        }
    }
}