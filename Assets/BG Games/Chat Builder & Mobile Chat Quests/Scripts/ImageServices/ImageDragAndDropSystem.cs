using System;
using System.Collections.Generic;
using System.Linq;
using B83.Win32;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices
{
    public class ImageDragAndDropSystem
    {
        private readonly string[] _imageExtensions = {
            ".png",
            ".jpg",
            ".jpeg"
        };

        public event Action<DroppedImageInfo> ImageDropped;

        public  void Initialize()
        {
            UnityDragAndDropHook.InstallHook();
            UnityDragAndDropHook.OnDroppedFiles += OnFileDropped;
        }

        public  void Dispose()
        {
            UnityDragAndDropHook.OnDroppedFiles -= OnFileDropped;
            UnityDragAndDropHook.UninstallHook();
        }

        private void OnFileDropped(List<string> filePaths, POINT mousePosition)
        {
            string filePath = filePaths[0];

            var fileInfo = new System.IO.FileInfo(filePath);
            var fileExtension = fileInfo.Extension.ToLower();

            if (!_imageExtensions.Contains(fileExtension))
            {
                return;
            }

            Vector2 point = new Vector2(mousePosition.x, mousePosition.y);
            DroppedImageInfo droppedImageInfo = new(filePath, point);

            ImageDropped?.Invoke(droppedImageInfo);
        }
    }
}