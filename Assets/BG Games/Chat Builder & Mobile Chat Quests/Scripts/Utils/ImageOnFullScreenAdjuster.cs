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
                Debug.LogError("Image dont have sprite");
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
                float inset = (1 / (screenAspect / imageAspect) - 1) / 2;
                _imageTransform.anchorMin = new Vector2(-inset, 0);
                _imageTransform.anchorMax = new Vector2((1 + inset), 1);
            }
            else
            {
                float inset = (1 / (imageAspect / screenAspect) - 1) / 2;
                if (_anchorOnTop)
                {
                    _imageTransform.anchorMin = new Vector2(0, -2 * inset);
                    _imageTransform.anchorMax = new Vector2(1, 1);
                }
                else if(_anchorOnBottom)
                {
                    _imageTransform.anchorMin = new Vector2(0, 0);
                    _imageTransform.anchorMax = new Vector2(1, (1 +2* inset));
                }
                else
                {
                
                
                    _imageTransform.anchorMin = new Vector2(0, -inset);
                    _imageTransform.anchorMax = new Vector2(1, (1 + inset));
                
                }
            }

            _imageTransform.anchoredPosition = Vector2.zero;
            _imageTransform.offsetMin = Vector2.zero;
            _imageTransform.offsetMax = Vector2.zero;
        }


    }
}