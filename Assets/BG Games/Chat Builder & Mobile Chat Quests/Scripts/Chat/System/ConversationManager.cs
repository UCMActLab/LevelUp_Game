using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class ConversationManager : MonoBehaviour
    {
        [Header("Entry point class")] [Space] [SerializeField]
        private AnswerOptionController _answerOptionController;

        [SerializeField] private MessageWritingAnimator _messageWritingAnimator;
        [SerializeField] private MessageContainer _messageContainer;
        [SerializeField] private LanguageSelectionPanel _languageSelectionPanel;
        [SerializeField] public ChatData _chatData;

        [Space] [Range(0.2f, 10f)] [SerializeField]
        private float _responseTimeInSeconds;

        private LanguageType _currentLanguageType = LanguageType.English;
        private readonly int _secondsMultiplier = 1000;
        private string _currentMessageId;
        private MessageSolution _currentMessage;

        private void Start() => Init();

        private void OnDisable() => _answerOptionController.SelectedAnswer -= SubmitAnswer;

        private void Init()
        {
            var uniqueLanguages = GetUniqueLanguagesInChatData();
            int languageCount = uniqueLanguages.Count;

            switch (languageCount)
            {
                case 0:
                    Debug.LogError("Problem with chat data detected. Language dictionaries is empty");
                    break;
                case 1:
                    _currentLanguageType = uniqueLanguages[0];
                    StartConversation();
                    break;
                default:
                    _languageSelectionPanel.Setup(uniqueLanguages);
                    _languageSelectionPanel.LanguageSelected += type =>
                    {
                        _languageSelectionPanel.Disable();
                        _currentLanguageType = type;
                        StartConversation();
                    };
                    break;
            }
        }

        private void StartConversation()
        {
            _answerOptionController.SelectedAnswer += SubmitAnswer;
            _currentMessageId = _chatData.MessageSolutionInfos[0].Id;
            _currentMessage = _chatData.MessageSolutionInfos[0];
            UpdateDialogueView();
        }

        private async void UpdateDialogueView()
        {
            _currentMessage = _chatData.MessageSolutionInfos.FirstOrDefault(message => message.Id == _currentMessageId);
            if (_currentMessage == null) return;

            await StartPrintingSimulation();
            DisplayNextMessage();

            if (_currentMessage.AnswerInfos.Length == 0)
            {
                _currentMessageId = _currentMessage.NextMessageId;
                UpdateDialogueView();
            }
            else
            {
                DisplayAnswers();
            }
        }

        private void DisplayNextMessage()
        {
            if (_currentMessage != null)
            { 
                if(_currentMessage.VideoClip != null)
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, _currentMessage.VideoClip);
                }
                else if (_currentMessage.AudioClip != null)
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, _currentMessage.AudioClip);
                }
                else if (_currentMessage.IsText)
                {
                   
                    _messageContainer.AddMessage(SenderType.Interlocutor, GetMessage(_currentMessage),
                        _currentMessage.BlurType);

                }
                else
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, GetSprite(_currentMessage),
                        _currentMessage.ImagePrice, _currentMessage.BlurType);
                }
            }
        }

        private void DisplayAnswers()
        {
            var nextMessage = _chatData.MessageSolutionInfos.FirstOrDefault(message => message.Id == _currentMessageId);
            if (nextMessage != null)
                _answerOptionController.DisplayAnswers(nextMessage.AnswerInfos, _currentLanguageType);
        }

        private void SubmitAnswer(string answerId)
        {
            var currentMessage = _chatData.MessageSolutionInfos.FirstOrDefault(message => message.Id == _currentMessageId);
            var answerMessage = currentMessage.AnswerInfos.FirstOrDefault(answer => answer.Id == answerId);

            _currentMessageId = answerMessage.NextMessageId;
            _messageContainer.AddMessage(SenderType.Player, GetMessage(answerMessage));
            UpdateDialogueView();
        }

        private async Task StartPrintingSimulation()
        {
            _messageWritingAnimator.Enable();
            await Task.Delay((int)(_responseTimeInSeconds * _secondsMultiplier));
            _messageWritingAnimator.Disable();
        }

        private string GetMessage(MessageSolution messageSolution)
        {
            return messageSolution.LocalisationDictionary.Find(l => l.Key == _currentLanguageType).Value;
        }

        private Sprite GetSprite(MessageSolution messageSolution)
        {
            var texture2D = messageSolution.Texture2D;

            if (texture2D != null) return SpriteCreator.CreateSprite(texture2D);
            Debug.LogWarning("Texture not set in Chat Data!!");
            return null;
        }

        private string GetMessage(AnswerInfo messageSolution)
        {
            return messageSolution.LocalisationDictionary.Find(l => l.Key == _currentLanguageType).Value;
        }

        private List<LanguageType> GetUniqueLanguagesInChatData()
        {
            if (_chatData == null)
            {
                return new List<LanguageType>();
            }

            return _chatData.MessageSolutionInfos
                .SelectMany(messageSolution => messageSolution.LocalisationDictionary
                    .Select(entry => entry.Key)
                    .Concat(messageSolution.AnswerInfos.SelectMany(answerInfo =>
                        answerInfo.LocalisationDictionary.Select(entry => entry.Key)))).Distinct().ToList();
        }
    }
}