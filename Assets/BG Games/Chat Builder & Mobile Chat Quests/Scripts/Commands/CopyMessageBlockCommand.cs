using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class CopyMessageBlockCommand : ICommand
    {
        private MessageSolutionInfo _bufferMessageSolution;

        public CopyMessageBlockCommand(MessageSolutionInfo messageSolutionInfo)
        {
            _bufferMessageSolution = messageSolutionInfo;
        }

        public void Execute()
        {
        
        }

        public void Undo()
        {
        
        }
    }
}
