using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem
{
    public class ProjectExporter : MonoBehaviour
    {
        [SerializeField] private ImageSavingService _imageSavingService;
        [SerializeField] private ProjectSaver _projectSaver;
        [SerializeField] private SerializationController _chatSerializationController;
        [SerializeField] private BlocksContainer _blocksContainer;
        [SerializeField] private LanguageSwitch _languageSwitch;
        [SerializeField] private PopupsService _popupsService;

        //TODO: Fix export validation
        public void ExportFiles()
        {
            // string folderPath;
            // if (EditorFilePathSaver.TryLoadDirectoryPath(out folderPath))
            // {
            //     StartExport(folderPath);
            // }
            // else
            // {
                _projectSaver.ExportChatEditorFile();
            // }
        }

        private void StartExport(string folderPath)
        {
            if (!_chatSerializationController.TrySerialize(out string[] serializationResult))
            {
                _popupsService.ShowPopup<SerializationErrorPopup>(PopupType.SerializationError);
                return;
            }

            string fileName = _blocksContainer.GetChatId();

            string[] suffixes = _languageSwitch.LanguagesListSo.LanguageDataElements
                .Where(languageData => languageData.IsActive)
                .Select(data => data.LanguageData.Suffix).ToArray();

            for (int i = 0; i < serializationResult.Length; i++)
            {
                string fileNameWithLanguage = $"{fileName}_{suffixes[i]}";
                JsonSavingService.SaveJsonFile(serializationResult[i], folderPath, fileNameWithLanguage);
            }

            SaveImages();
        }

        private void SaveImages()
        {
            _imageSavingService.SaveAllImages(FileNamesCreator.ImagesFolderName);
        }
    }
}