using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.MessageConnection;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories
{
    public class MessageConnectionFactory : MonoBehaviour
    {
        [SerializeField] private MessageConnector _connectionPrefab;
        [SerializeField] private Transform _connectionInstancesParent;

        [SerializeField] private CommandsHandler _commandsHandler;

        public MessageConnector CreateNewConnectionLine(ConnectionStartPoint connectionStartPoint)
        {
            MessageConnector connectionInstance = Instantiate(_connectionPrefab, _connectionInstancesParent);
            connectionInstance.Init(connectionStartPoint, _commandsHandler);
            return connectionInstance;
        }
    }
}