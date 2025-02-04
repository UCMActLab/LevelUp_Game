using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class EndBlock : DefaultBlock
    {
        [SerializeField] private ConnectionEndPoint _connectionEndPoint;
        public ConnectionEndPoint ConnectionEndPoint => _connectionEndPoint;

        public override void AddNextBlock(BlockObject blockObject)
        {
        }

        public override void AddPreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Add(blockObject);
        }

        public override void RemoveNextBlock(BlockObject blockObject)
        {
        }

        public override void RemovePreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Remove(blockObject);
        }
    }
}