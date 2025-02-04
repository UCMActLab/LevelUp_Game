using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class LanguageSelectionPanel : MonoBehaviour
    {
        [SerializeField] private UIButton _selectionButton;
        [SerializeField] private TMP_Dropdown _languagesDropDown;

        public event Action<LanguageType> LanguageSelected;
        private LanguageType _currentLanguage;
        private List<LanguageType> _availableLanguages;
    
        private void Awake()
        {
            _selectionButton.AssignAction(SelectionButtonClickHandler);
        }

        public void Setup(List<LanguageType> languageTypes)
        {
            gameObject.SetActive(true);
            _availableLanguages = languageTypes;
        
            _languagesDropDown.options = languageTypes.Select(value => new TMP_Dropdown.OptionData(value.ToString())).ToList();
            _languagesDropDown.onValueChanged.AddListener(OnLanguageTypeValueChanged);
            _languagesDropDown.value = 0;
        
            _currentLanguage = _currentLanguage = languageTypes[0];
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void SelectionButtonClickHandler()
        {
            LanguageSelected?.Invoke(_currentLanguage);
        }
    
        private void OnLanguageTypeValueChanged(int selectedIndex)
        {
            _currentLanguage = _availableLanguages[selectedIndex];
        }
    }
}
