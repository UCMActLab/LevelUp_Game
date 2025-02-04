using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.Popups;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class SerializationErrorPopup : PopupBase
    {
        [SerializeField] private ConfirmationPopupConfirmButton _closeButton;

        private void Start()
        {
            _closeButton.AssignAction(Hide);
        }
    }
}
