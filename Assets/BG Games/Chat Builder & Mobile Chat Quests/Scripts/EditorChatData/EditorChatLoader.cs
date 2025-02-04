using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Utils;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData
{
    public class EditorChatLoader : MonoBehaviour
    {
        [SerializeField] private MessageBlockFactory _messageBlockFactory;
        [SerializeField] private MessageConnectionFactory _messageConnectionFactory;
        [Space]
        [SerializeField] private ChatBlocksContainer _chatBlocksContainer;
        [SerializeField] private ImageSavingService _imageSavingService;
        [SerializeField] private LanguageSwitch _languageSwitch;
        [Space]
        [SerializeField] private ChooseAssetPopupHandler _chooseAssetPopupHandler;
        [SerializeField] private ProjectSaver _projectSaver;

        private ChatDataLoader _chatDataLoader;
        private MessageBlockHandler _messageBlockHandler;
        private ConnectionHandler _connectionHandler;

        private static bool _needToLoadFromFile;
        private static string _filePath = string.Empty;

        private void Start()
        {
            InitializeHandlers();

            if (_needToLoadFromFile)
            {
                DeserializeFiles(_filePath);

                _needToLoadFromFile = false;
                _filePath = string.Empty;
            }
            else
            {
                if (_projectSaver.GetSortedChatAssetsList().Any())
                {
                    _chooseAssetPopupHandler.OpenExitConfirmationPopup(_projectSaver.GetSortedChatAssetsList(), this);
                }
            }
        }

        private void InitializeHandlers()
        {
            _chatDataLoader = new ChatDataLoader();
            _messageBlockHandler = new MessageBlockHandler(_messageBlockFactory);
            _connectionHandler = new ConnectionHandler(_messageConnectionFactory, _chatBlocksContainer, _messageBlockHandler);
        }
        
        public async void LoadEditorFile()
        {
            var (success, filePath) = await RootPathLocator.TryGetChatEditorLoadPathAsync();
            if (success)
            {
                OpenEditorFile(filePath);
            }
            else
            {
                Debug.Log("File selection was canceled.");
            }
        }

        public void OpenEditorFile(string filePath)
        {
            _needToLoadFromFile = true;
            _filePath = filePath;

            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            operation.completed += _ => EditorFilePathSaver.Save(filePath);
        }

        private void DeserializeFiles(string filePath)
        {
            FileIO.TryReadText(filePath, out string result);
            _chatDataLoader.LoadJsonAndCreateScriptableObject(result, filePath);
            ChatSolutionInfo chatSolutionInfo = JsonConvert.DeserializeObject<ChatSolutionInfo>(result);

            if (chatSolutionInfo == null)
                return;

            LoadBlocks(chatSolutionInfo);
        }

        private void LoadBlocks(ChatSolutionInfo chatSolutionInfo)
        {
            _chatBlocksContainer.SetChatId(chatSolutionInfo.ChatId);
            _chatBlocksContainer.StartBlock.transform.position = chatSolutionInfo.StartPosition;
            _chatBlocksContainer.EndBlock.transform.position = chatSolutionInfo.EndPosition;

            if (!string.IsNullOrEmpty(chatSolutionInfo.StartPointLinkId))
            {
                _chatBlocksContainer.StartBlock.ConnectionStartPoint.Id = chatSolutionInfo.StartPointLinkId;
                _connectionHandler.CreateConnectedMessage(chatSolutionInfo.StartPointLinkId,
                    _chatBlocksContainer.StartBlock.ConnectionStartPoint);
            }

            LoadMessages(chatSolutionInfo);
        }

        private void LoadMessages(ChatSolutionInfo chatSolutionInfo)
        {
            foreach (MessageSolutionInfo messageSolutionInfo in chatSolutionInfo.MessageSolutionInfos)
            {
                ChatMessageBlock messageBlock = _messageBlockHandler.GetMessageBlock(messageSolutionInfo.Id);

                messageBlock.LoadData(messageSolutionInfo, _imageSavingService);

                if (!string.IsNullOrEmpty(messageSolutionInfo.NextMessageId))
                {
                    _connectionHandler.CreateConnectedMessage(messageSolutionInfo.NextMessageId, messageBlock.ConnectionStartPoint);
                }
                else
                {
                    foreach (AnswerSolutionInfo answerInfo in messageSolutionInfo.AnswerInfos)
                    {
                        _connectionHandler.CreateAnswer(messageBlock, answerInfo);
                    }
                }
            }
        }
    }
}