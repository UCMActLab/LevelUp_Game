using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using Ink.Runtime;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class AnswerOptionController : MonoBehaviour
    {
        [SerializeField] private DialogueAnswerButton _dialogueAnswerButtonPrefab;
        [SerializeField] private Transform _parent;

        private List<DialogueAnswerButton> _dialogueAnswerButtons=new();

        public event Action<Choice> SelectedAnswer;

        public void DisplayAnswers(List<Choice> answers)
        {
            foreach (var info in answers)
            {
                SpawnDialogueAnswerButton(info);
            }
        }

        private void SpawnDialogueAnswerButton(Choice answerChoice)
        {
            if(_parent != null)
            {
                var answer = Instantiate(_dialogueAnswerButtonPrefab, _parent);

                answer.AnswerClicked += OnAnswerClicked;
                answer.Setup(answerChoice);
                _dialogueAnswerButtons.Add(answer);
            }
        }

        private void OnAnswerClicked(Choice answer)
        {
            SelectedAnswer?.Invoke(answer);
            DestroyAnswers();
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
