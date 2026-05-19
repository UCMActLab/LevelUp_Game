using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class MessageView : MonoBehaviour
    {
        [Space]
        [SerializeField] private Image _background;
        [SerializeField] private HorizontalOrVerticalLayoutGroup _messageLayout;
        [SerializeField] private TMP_Text _nameText = null;
        [SerializeField] private LocalizeStringEvent _nameLocalized;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private LocalizeStringEvent _messageLocalized;
        [SerializeField] private GameObject _imageHolder;
        [SerializeField] private GameObject _videoHolder;
        [SerializeField] private GameObject _audioHolder;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _image;
        [SerializeField] private ImageOnFullScreenAdjuster _adjuster;

        public void Setup(string name, string table, string message, bool isArticleTrue)
        {
            if(name != "")
            {
                if(_nameLocalized == null)
                {
                    _nameLocalized = transform.GetChild(1).GetComponent<LocalizeStringEvent>();
                }
                _nameLocalized.StringReference.SetReference("NAMES", name);
            }

            _messageLocalized.StringReference.SetReference(table, message);

            // VFX
            if (GetComponent<ElectionVFX>())
            {
                if (isArticleTrue)
                    GetComponent<ElectionVFX>().setParticles();

                GetComponent<ElectionVFX>().setGradient(isArticleTrue);
            }
        }

        public void Setup(string name, string message, bool isArticleTrue)
        {


            if (name != "")
            {
                _nameText.SetText(name);
            }
            _messageText.SetText(message);

            // VFX
            if (GetComponent<ElectionVFX>())
            {
                if(isArticleTrue)
                    GetComponent<ElectionVFX>().setParticles();

                    GetComponent<ElectionVFX>().setGradient(isArticleTrue);
            }
        }
    }
}