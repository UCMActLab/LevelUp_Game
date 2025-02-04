using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.EditorTopBar;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.SceneManagement;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class ExitPopupShowHandler : MonoBehaviour
    {
        [SerializeField] private MainMenuButton _mainMenuButton;
        [SerializeField] private EditorSceneChanger _editorSceneChanger;
        [SerializeField] private PopupsService _popupsService;

        private void Start()
        {
            _mainMenuButton.AssignAction(OpenExitConfirmationPopup);
        }

        private void OpenExitConfirmationPopup()
        {
            _popupsService.ShowPopup<ExitConfirmationPopup>(PopupType.ExitConfirmation, popup => popup.Initialize(_editorSceneChanger));
        }
    }
}

