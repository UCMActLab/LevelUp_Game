using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Shortcuts;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class CommandsHandler : MonoBehaviour
    {
        [SerializeField] private ShortcutDetector _shortcutDetector;

        private Stack<ICommand> _commands = new Stack<ICommand>();
        private Stack<ICommand> _undoneCommands = new Stack<ICommand>();

        private void Start()
        {
            _shortcutDetector.AddListener(ShortcutAction.Undo, UndoCommand);
            _shortcutDetector.AddListener(ShortcutAction.Redo, RedoCommand);
        }

        private void OnDestroy()
        {
            _shortcutDetector.RemoveListener(ShortcutAction.Undo, UndoCommand);
            _shortcutDetector.RemoveListener(ShortcutAction.Redo, RedoCommand);
        }

        public void AddCommand(ICommand newCommand)
        {
            if (_undoneCommands.Count > 0)
                _undoneCommands.Clear();

            _commands.Push(newCommand);
        }

        public void UndoCommand()
        {
            if (_commands.Count <= 0)
                return;

            ICommand undoneCommand = _commands.Pop();
            undoneCommand.Undo();
            _undoneCommands.Push(undoneCommand);
        }

        public void RedoCommand()
        {
            if (_undoneCommands.Count <= 0)
                return;

            ICommand redoneCommand = _undoneCommands.Pop();
            redoneCommand.Execute();
            _commands.Push(redoneCommand);
        }
    }
}