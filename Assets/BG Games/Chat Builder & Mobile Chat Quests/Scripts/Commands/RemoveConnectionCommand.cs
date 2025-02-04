using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.MessageConnection;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class RemoveConnectionCommand : ICommand
    {
        private readonly MessageConnector _messageConnector;
        private readonly ConnectionStartPoint _connectionStartPoint;
        private readonly ConnectionEndPoint _connectionEndPoint;
        private string _startId;

        public RemoveConnectionCommand(MessageConnector messageConnector, ConnectionStartPoint connectionStartPoint,
            ConnectionEndPoint connectionEndPoint)
        {
            _messageConnector = messageConnector;
            _connectionStartPoint = connectionStartPoint;
            _connectionEndPoint = connectionEndPoint;

            _startId = connectionEndPoint.BlockObject.Id;
        }

        public void Execute()
        {
            _messageConnector.gameObject.SetActive(false);
            _messageConnector.IsConnected = false;
            _connectionStartPoint.RemoveConnection(_connectionEndPoint);
            _connectionStartPoint.Id = string.Empty;

        }

        public void Undo()
        {
            _messageConnector.gameObject.SetActive(true);
            _messageConnector.IsConnected = true;

            _connectionStartPoint.ConnectToEndPoint(_connectionEndPoint);
            
            _connectionStartPoint.Id = _startId;
        }
    }
}