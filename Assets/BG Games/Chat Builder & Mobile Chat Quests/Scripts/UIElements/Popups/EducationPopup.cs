using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class EducationPopup : MonoBehaviour
    {
        [SerializeField] private BlockCreationPopupShowHandler _blockCreationPopupShowHandler;
        [SerializeField] private GameObject _educationPopover;

        private void OnEnable()
        {
            _blockCreationPopupShowHandler.PopupCreated += PopupCreationHandler;
            _educationPopover.SetActive(true);
        }

        private void OnDisable()
        {
            _blockCreationPopupShowHandler.PopupCreated += PopupCreationHandler;
        }

        private void PopupCreationHandler()
        {
            _educationPopover.SetActive(false);
        }
    }

}
