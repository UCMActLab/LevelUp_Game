using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class AnswerOptionController : MonoBehaviour
    {
        [SerializeField] private CurrencyService _currencyService;
        [SerializeField] private DialogueAnswerButton _dialogueAnswerButtonPrefab;
        [SerializeField] private DialogueAnswerButton _paidDialogueAnswerButtonPrefab;
        [SerializeField] private Transform _parent;

        private List<DialogueAnswerButton> _dialogueAnswerButtons=new();

        public event Action<string> SelectedAnswer;

        public void DisplayAnswers(AnswerInfo[] answerInfos, LanguageType languageType)
        {
            foreach (var info in answerInfos)
            {
                SpawnDialogueAnswerButton(info, languageType);
            }
        }

        private void SpawnDialogueAnswerButton(AnswerInfo answerInfo, LanguageType languageType)
        {
            var prefab = DefiningResponseType(answerInfo);
            var answer = Instantiate(prefab, _parent);

            answer.AnswerClicked += OnAnswerClicked;
            answer.Setup(answerInfo, answerInfo.Free ? 0 : _currencyService.GetAnswerCost(), languageType);
            _dialogueAnswerButtons.Add(answer);
        }

        private DialogueAnswerButton DefiningResponseType(AnswerInfo answerInfo)
        {
            return answerInfo.Free ? _dialogueAnswerButtonPrefab : _paidDialogueAnswerButtonPrefab;
        }

        private void OnAnswerClicked(string answerId, int cost)
        {
            if (_currencyService.Pay(cost))
            {
                SelectedAnswer?.Invoke(answerId);
                DestroyAnswers();
            }
        }

        private void DestroyAnswers()
        {
            foreach (var answer in _dialogueAnswerButtons)
            {
                answer.AnswerClicked -= OnAnswerClicked;
                Destroy(answer.gameObject);
            }

            _dialogueAnswerButtons.Clear();
        }
    }
}
