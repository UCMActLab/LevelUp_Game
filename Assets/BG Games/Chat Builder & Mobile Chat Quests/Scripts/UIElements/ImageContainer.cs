using System;
using System.Collections.Generic;
using System.IO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class ImageContainer : MonoBehaviour
    {
        [SerializeField] private MessageImage _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private Sprite _testImage;
        [SerializeField] private ImageSavingService _imageSavingService;
        
        private List<MessageImage> _messages = new();

        private void Awake()
        {
            if (EditorFilePathSaver.TryLoadDirectoryPath(out string directoryPath))
            {
                string folderPath = Path.Combine(directoryPath, FileNamesCreator.ImagesFolderName);
                
                foreach (string filePath in Directory.GetFiles(folderPath))
                {
                    string file = Path.GetFileNameWithoutExtension(filePath);
                    string extension = Path.GetExtension(filePath)[1..];
                    
                    Debug.Log(filePath);
                    
                    Sprite sprite = SpriteCreator.LoadSprite(filePath);

                    CashedImage cashedImage = new(file, sprite, (ImageFormat)Enum.Parse(typeof(ImageFormat), extension));
                    AddNewImage(cashedImage);

                    _imageSavingService.CashImage(cashedImage);
                }
            }
        }

        public void AddNewImage()
        {
            MessageImage messageImage = Instantiate(_prefab, _parent);
            
            _messages.Add(messageImage);
        }

        public void AddNewImage(CashedImage cashedImage)
        {
            MessageImage messageImage = Instantiate(_prefab, _parent);
            messageImage.SetCashedImage(cashedImage);
            
            _messages.Add(messageImage);
        }
        
        private string GetFileFullPath(string filePath, string directoryPath)
        {
            string folderPath = Path.Combine(directoryPath, FileNamesCreator.ImagesFolderName);
            string file = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            
            string fileName = FileNamesCreator.CreateImageName(file, extension);

            string fileFullPath = Path.Combine(folderPath, fileName);
            return fileFullPath;
        }
    }
}