using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public abstract class DefaultBlock : BlockObject
    {
        [SerializeField] private DragAndDropSystem _dragAndDropSystem;
        private CommandsHandler _commandsHandler;


        public void Init(CommandsHandler commandsHandler)
        {
            _commandsHandler = commandsHandler;
            _dragAndDropSystem.OnDragEnded += SaveMoveCommand;
        }

        private void SaveMoveCommand(Vector3 from, Vector3 to)
        {
            MoveBlockCommand command = new MoveBlockCommand(transform, from, to);
            _commandsHandler.AddCommand(command);
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id))
            {
                SetId(GuidUtils.GetGuidString());
            }
        }
    }
}