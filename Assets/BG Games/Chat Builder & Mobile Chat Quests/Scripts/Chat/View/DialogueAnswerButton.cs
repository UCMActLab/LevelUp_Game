using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View
{
    public class DialogueAnswerButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _answerText;
        [SerializeField] private Button _button;

        private Choice _answerPicked;

        public event Action<Choice> AnswerClicked;

        public void Setup(Choice answer)
        {
            _answerText.text = answer.text;
            _answerPicked = answer;
            _button.onClick.AddListener(() => AnswerClicked?.Invoke(_answerPicked));
        }
    }
}
