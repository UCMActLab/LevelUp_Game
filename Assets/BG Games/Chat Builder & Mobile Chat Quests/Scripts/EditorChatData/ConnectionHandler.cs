using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.MessageConnection;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    public class ConnectionHandler
    {
        private readonly MessageConnectionFactory _messageConnectionFactory;
        private readonly ChatBlocksContainer _chatBlocksContainer;
        private readonly MessageBlockHandler _messageBlockHandler;

        public ConnectionHandler(MessageConnectionFactory messageConnectionFactory,
            ChatBlocksContainer chatBlocksContainer,
            MessageBlockHandler messageBlockHandler)
        {
            _messageConnectionFactory = messageConnectionFactory;
            _chatBlocksContainer = chatBlocksContainer;
            _messageBlockHandler = messageBlockHandler;
        }

        public void CreateConnectedMessage(string nextMessageId, ConnectionStartPoint connectionStartPoint)
        {
            ConnectionEndPoint connectionEndPoint = GetConnectionEndPoint(nextMessageId);
            MessageConnector connector = _messageConnectionFactory.CreateNewConnectionLine(connectionStartPoint);

            connectionStartPoint.ConnectToEndPoint(connectionEndPoint);
            connector.Connect(connectionEndPoint);
        }

        public void CreateAnswer(ChatMessageBlock messageBlock, AnswerSolutionInfo answerInfo)
        {
            messageBlock.AddAnswer();
            if (messageBlock.NextBlocks[^1] is AnswerBlock answerBlock)
            {
                answerBlock.SetInfo(answerInfo);
                ConnectionEndPoint connectionEndPoint = GetConnectionEndPoint(answerInfo.NextMessageId);

                MessageConnector connector = _messageConnectionFactory.CreateNewConnectionLine(answerBlock.ConnectionStartPoint);
                connector.Connect(connectionEndPoint);

                answerBlock.ConnectionStartPoint.ConnectToEndPoint(connectionEndPoint);
            }
        }

        private ConnectionEndPoint GetConnectionEndPoint(string id)
        {
            return _chatBlocksContainer.EndBlock.Id == id ? _chatBlocksContainer.EndBlock.ConnectionEndPoint : _messageBlockHandler.GetMessageBlock(id).ConnectionEndPoint;
        }
    }

}
