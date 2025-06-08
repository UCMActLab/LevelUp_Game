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
        [SerializeField] private ImageOnFullScreenAdjuster _adjuster;
        

        public void Setup(string message)
        {
            _messageText.text = message;

            //if (message.IsOneEmoji())
            //{
            //    _messageLayout.padding.top = _emojiTopPadding;
            //    _messageLayout.padding.bottom = _emojiBottomPadding;

            //    _messageText.fontSize = _emojiFontSize;
            //    _messageText.alignment = _emojiLayoutOptions;
            //    _background.enabled = false;
            //}
        }

        public void Setup(Sprite spite)
        {
            if (spite == null) return;

            _imageHolder.SetActive(true);
            _image.sprite = spite;
            _adjuster.SetupProportions();
        }

        public void SetupV(string video)
        {
            if (video == null) return;

            _videoHolder.SetActive(true);
            RenderTexture tex = new RenderTexture(1080, 1080, 32);

            _rawImage.texture = tex;
            _videoPlayer.targetTexture = tex;

            _videoPlayer.url = video;
        }

        public void Setup(AudioClip audio)
        {
            if (audio == null) return;

            _audioHolder.SetActive(true);

            _audioSource.clip = audio;
        }
    }
}