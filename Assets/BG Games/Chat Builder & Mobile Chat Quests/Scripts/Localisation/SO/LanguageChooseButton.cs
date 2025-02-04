using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO
{
    public class LanguageChooseButton : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Color _active;
        [SerializeField] private Color _notActive;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        [field:SerializeField] public LanguageType Language { get; private set; }

        public event Action<LanguageChooseButton> Clicked;
        private void Start()
        {
            _button.onClick.AddListener(OnClicked);
        }

        public void Init(LanguageDataSO languageData)
        {
            _text.text = languageData.Name;
            Language = languageData.Language;
        }

        public void TurnActive()
        {
            _background.color = _active;
        }

        public void TurnNotActive()
        {
            _background.color = _notActive;

        }
        private void OnClicked()
        {
            Clicked?.Invoke(this);
            TurnActive();
        }
    
    }
}
