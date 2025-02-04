using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Shortcuts;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils.MessageBlockUtilities
{
    public class MessageBlockHandler : MonoBehaviour
    {
        [SerializeField] private MessageBlockFactory _messageBlockFactory;
        [SerializeField] private ShortcutDetector _shortcutDetector;
        [SerializeField] private CommandsHandler _commandsHandler;

        private ChatMessageBlock _currentBlock;
        private MessageSolutionInfo _currentMessageSolutionInfo;
        
        [SerializeField] private List<ChatMessageBlock> _selectedBlocks = new();
        private List<MessageSolutionInfo> _copiedMessageSolutionInfos = new();

        public List<ChatMessageBlock> AllMessages { get; private set; } = new();
        public bool CanPaste => _copiedMessageSolutionInfos.Count > 0;

        private void Awake() => Init();

        private void Init() => _messageBlockFactory.CreatedMessageBlock += OnCreatedMessageBlock;

        private void Start()
        {
            _shortcutDetector.AddListener(ShortcutAction.Copy, CopyMessageBlock);
            _shortcutDetector.AddListener(ShortcutAction.Paste, PasteMessageBlock);
            _shortcutDetector.AddListener(ShortcutAction.Delete, DeleteMessageBlock);
        }

        private void OnDestroy() => _messageBlockFactory.CreatedMessageBlock -= OnCreatedMessageBlock;
        
        private void CopyMessageBlock()
        {
            _copiedMessageSolutionInfos.Clear();
            foreach (var block in _selectedBlocks)
            {
                MessageSolutionInfo messageSolutionInfo = block?.GetMessageSolutionInfo();
                if (messageSolutionInfo != null)
                {
                    _copiedMessageSolutionInfos.Add(messageSolutionInfo);
                }
            }
        }

        public void PasteMessageBlock()
        {
            if (!CanPaste)
                return;

            foreach (var messageSolutionInfo in _copiedMessageSolutionInfos)
            {
                var createMessageBlockCommand = new PasteMessageBlockCommand(_messageBlockFactory, messageSolutionInfo);
                createMessageBlockCommand.Execute();
                _commandsHandler.AddCommand(createMessageBlockCommand);
            }
        }

        private void DeleteMessageBlock()
        {
            Debug.Log("delete");
            foreach (var selectedBlock in _selectedBlocks)
            {
                selectedBlock.Remove();
                selectedBlock.OnRemoved -= RemovingBlockHandler;
                selectedBlock.OnDragging -= DraggingBlocksHandler;
            }
            _selectedBlocks.Clear();
            DeselectBlocks();
        }

        private void OnCreatedMessageBlock(ChatMessageBlock block)
        {
            block.SelectedMessageBlock += OnSelectedBlock;
            AllMessages.Add(block);
        }
        
        public void ClearSelection()
        {
            DeselectBlocks();
            foreach (var selectedBlock in _selectedBlocks)
            {
                selectedBlock.OnRemoved -= RemovingBlockHandler;
                selectedBlock.OnDragging -= DraggingBlocksHandler;
            }

            _selectedBlocks.Clear();
        }

        private void OnSelectedBlock(ChatMessageBlock block, bool isMultipleSelection)
        {
            if (isMultipleSelection)
            {
                if (!_selectedBlocks.Contains(block))
                {
                    _selectedBlocks.Add(block);
                    block.OnRemoved += RemovingBlockHandler;
                    block.OnDragging += DraggingBlocksHandler;
                }
            }
            else
            {
               AddSingleBlock(block);
            }
        }

        private void AddSingleBlock(ChatMessageBlock block)
        {
            ClearSelection();
                
            if (!_selectedBlocks.Contains(block))
            {
                _selectedBlocks.Add(block);
            }
                
            block.EnableOutline();
        }

        private void RemovingBlockHandler(ChatMessageBlock block)
        {
            foreach (var selectedBlock in _selectedBlocks)
            {
                if (selectedBlock != block)
                {
                    selectedBlock.Remove();
                    selectedBlock.OnRemoved -= RemovingBlockHandler;
                    selectedBlock.OnDragging -= DraggingBlocksHandler;
                }
            }
            _selectedBlocks.Clear();
            DeselectBlocks();
        }

        private void DraggingBlocksHandler(ChatMessageBlock block, Vector3 offset)
        {
            foreach (var selectedBlock in _selectedBlocks)
            {
                if (selectedBlock != block)
                {
                    selectedBlock.DragAndDropSystem.MoveBlock(offset);
                }
            }
        }
        
        private void DeselectBlocks()
        {
            foreach (var block in AllMessages)
            {
                block.DisableOutline();
            }
        }
    }
}

