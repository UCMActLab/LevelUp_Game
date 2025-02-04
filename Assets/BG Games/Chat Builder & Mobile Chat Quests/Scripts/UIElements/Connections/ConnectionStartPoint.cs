using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections
{
    public class ConnectionStartPoint : ConnectionPoint, IPointerDownHandler
    {
        [SerializeField] private BlockObject _blockObject;
        public string Id;
        private MessageConnectionFactory _messageConnectionFactory;

        public BlockObject BlockObject => _blockObject;

        public void Init(MessageConnectionFactory messageConnectionFactory)
        {
            _messageConnectionFactory = messageConnectionFactory;
        }

        public void ConnectToEndPoint(ConnectionEndPoint connectionEndPoint)
        {
            _blockObject.AddNextBlock(connectionEndPoint.BlockObject);

            Id = connectionEndPoint.BlockObject.Id;
            connectionEndPoint.ConnectToStartPoint(this);

            if (_blockObject is MessageBlock messageBlock)
            {
                messageBlock.DisableAnswerButton();
            }
        }

        public void RemoveConnection(ConnectionEndPoint connectionEndPoint)
        {
            _blockObject.RemoveNextBlock(connectionEndPoint.BlockObject);
            connectionEndPoint.RemoveConnection(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _messageConnectionFactory.CreateNewConnectionLine(this);
        }
    }
}