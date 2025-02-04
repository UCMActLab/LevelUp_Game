using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public abstract class TextHolderBlock:DestroyableBlock
    {
        [SerializeField] protected TMP_InputField InputField;

        private LanguageType _currentLanguage = LanguageSwitch.StartLanguage;

        public Dictionary<LanguageType, string> LocalisationDictionary { get; protected set; } = new();
        protected string LastText;
  
        protected void OnInputFieldChanged(string newText)
        {
            ChangeTextCommand command = new ChangeTextCommand(InputField, LastText, newText,LocalisationDictionary,_currentLanguage);
            command.Execute();
            LastText = newText;
            CommandsHandler.AddCommand(command);
        }

        public virtual void ChangeLanguage(LanguageType newLanguage)
        {
            LocalisationDictionary.TryAdd(newLanguage, "");
            LastText = "";
            _currentLanguage = newLanguage;
            InputField.text = LocalisationDictionary[newLanguage];
        }
        
        public abstract string Content { get; }

    }
}