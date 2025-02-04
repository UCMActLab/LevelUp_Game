using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class DeleteDestroyableBlockCommand : ICommand
    {
        private DestroyableBlock _createdBlock;

        public DeleteDestroyableBlockCommand(DestroyableBlock answerBlock)
        {
            _createdBlock = answerBlock;
        }

        public void Execute()
        {
            _createdBlock.gameObject.SetActive(false);
            _createdBlock.OnDisabled();
        }

        public void Undo()
        {
            _createdBlock.gameObject.SetActive(true);
            _createdBlock.OnEnabled();
        }
    }
}