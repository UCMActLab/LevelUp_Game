using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Utils;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Shortcuts;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;
using Newtonsoft.Json;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem
{
    public class ProjectSaver : MonoBehaviour
    {
        [SerializeField] private ImageSavingService _imageSavingService;
        [SerializeField] private SerializationController _chatSerializationController;
        [SerializeField] private ShortcutDetector _shortcutDetector;
        
        private const string SaveFileName = "FileAssetsInfoData.json";
        private ChatDataLoader _chatDataLoader;

        private void Start()
        {
            _chatDataLoader = new ChatDataLoader();
            _shortcutDetector.AddListener(ShortcutAction.Save, SaveChatEditorFile);
        }

        private void OnDestroy()
        {
            _shortcutDetector.RemoveListener(ShortcutAction.Save, SaveChatEditorFile);
        }
        
        public List<FileEditInfo> GetSortedChatAssetsList()
        {
            var fileEditInfos = LoadFromFile();
    
            var validFileInfos = fileEditInfos
                .Where(info => File.Exists(info.FilePath))
                .OrderByDescending(info => DateTime.Parse(info.EditDate)) 
                .ToList();
    
            return validFileInfos;
        }
        
        public async void ExportChatEditorFile()
        {
            var (success, newFilePath) = await EditorFilePathSaverSystem.TryAddFilePath(SaveEditorFiles);
            if (!success)
            {
                Debug.Log("Failed to save the Chat Editor file.");
            }
        }
        
        public async void SaveChatEditorFile()
        {
            if (EditorFilePathSaver.TryGetPath(out string filePath))
            {
                SaveEditorFiles(filePath);
            }
            else
            {
                var (success, newFilePath) = await EditorFilePathSaverSystem.TryAddFilePath(SaveEditorFiles);
                if (!success)
                {
                    Debug.Log("Failed to save the Chat Editor file.");
                }
            }
        }

        private void SaveEditorFiles(string filePath)
        {
            NormalizeFilePath(filePath);
            SaveImages();

            var folderPath = FileNamesCreator.CreateDirectoryPath(filePath);
            
            _chatSerializationController.SerializeEditorFiles(out string serializationResult, out string id);
           
            var fileName = Path.GetFileName(filePath);
            JsonSavingService.SaveJsonEditorFile(serializationResult, folderPath, fileName);
            EditorFilePathSaver.Save(Path.Combine(folderPath, fileName));
            _chatDataLoader.LoadJsonAndCreateScriptableObject(serializationResult, fileName);
            
            TrySaveChatEditorFile(filePath);
        }
        
        private void NormalizeFilePath(string filePath) => filePath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        private void SaveImages() => _imageSavingService.SaveAllImages(FileNamesCreator.ImagesFolderName);
        private string GetSaveFilePath() => Path.Combine(Application.persistentDataPath, SaveFileName);

        private void TrySaveChatEditorFile(string filePath)
        {
            var fileEditInfos = LoadFromFile();
            
            if (fileEditInfos.Any(info => info.FilePath == filePath))
                return;
            
            fileEditInfos.Add(new FileEditInfo
            {
                FilePath = filePath,
                EditDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            
            SaveToFile(fileEditInfos);
        }
        
        private List<FileEditInfo> LoadFromFile()
        {
            var filePath = GetSaveFilePath();
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<FileEditInfo>>(json) ?? new List<FileEditInfo>();
            }
            return new List<FileEditInfo>();
        }
        
        private void SaveToFile(List<FileEditInfo> fileEditInfos)
        {
            var json = JsonConvert.SerializeObject(fileEditInfos, Formatting.Indented);
            File.WriteAllText(GetSaveFilePath(), json);
        }
    }
    
    [Serializable]
    public class FileEditInfo
    {
        public string FilePath;
        public string EditDate; 
    }
}