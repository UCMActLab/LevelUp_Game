using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public abstract class MessageBlock : TextHolderBlock
    {
        [SerializeField] private Button _addAnswerButton;
        [SerializeField] private Transform _answersParent;
        [SerializeField] protected DragAndDropSystem _dragAndDropSystem;
        [Space]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite longBackgroundSprite;
        [SerializeField] private Sprite shortBackgroundSprite;

        private AnswerFactory _answerFactory;
        protected ImageSavingService ImageSavingService;
        protected DroppedImageManager DroppedImageManager;

        protected override void Start()
        {
            base.Start();
            _addAnswerButton.onClick.AddListener(AddAnswer);
        }
        public void Init(AnswerFactory answerFactory, MessageConnectionFactory messageConnectionFactory,
            CommandsHandler commandsHandler, ImageSavingService imageSavingService, DroppedImageManager droppedImageManager)
        {
            _answerFactory = answerFactory;
            MessageConnectionFactory = messageConnectionFactory;
            CommandsHandler = commandsHandler;
            ImageSavingService = imageSavingService;
            DroppedImageManager = droppedImageManager;

            _dragAndDropSystem.OnDragEnded += SaveMoveCommand;
        }
        public void AddAnswerToList(DestroyableBlock answerBlock)
        {
            AddNextBlock(answerBlock);
            answerBlock.AddPreviousBlock(this);
        }
        public void AddAnswer()
        {
            if (NextBlocks.Count >= 3)
                return;
            CreateAnswerCommand command = new CreateAnswerCommand(this, _answerFactory, _answersParent);
            command.Execute();
            CommandsHandler.AddCommand(command);
            DisableConnector();
            CheckOnSoloNoFreeAnswers();
            UpdateBackground();
        }
        public void RemoveAnswerFromList(DestroyableBlock answerBlock)
        {
            RemoveNextBlock(answerBlock);
            answerBlock.RemovePreviousBlock(this);
            CheckOnSoloNoFreeAnswers();
            UpdateBackground();
        }

        public void EnableAnswerButton() => _addAnswerButton.gameObject.SetActive(true);

        public void DisableAnswerButton() => _addAnswerButton.gameObject.SetActive(false);

        public void CheckOnSoloNoFreeAnswers()
        {
            if (NextBlocks.Count <= 0)
                return;
            if(NextBlocks[0] is MessageBlock)
                return;

            List<AnswerBlock> answerBlocks = NextBlocks.GetAnswerBlocks();
            bool haveFreeAnswer=false;
            foreach (AnswerBlock nextBlock in answerBlocks)
            {
                if (nextBlock.PriceType==PriceType.Free)
                {
                    haveFreeAnswer = true;
                    break;
                }
            }

            foreach (AnswerBlock nextBlock in answerBlocks)
            {
                nextBlock.ChangeBackground(!haveFreeAnswer);
            }
        }

        private void UpdateBackground()
        {
            if (NextBlocks.Count <= 0)
                return;
            backgroundImage.sprite = NextBlocks[0] is MessageBlock ? shortBackgroundSprite : longBackgroundSprite;
        }

        private void EnableConnector() => ConnectionStartPoint.gameObject.SetActive(true);

        private void DisableConnector() => ConnectionStartPoint.gameObject.SetActive(false);

        private void SaveMoveCommand(Vector3 from, Vector3 to)
        {
            MoveBlockCommand command = new MoveBlockCommand(transform, from, to);
            CommandsHandler.AddCommand(command);
        }
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
            if (NextBlocks.Count <= 0)
            {
                EnableAnswerButton();
                EnableConnector();
            }
        }

        public override void RemovePreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Remove(blockObject);
        }
        protected AnswerSolutionInfo[] GetAnswersData()
        {
            AnswerSolutionInfo[] answerInfos = new AnswerSolutionInfo[] { };
            if (string.IsNullOrEmpty(ConnectionStartPoint.Id))
            {
                List<AnswerSolutionInfo> answerSolutionInfos = new List<AnswerSolutionInfo>();
                foreach (BlockObject block in NextBlocks)
                {
                    if (block is AnswerBlock answerBlock)
                    {
                        AnswerSolutionInfo answerSolutionInfo = answerBlock.GetAnswerSolutionInfo();
                        answerSolutionInfos.Add(answerSolutionInfo);
                    }
                }

                answerInfos = answerSolutionInfos.ToArray();
            }
            return answerInfos;

        }

    }
}