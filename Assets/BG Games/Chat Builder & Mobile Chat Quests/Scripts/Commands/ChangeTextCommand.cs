using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class ChangeTextCommand : ICommand
    {
        private readonly TMP_InputField _inputField;
        private readonly string _newText;
        private readonly Dictionary<LanguageType, string> _languageTexts;
        private readonly LanguageType _currentLanguage;
        private readonly string _oldText;

        public ChangeTextCommand(TMP_InputField inputField, string oldText, string newText,
            Dictionary<LanguageType, string> languageTexts = null, LanguageType currentLanguage = default)
        {
            _inputField = inputField;
            _oldText = oldText;
            _newText = newText;
            _languageTexts = languageTexts;
            _currentLanguage = currentLanguage;
        }

        public void Execute()
        {
            _inputField.text = _newText;

            if (_languageTexts is not null)
            {
                _languageTexts[_currentLanguage] = _newText;
            }
        }

        public void Undo()
        {
            _inputField.text = _oldText;

            if (_languageTexts is not null)
            {
                _languageTexts[_currentLanguage] = _oldText;
            }
        }
    }
}