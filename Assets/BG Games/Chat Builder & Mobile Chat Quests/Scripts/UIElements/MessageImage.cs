using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class MessageImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _hoverImage;
        [SerializeField] private Sprite _backgroundImage;
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
            _hoverImage.SetActive(false);
        }

        public void RemoveImage()
        {
            _cashedImage = null;
            _image.sprite = _backgroundImage;
            _hoverImage.SetActive(true);
        }
    }
}