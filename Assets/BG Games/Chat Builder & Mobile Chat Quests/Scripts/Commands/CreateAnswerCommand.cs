using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class CreateAnswerCommand : ICommand
    {
        private AnswerBlock _block;

        public CreateAnswerCommand(MessageBlock messageBlock, AnswerFactory answerFactory, Transform parent)
        {
            _block = answerFactory.Create(parent);
            _block.Enabled += messageBlock.AddAnswerToList;
            _block.Disabled += messageBlock.RemoveAnswerFromList;
            _block.PriceTypeChanged += messageBlock.CheckOnSoloNoFreeAnswers;
        }

        public void Execute()
        {
            _block.gameObject.SetActive(true);
            _block.OnEnabled();
        }

        public void Undo()
        {
            _block.gameObject.SetActive(false);
            _block.OnDisabled();
        }
    }
}