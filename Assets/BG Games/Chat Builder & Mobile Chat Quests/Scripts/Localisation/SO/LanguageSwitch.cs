using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO
{
    public class LanguageSwitch : MonoBehaviour
    {
        [SerializeField] private LanguagesListSO languagesListSo;
        [SerializeField] private BlockFactory[] _blockFactorys;
        [SerializeField] private TMP_Dropdown _languaugesDropdown;
        [Space] [SerializeField] private CommandsHandler _commandsHandler;
    
        public LanguagesListSO LanguagesListSo => languagesListSo;

        public const LanguageType StartLanguage = LanguageType.English;
        private List<LanguageType> _availableLanguages = new();
        private LanguageType _currentLanguage;
    
        private void Awake()
        {
            GenerateLanguageOptions();
        }
    
        public void ChangeLanguage(LanguageType languageType)
        {
            _currentLanguage = languageType;
            _languaugesDropdown.value = _availableLanguages.IndexOf(languageType);
        
            foreach (var block in _blockFactorys)
            {
                foreach (TextHolderBlock textBlock in block.GetTextBlocks())
                {
                    textBlock.ChangeLanguage(languageType);
                }
            }
        }
    
        private void GenerateLanguageOptions()
        {
            foreach (var languageData in languagesListSo.LanguageDataElements)
            {
                if (languageData.IsActive)
                {
                    _availableLanguages.Add(languageData.LanguageData.Language);
                }
            }
        
            SetupDropdown(_availableLanguages);
            _currentLanguage = _availableLanguages[0];
        }
    
        public void RegenerateLanguageOptions(List<LanguageType> languageTypes)
        {
            _availableLanguages = languageTypes;
            SetupDropdown(_availableLanguages);
            _currentLanguage = _availableLanguages[0];
        }

        private void SetupDropdown(List<LanguageType> languages)
        {
            _languaugesDropdown.options = languages.Select(value => new TMP_Dropdown.OptionData(value.ToString())).ToList();
            _languaugesDropdown.onValueChanged.AddListener(OnLanguageTypeValueChanged);
            _languaugesDropdown.value = 0;
        }
    
        private void OnLanguageTypeValueChanged(int selectedIndex)
        {
            ChangeLanguageCommand(_availableLanguages[selectedIndex]);
        }
    
        private void ChangeLanguageCommand(LanguageType languageType)
        {
            ChangeLanguageCommand command = new ChangeLanguageCommand(this, _currentLanguage, languageType);
            command.Execute();
            _commandsHandler.AddCommand(command);
        }
    }
}