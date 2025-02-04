using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class ChooseAssetPopupHandler : MonoBehaviour
    {
        [SerializeField] private PopupsService _popupsService;

        public void OpenExitConfirmationPopup(List<FileEditInfo> files, EditorChatLoader editorChatLoader)
        {
            _popupsService.ShowPopup<ChooseAssetPopup>(PopupType.ChooseAssets, popup => popup.Initialize(files, editorChatLoader));
        }
    }

}
