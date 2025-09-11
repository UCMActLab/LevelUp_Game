using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

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

        public void ArticleReadOptions(List<Choice> answers, ArticleDataSetter article)
        {
            Choice read = answers[0];
            Choice skip = answers[1];

            article.OnRead += OnAnswerClicked;
            article.OnSkip += OnAnswerClicked;
            article.OnShare += OnAnswerClicked;

            article.SetUpButtons(read, skip);
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

        public void OnAnswerClicked(Choice answer)
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
