using System.IO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class DateImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private CashedImage _cashedImage;
        public CashedImage CashedImage => _cashedImage;

        public void SetCashedImage(CashedImage cashedImage)
        {
            if (cashedImage == null)
            {
                RemoveImage();
                return;
            }

            _cashedImage = cashedImage;
            _image.sprite = cashedImage.Sprite;
        }
        
        public void LoadImage(string id, ImageFormat format)
        {
            if (EditorFilePathSaver.TryLoadDirectoryPath(out string directoryPath))
            {
                string fileFullPath = GetFileFullPath(id, format, directoryPath);
                if (!File.Exists(fileFullPath))
                {
                    RemoveImage();
                    return;
                }

                Sprite sprite = SpriteCreator.LoadSprite(fileFullPath);

                CashedImage cashedImage = new(id, sprite, format);
                SetCashedImage(cashedImage);

                _cashedImage = cashedImage;
            }
            else
            {
                RemoveImage();
            }
        }

        public void RemoveImage()
        {
            _cashedImage = null;
            _image.sprite = null;
        }
        
        private string GetFileFullPath(string id, ImageFormat imageFormat, string directoryPath)
        {
            string folderPath = Path.Combine(directoryPath, FileNamesCreator.ImagesFolderName);

            string fileName = FileNamesCreator.CreateImageName(id, imageFormat.ToString());

            string fileFullPath = Path.Combine(folderPath, fileName);
            return fileFullPath;
        }
    }
}