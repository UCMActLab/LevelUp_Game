using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private string _freeImagText = "Free";
        [Space] 
        [SerializeField] private int _emojiTopPadding = -67;
        [SerializeField] private int _emojiBottomPadding = -5;
        [SerializeField] private float _emojiFontSize = 208.5f;
        [SerializeField] private TextAlignmentOptions _emojiLayoutOptions;
        [Space]
        [SerializeField] private Image _background;
        [SerializeField] private HorizontalOrVerticalLayoutGroup _messageLayout;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private GameObject _imageHolder;
        [SerializeField] private GameObject _videoHolder;
        [SerializeField] private GameObject _audioHolder;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _imageCost;
        [SerializeField] private GameObject _coinIcon;
        
        [SerializeField] private UIButton _openImageButton;
        [SerializeField] private GameObject _frameBlur;
        
        [SerializeField] private ImageOnFullScreenAdjuster _adjuster;
        
        private CurrencyService _currencyService;
        private int _imageCostValue;

        public void Setup(string message, bool isBlur)
        {
            _messageText.text = message;

            if (message.IsOneEmoji())
            {
                _messageLayout.padding.top = _emojiTopPadding;
                _messageLayout.padding.bottom = _emojiBottomPadding;

                _messageText.fontSize = _emojiFontSize;
                _messageText.alignment = _emojiLayoutOptions;
                _background.enabled = false;
            }
        }

        public void Setup(Sprite spite, CurrencyService currencyService, bool isBlur, int imagePrice)
        {
            if (spite == null) return;
            _currencyService = currencyService;

            _imageHolder.SetActive(true);
            _image.sprite = spite;
            _adjuster.SetupProportions();

            if (!isBlur)
            {
                _openImageButton.gameObject.SetActive(false);
                _frameBlur.SetActive(false);
            }
            
            _imageCostValue = imagePrice;
            
            if (imagePrice == 0)
            {
                _coinIcon.gameObject.SetActive(false);
                _imageCost.text = _freeImagText;
            }
            else
            {
                _imageCost.text = _imageCostValue.ToString();
            }

            _openImageButton.AssignAction(OpenImageClickHandler);
        }

        public void Setup(VideoClip video)
        {
            if (video == null) return;

            _videoHolder.SetActive(true);
            RenderTexture tex = new RenderTexture((int)video.width, (int)video.height, 32);

            _rawImage.texture = tex;
            _videoPlayer.targetTexture = tex;

            _videoPlayer.clip = video;
        }

        public void Setup(AudioClip audio)
        {
            if (audio == null) return;

            _audioHolder.SetActive(true);

            _audioSource.clip = audio;
        }

        private void OpenImageClickHandler()
        {
            if (_currencyService.Pay(_imageCostValue))
            {
                _openImageButton.gameObject.SetActive(false);
                if(_frameBlur)
                    _frameBlur.SetActive(false);
            }
        }
    }
}