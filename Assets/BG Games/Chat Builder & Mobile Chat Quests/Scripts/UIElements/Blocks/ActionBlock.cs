using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public class ActionBlock : DestroyableBlock
    {
        [SerializeField] private TMP_Dropdown _actionTypeDropdown;
        [SerializeField] private DragAndDropSystem _dragAndDropSystem;

        public ActionType ActionType
        {
            get
            {
                string actionTypeString = _actionTypeDropdown.options[_actionTypeDropdown.value].text;
                ActionType actionType = (ActionType)Enum.Parse(typeof(ActionType), actionTypeString);

                return actionType;
            }
        }

        public override void AddNextBlock(BlockObject blockObject)
        {
            NextBlocks.Add(blockObject);
        }

        public override void AddPreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Add(blockObject);
        }

        public override void RemoveNextBlock(BlockObject blockObject)
        {
            NextBlocks.Remove(blockObject);
        }

        public override void RemovePreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Remove(blockObject);
        }

        protected override void RemoveBlock()
        {
            DeleteDestroyableBlockCommand command = new DeleteDestroyableBlockCommand(this);
            command.Execute();
            CommandsHandler.AddCommand(command);
        }

        public void Init(MessageConnectionFactory messageConnectionFactory, CommandsHandler commandsHandler)
        {
            MessageConnectionFactory = messageConnectionFactory;
            CommandsHandler = commandsHandler;

            _dragAndDropSystem.OnDragEnded += SaveMoveCommand;
        }

        private void SaveMoveCommand(Vector3 from, Vector3 to)
        {
            MoveBlockCommand command = new MoveBlockCommand(transform, from, to);
            CommandsHandler.AddCommand(command);
        }

        public void LoadData(ActionSolutionInfo actionSolutionInfo)
        {
            _actionTypeDropdown.value = (int)actionSolutionInfo.ActionType;
            transform.position = actionSolutionInfo.Position;
        }

        public ActionSolutionInfo GetMessageSolutionInfo()
        {
            ActionSolutionInfo stageSolutionInfo = new ActionSolutionInfo
            {
                Id = Id,
                Position = transform.position,
                NextMessageId = ConnectionStartPoint.Id,
                ActionType = (ActionType)_actionTypeDropdown.value
            };
            return stageSolutionInfo;
        }
    }
}