using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public class ImageOnFullScreenAdjuster : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _imageTransform;
        [SerializeField] private RectTransform _parentTransform;
        [SerializeField] private bool _onStart = true;
        [SerializeField] private bool _anchorOnTop = false;
        [SerializeField] private bool _anchorOnBottom = false;

        private async void Start()
        {
            if (_onStart)
            {
                await Task.Yield();

                SetupProportions();
            }
        }

        public void SetupProportions()
        {
            if (!_image.sprite)
            {
                Debug.LogError("Image doesn't have a sprite");
                return;
            }

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            if (_parentTransform)
            {
                screenWidth = _parentTransform.rect.width;
                screenHeight = _parentTransform.rect.height;
            }

            float imageWidth = _image.sprite.texture.width;
            float imageHeight = _image.sprite.texture.height;

            float screenAspect = screenWidth / screenHeight;
            float imageAspect = imageWidth / imageHeight;

            if (imageAspect > screenAspect)
            {
                // La imagen es más ancha que la pantalla, se ajusta por el ancho
                float scaleFactor = screenAspect / imageAspect;
                _imageTransform.anchorMin = new Vector2(0, (1 - scaleFactor) / 2);
                _imageTransform.anchorMax = new Vector2(1, (1 + scaleFactor) / 2);
            }
            else
            {
                // La imagen es más alta que la pantalla, se ajusta por la altura
                float scaleFactor = imageAspect / screenAspect;
                _imageTransform.anchorMin = new Vector2((1 - scaleFactor) / 2, 0);
                _imageTransform.anchorMax = new Vector2((1 + scaleFactor) / 2, 1);
            }

            _imageTransform.anchoredPosition = Vector2.zero;
            _imageTransform.offsetMin = Vector2.zero;
            _imageTransform.offsetMax = Vector2.zero;
        }


    }
}