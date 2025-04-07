using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Video;
using static System.Net.WebRequestMethods;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class ConversationManager : MonoBehaviour
    {
        [Header("Entry point class")] [Space] [SerializeField]
        private AnswerOptionController _answerOptionController;

        [SerializeField] private MessageWritingAnimator _messageWritingAnimator;
        [SerializeField] private MessageContainer _messageContainer;
        
        // ink
        [SerializeField] public TextAsset inkJSONAsset;
        public Story story;

        [Space] [Range(0.2f, 10f)] [SerializeField]
        private float _responseTimeInSeconds;

        private readonly int _secondsMultiplier = 1000;
        private MessageSolution _currentMessage;

        private bool _isWriting = false;
        private float _currentTime = 0;

        private void Start() => Init();

        private void OnDisable() => _answerOptionController.SelectedAnswer -= SubmitAnswer;

        private void Init()
        {
            StartConversation();
        }

        private void StartConversation()
        {
            story = new Story(inkJSONAsset.text);
            _answerOptionController.SelectedAnswer += SubmitAnswer;
            StartCoroutine(UpdateDialogueView());
        }

        IEnumerator UpdateDialogueView()
        {
            while (story.canContinue)
            {
                // Continue gets the next line of the story
                string text = story.Continue();
                // This removes any white space from the text.
                text = text.Trim();
                // Display the text on screen!
                _currentMessage = new MessageSolution();
                _currentMessage.text = text;

                yield return StartCoroutine(StartPrintingSimulation());
                DisplayNextMessage();

                handleTags();
            }

            // Display all the choices, if there are any!
            if (story.currentChoices.Count > 0)
            {
                DisplayAnswers(story.currentChoices);
            }
            // If we've read all the content and there's no choices, the story is finished!
            else
            {
                Debug.Log("FIN");
            }
        }

        void handleTags()
        {
            foreach (string tag in story.currentTags)
            {
                _currentMessage = new MessageSolution();

                string[] splitTag = tag.Split(':');
                string key = splitTag[0].Trim();
                string value = splitTag[1].Trim();

                if (key == "image")
                    _currentMessage.Texture2D = Resources.Load(value) as Texture2D;
                else if (key == "video")
                {
                    Debug.Log(value);
                    _currentMessage.VideoURL = "https://drive.google.com/uc?export=download&id=" + value;
                }
                else if (key == "audio")
                    _currentMessage.AudioClip = Resources.Load(value) as AudioClip;

                DisplayNextMessage();
            }
        }

        private void DisplayNextMessage()
        {
            if (_currentMessage != null)
            { 
                if(_currentMessage.VideoURL != null)
                {
                    _messageContainer.AddMessageV(SenderType.Interlocutor, _currentMessage.VideoURL);
                }
                else if (_currentMessage.AudioClip != null)
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, _currentMessage.AudioClip);
                }
                else if (_currentMessage.Texture2D != null)
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, GetSprite(_currentMessage));
                }
                else
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, _currentMessage.text);
                }
            }
        }

        private void DisplayAnswers(List<Choice> answers)
        {
            _answerOptionController.DisplayAnswers(answers);
        }

        private void SubmitAnswer(Choice answerChoice)
        {
            _messageContainer.AddMessage(SenderType.Player, answerChoice.text);
            story.ChooseChoiceIndex(answerChoice.index);
            story.Continue();
            StartCoroutine(UpdateDialogueView());
        }

        IEnumerator StartPrintingSimulation()
        {
            _messageWritingAnimator.Enable();
            yield return new WaitForSeconds(_responseTimeInSeconds);
            _messageWritingAnimator.Disable();
        }

        private Sprite GetSprite(MessageSolution messageSolution)
        {
            var texture2D = messageSolution.Texture2D;

            if (texture2D != null) return SpriteCreator.CreateSprite(texture2D);
            Debug.LogWarning("Texture not set in Chat Data!!");
            return null;
        }
    }
}