using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class DialogueAnswerButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _answerText;
        [SerializeField] private Button _button;

        private string _messageId;
        private int _cost;

        public event Action<string, int> AnswerClicked;

        public void Setup(AnswerInfo answerInfo, int cost, LanguageType languageType)
        {
            _cost = cost;
            _messageId = answerInfo.Id;
            _answerText.text = GetMessage(answerInfo, languageType);
            _button.onClick.AddListener(() => AnswerClicked?.Invoke(_messageId, _cost));
        }
    
        private string GetMessage(AnswerInfo messageSolution, LanguageType languageType)
        {
            return messageSolution.LocalisationDictionary.Find(l => l.Key == languageType).Value;
        }
    }
}
