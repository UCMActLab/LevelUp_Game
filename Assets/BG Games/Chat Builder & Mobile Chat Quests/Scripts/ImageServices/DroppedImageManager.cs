using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices
{
    public class DroppedImageManager : MonoBehaviour
    {
        [SerializeField] private ImageSavingService _imageSavingService;
        [SerializeField] private CommandsHandler _commandsHandler;

        private ImageDragAndDropSystem _imageDragAndDropSystem;

        private void Start()
        {
            InitializeHandlers();
        }

        private void OnDestroy()
        {
            _imageDragAndDropSystem.ImageDropped -= OnImageDropped;
            _imageDragAndDropSystem.Dispose();
        }

        private void InitializeHandlers()
        {
            _imageDragAndDropSystem = new ImageDragAndDropSystem();
            _imageDragAndDropSystem.Initialize();
            _imageDragAndDropSystem.ImageDropped += OnImageDropped;
        }

        public void RemoveImageFromCashed(CashedImage cashedImage)
        {
            _imageSavingService.RemoveImage(cashedImage.Id);
        }

        public void OnImageDropped(DroppedImageInfo imageInfo)
        {
            MessageImage messageImage = TryGetMessageImage(imageInfo);

            if (messageImage == null)
            {
                return;
            }

            ImageFormat imageFormat = GetImageFormatFromPath(imageInfo.Path);
            Sprite sprite = SpriteCreator.LoadSprite(imageInfo.Path);

            CashedImage cashedImage = new(GuidUtils.GetGuidString(), sprite, imageFormat);

            ImageCreateCommand command = new ImageCreateCommand(messageImage, cashedImage);
            command.Execute();
            _commandsHandler.AddCommand(command);

            _imageSavingService.CashImage(cashedImage);
        }
        
        public void OnImageDropped(DroppedImageInfo imageInfo, MessageImage messageImage)
        {
            ImageFormat imageFormat = GetImageFormatFromPath(imageInfo.Path);
            Sprite sprite = SpriteCreator.LoadSprite(imageInfo.Path);

            CashedImage cashedImage = new(GuidUtils.GetGuidString(), sprite, imageFormat);

            ImageCreateCommand command = new ImageCreateCommand(messageImage, cashedImage);
            command.Execute();
            _commandsHandler.AddCommand(command);

            _imageSavingService.CashImage(cashedImage);
        }

        private MessageImage TryGetMessageImage(DroppedImageInfo imageInfo)
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out MessageImage messageImage))
                {
                    return messageImage;
                }
            }

            return null;
        }

        private ImageFormat GetImageFormatFromPath(string path)
        {
            int pointIndex = path.LastIndexOf('.');
            string format = path[(pointIndex + 1)..];

            ImageFormat imageFormat = (ImageFormat)Enum.Parse(typeof(ImageFormat), format);

            return imageFormat;
        }
    }
}