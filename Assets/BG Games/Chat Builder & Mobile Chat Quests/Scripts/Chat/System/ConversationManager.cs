using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;
//using static System.Net.WebRequestMethods;

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

        private MessageSolution _currentMessage;

        private void OnDisable() => _answerOptionController.SelectedAnswer -= SubmitAnswer;

        private void Start()
        {
            inkJSONAsset = new TextAsset(ServerManager.Instance.inkText);
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
                string key = "";
                string value = "";

                if (splitTag.Length > 1)
                {
                    key = splitTag[0].Trim();
                    value = splitTag[1].Trim();
                }

                if(key != null && value != null)
                {
                    if (key == "image")
                    {
                        StartCoroutine(DownloadImage("https://drive.google.com/uc?export=download&id=" + value));
                    }
                    else if (key == "video")
                    {
                        Debug.Log(value);
                        _currentMessage.VideoURL = "https://drive.google.com/uc?export=download&id=" + value;
                        DisplayNextMessage();
                    }
                    else if (key == "audio")
                    {
                        _currentMessage.AudioClip = Resources.Load(value) as AudioClip;
                        DisplayNextMessage();
                    }
                    else
                        _currentMessage = null;
                }
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
            StartCoroutine(UpdateDialogueView());
        }

        IEnumerator StartPrintingSimulation()
        {
            _messageWritingAnimator.Enable();
            yield return new WaitForSeconds(_responseTimeInSeconds);
            _messageWritingAnimator.Disable();
        }

        IEnumerator DownloadImage(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                _currentMessage.Texture2D = ((DownloadHandlerTexture)request.downloadHandler).texture;
                DisplayNextMessage();
            }
            else Debug.LogError("No se ha podido cargar la textura: " + request.downloadHandler.error );
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