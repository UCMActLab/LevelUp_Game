using System;
using System.IO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    [Serializable]
    public class ImageContent
    {
        [SerializeField] private MessageImage _messageImage;
        [SerializeField] private Button _imageAddButton;
        [SerializeField] private Button _imageRemoveButton;
        
        private ImageSavingService _imageSavingService;
        private DroppedImageManager _droppedImageManager;
        private CommandsHandler _commandsHandler;
        public CashedImage CashedImage => _messageImage.CashedImage;

        public void Init(ImageSavingService imageSavingService, CommandsHandler commandHandler, DroppedImageManager droppedImageManager)
        {
            _imageSavingService = imageSavingService;
            _commandsHandler = commandHandler;
            _droppedImageManager = droppedImageManager;
            _imageAddButton.onClick.AddListener(AddImage);
            _imageRemoveButton.onClick.AddListener(RemoveImage);
        }
        
        private async void AddImage()
        {
            var (success, filePath) = await RootPathLocator.TryGetImageLoadPath();
            if (success && !string.IsNullOrEmpty(filePath))
            {
                DroppedImageInfo droppedImageInfo = new(filePath, Vector2.zero);
                _droppedImageManager.OnImageDropped(droppedImageInfo, _messageImage);
            }
            else
            {
                Debug.Log("Image selection was canceled or the file path is invalid.");
            }
        }

        public void LoadImage(string id, ImageFormat format)
        {
            string fileFullPath = GetFileFullPath(id, format, FileNamesCreator.ImagesFolderName);
            
            if (!File.Exists(fileFullPath))
            {
                _messageImage.RemoveImage();
                return;
            }

            Sprite sprite = SpriteCreator.LoadSprite(fileFullPath);

            CashedImage cashedImage = new(id, sprite, format);
            _messageImage.SetCashedImage(cashedImage);

            _imageSavingService.CashImage(cashedImage);
        }

        public void GetImageData(out string imageName, out ImageFormat imageFormat)
        {
            imageName = string.Empty;
            imageFormat = default;
            if (_messageImage.CashedImage != null)
            {
                imageName = _messageImage.CashedImage.Id;
                ImageFormat format = _messageImage.CashedImage.ImageFormat;
                imageFormat = format;
            }
        }

        private string GetFileFullPath(string id, ImageFormat imageFormat, string directoryPath)
        {
            string fileName = FileNamesCreator.CreateImageName(id, imageFormat.ToString());

            string fileFullPath = Path.Combine(directoryPath, fileName);
            return fileFullPath;
        }

        private void RemoveImage()
        {
            ImageRemoveCommand command = new ImageRemoveCommand(_messageImage);
            command.Execute();
            _commandsHandler.AddCommand(command);
        }
    }
}