using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class ChatBlocksContainer : BlocksContainer
    {
        [SerializeField] private MessageConnectionFactory _messageConnectionFactory;
        [SerializeField] private MessageBlockFactory _messageBlockFactory;
        [SerializeField] private CommandsHandler _commandsHandler;
        [Space]
        [SerializeField] private StartBlock _startBlock;
        [SerializeField] private EndBlock _endBlock;

        private string _id;
        public Dictionary<string, ChatMessageBlock> ActiveMessageBlocks => _messageBlockFactory.AvailableMessages;
        public StartBlock StartBlock => _startBlock;
        public EndBlock EndBlock => _endBlock;

        private void Start()
        {
            _startBlock.Init(_commandsHandler);
            _endBlock.Init(_commandsHandler);

            _startBlock.SetConnectionsFactory(_messageConnectionFactory);
        }

        public bool HaveChatId() => string.IsNullOrEmpty(_id);

        public void SetChatId(string id) => _id = id;
        public override string GetChatId() => _id;
    }
}