using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public class StartBlock : DefaultBlock
    {
        [SerializeField] private ConnectionStartPoint _connectionStartPoint;

        public ConnectionStartPoint ConnectionStartPoint => _connectionStartPoint;
        private MessageConnectionFactory _messageConnectionFactory;

        public override void AddNextBlock(BlockObject blockObject)
        {
            NextBlocks.Clear();
            NextBlocks.Add(blockObject);
        }

        public override void AddPreviousBlock(BlockObject blockObject)
        {
        }

        public override void RemoveNextBlock(BlockObject blockObject)
        {
            NextBlocks.Clear();
        }

        public override void RemovePreviousBlock(BlockObject blockObject)
        {
        }

        public void SetConnectionsFactory(MessageConnectionFactory messageConnectionFactory)
        {
            _messageConnectionFactory = messageConnectionFactory;
            _connectionStartPoint.Init(_messageConnectionFactory);
        }
    }
}