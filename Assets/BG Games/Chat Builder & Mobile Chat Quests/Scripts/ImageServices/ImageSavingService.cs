using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices
{
    public class ImageSavingService : MonoBehaviour
    {
        private LinkedList<CashedImage> _images = new();

        public void CashImage(CashedImage cashedImage)
        {
            _images.AddLast(cashedImage);
        }

        public void RemoveImage(string imageId)
        {
            var cashedImage = _images.FirstOrDefault(image => image.Id == imageId);

            if (cashedImage != null)
            {
                _images.Remove(cashedImage);
            }
        }

        public void SaveAllImages(string folderPath)
        {
            FileIO.TryCreateFolder(folderPath);

            foreach (var image in _images)
            {
                Texture2D texture2D = image.Sprite.texture;
                byte[] bytes = null;

                string fullPath = GetFullPath(folderPath, image);

                string format = image.ImageFormat.ToString();

                switch (format.ToLower())
                {
                    case "png":
                        bytes = texture2D.EncodeToPNG();
                        break;
                    case "jpg":
                    case "jpeg":
                        bytes = texture2D.EncodeToJPG();
                        break;
                }

                FileIO.WriteBytes(fullPath, bytes);
            }
        }

        public static string GetFullPath(string folderPath, CashedImage image)
        {
            string imageName = image.Id;
            string format = image.ImageFormat.ToString();
            string fullPath = FileNamesCreator.CreateImageNameWithPath(folderPath, imageName, format);
            return fullPath;
        }
    }
}

