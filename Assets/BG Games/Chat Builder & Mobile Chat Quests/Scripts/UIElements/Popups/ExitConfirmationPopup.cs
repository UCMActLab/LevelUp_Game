using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.Popups;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.SceneManagement;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class ExitConfirmationPopup : PopupBase
    {
        [SerializeField] private ExitPopupCloseButton closeButton;
        [SerializeField] private ExitPopupConfirmButton confirmButton;

        public void Initialize(EditorSceneChanger editorSceneChanger)
        {
            closeButton.AssignAction(Hide);
            confirmButton.AssignAction(editorSceneChanger.RestartScene);
        }
    }

}

