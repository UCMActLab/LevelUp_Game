using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils.MessageBlockUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class BlockCreationPopupShowHandler : MonoBehaviour
    {
        [SerializeField] private PopupsService _popupsService;
        [SerializeField] private MessageBlockFactory _messageBlockFactory;
        [SerializeField] private CommandsHandler _commandsHandler;
        [SerializeField] private MessageBlockHandler _messageBlockHandler;
        
        private ChatBlockCreationPopup _chatBlockCreationPopup;
        public event Action PopupCreated;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (!_chatBlockCreationPopup)
                {
                    ShowBlockCreationPopup();
                }
                else
                {
                    CloseBlockCreationPopup();
                }
            }
        }

        private void ShowBlockCreationPopup()
        {
            if (_chatBlockCreationPopup != null)
                return;

            _chatBlockCreationPopup = _popupsService.ShowPopup<ChatBlockCreationPopup>(
                PopupType.BlockCreation,
                popup => popup.Initialize(_messageBlockFactory, _commandsHandler, _messageBlockHandler)
            );

            _chatBlockCreationPopup.Created += CloseBlockCreationPopup;
            _chatBlockCreationPopup.transform.position = Input.mousePosition;
            PopupCreated?.Invoke();
        }

        private void CloseBlockCreationPopup()
        {
            if (_chatBlockCreationPopup == null)
                return;

            _chatBlockCreationPopup.Created -= CloseBlockCreationPopup;
            _chatBlockCreationPopup = null;

            _popupsService.CloseTopPopup();
        }
    }
}