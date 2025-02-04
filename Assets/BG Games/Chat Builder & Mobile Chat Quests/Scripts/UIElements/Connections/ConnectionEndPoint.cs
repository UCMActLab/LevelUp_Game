using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections
{
    public class ConnectionEndPoint : ConnectionPoint
    {
        [SerializeField] private BlockObject _blockObject;

        public BlockObject BlockObject => _blockObject;

        public void ConnectToStartPoint(ConnectionStartPoint connectionStartPoint)
        {
            _blockObject.AddPreviousBlock(connectionStartPoint.BlockObject);
        }

        public void RemoveConnection(ConnectionStartPoint connectionStartPoint)
        {
            _blockObject.RemovePreviousBlock(connectionStartPoint.BlockObject);
        }
    }
}
