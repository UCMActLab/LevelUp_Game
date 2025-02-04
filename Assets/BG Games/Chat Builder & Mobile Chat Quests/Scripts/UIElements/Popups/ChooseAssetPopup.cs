using System.Collections.Generic;
using System.IO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.Popups;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class ChooseAssetPopup : PopupBase
    {
        [SerializeField] private ChooseAssetButton buttonChoosePrefab;
        [SerializeField] private Transform placeHolder;
        [SerializeField] private ExitPopupCloseButton closeButton;

        private EditorChatLoader _editorChatLoader;
        
        public void Initialize(List<FileEditInfo> listOfFiles, EditorChatLoader editorChatLoader)
        {
            _editorChatLoader = editorChatLoader;
            closeButton.AssignAction(Hide);
            InstantiateButtons(listOfFiles);
        }

        private void InstantiateButtons(List<FileEditInfo> listOfFiles)
        {
            foreach (var file in listOfFiles)
            {
                var chooseAssetButton = Instantiate(buttonChoosePrefab, placeHolder);
                chooseAssetButton.Setup(Path.GetFileNameWithoutExtension(file.FilePath), file.EditDate);
                chooseAssetButton.AssignAction(() => Action(file.FilePath));
            }
        }

        private void Action(string path)
        {
            _editorChatLoader.OpenEditorFile(path);
        }
    }
}

