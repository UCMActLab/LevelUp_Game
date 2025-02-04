using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.MessageConnection;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands
{
    public class CreateConnectionCommand : ICommand
    {
        private readonly MessageConnector _messageConnector;
        private readonly ConnectionStartPoint _connectionStartPoint;
        private readonly ConnectionEndPoint _connectionEndPoint;


        public CreateConnectionCommand(MessageConnector messageConnector, ConnectionStartPoint connectionStartPoint,
            ConnectionEndPoint connectionEndPoint)
        {
            _messageConnector = messageConnector;
            _connectionStartPoint = connectionStartPoint;
            _connectionEndPoint = connectionEndPoint;
        }

        public void Execute()
        {
            _messageConnector.gameObject.SetActive(true);
            _messageConnector.IsConnected = true;

            _connectionStartPoint.ConnectToEndPoint(_connectionEndPoint);
        }

        public void Undo()
        {
            _messageConnector.gameObject.SetActive(false);
            _messageConnector.IsConnected = false;

            _connectionStartPoint.RemoveConnection(_connectionEndPoint);
        }
    }
}