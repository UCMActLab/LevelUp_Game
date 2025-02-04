using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public class BackgroundBlock : DestroyableBlock
    {
        [SerializeField] private DragAndDropSystem _dragAndDropSystem;
        [SerializeField] private DateImage _dateImage;

        public string ImageName => _dateImage.CashedImage.Id;

        public BackgroundSolutionInfo GetMessageSolutionInfo() => new()
        {
            Id = Id,
            Position = transform.position,
            NextMessageId = ConnectionStartPoint.Id,
            ImageId = _dateImage.CashedImage.Id,
            ImageFormat = _dateImage.CashedImage.ImageFormat
        };

        protected override void RemoveBlock()
        {
            DeleteDestroyableBlockCommand command = new DeleteDestroyableBlockCommand(this);
            command.Execute();
            CommandsHandler.AddCommand(command);
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

        public void LoadData(BackgroundSolutionInfo solutionInfo, ImageSavingService imageSavingService)
        {
            transform.position = solutionInfo.Position;

            _dateImage.LoadImage(solutionInfo.ImageId, solutionInfo.ImageFormat);
            //_dateImage.Init(imageSavingService, CommandsHandler);
            //_dateImage.LoadImage(solutionInfo.Id,solutionInfo.ImageFormat);
        }
    }
}