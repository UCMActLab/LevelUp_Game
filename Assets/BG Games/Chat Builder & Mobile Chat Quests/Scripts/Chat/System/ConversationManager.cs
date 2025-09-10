using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using TMPro.EditorUtilities;
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
        [SerializeField] private GameObject _articlePrefab;

        [SerializeField] private Transform _articleParent;

        // ink
        [SerializeField] public TextAsset inkJSONAsset;
        public Story story;

        [Space] [Range(0.2f, 10f)] [SerializeField]
        private float _responseTimeInSeconds;

        private MessageSolution _currentMessage;

        private ArticleDataSetter _currentArticle;

        // Llega este texto por Ink cuando se va a recibir un artículo
        private const string ARTICLE_RECEIVED_FLAG = "ARTICLE RECEIVED";

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

        public string GetNextStoryText()
        {
            if (story.canContinue)
                // Continue gets the next line of the story
                // This removes any white space from the text.
                return story.Continue().Trim();
            else return null;
        }

        private bool DoArticle()
        {
            if(_currentArticle != null)
            {
                switch (_currentArticle.Action)
                {
                    case ArticleAction.Read:
                        ReadArticle();
                        break;
                    case ArticleAction.Skip:
                        _currentArticle.Action = ArticleAction.None;
                        string text = GetNextStoryText();
                        if(story.currentChoices.Count > 0)
                        {
                            // escogemos saltarnos el artículo
                            _answerOptionController.OnAnswerClicked(story.currentChoices[3]);
                        }
                        break;
                    case ArticleAction.Share:
                        ShareArticle();
                        break;
                    case ArticleAction.None:
                        _currentArticle = null;
                        break;
                }
            }

            // devuelve true si ya no queremos hacer nada con el artículo
            return _currentArticle == null;
        }

        bool _alreadyUpdatingDialogues = false;
        IEnumerator UpdateDialogueView()
        {
            if (!_alreadyUpdatingDialogues)
            {
                _alreadyUpdatingDialogues = true;
                bool article = false;
                while (story.canContinue)
                {
                    // si ya no queremos el artículo que tenemos a mano, hacemos el flujo normal de la historia
                    if (DoArticle())
                    {
                        string text = GetNextStoryText();

                        if (text == ARTICLE_RECEIVED_FLAG)
                        {
                            HandleArticleReceived();
                            article = true;
                        }

                        else
                        {
                            // Display the text on screen!
                            _currentMessage = new MessageSolution();
                            _currentMessage.text = text;

                            yield return StartCoroutine(StartPrintingSimulation());

                            DisplayNextMessage();

                            handleTags();

                            article = false;
                        }
                    }
                    else { article = true; }
                }

                // Display all the choices, if there are any!
                if (!article && story.currentChoices.Count > 0)
                {
                    DisplayAnswers(story.currentChoices);
                }
                // If we've read all the content and there's no choices, the story is finished!
                else
                {
                    Debug.Log("FIN");
                }
                _alreadyUpdatingDialogues = false;
            }
        }

        private void HandleArticleReceived()
        {
            ArticleData articleData = new ArticleData();
            articleData.articleTitle = GetNextStoryText().Replace("Article headline: ", string.Empty);

            _currentArticle = Instantiate(_articlePrefab, _articleParent).GetComponent<ArticleDataSetter>();
            // Aquí se presenta la decisión de si se quiere leer o no el artículo
            if(story.currentChoices.Count > 0)
            {
                _answerOptionController.ArticleReadOptions(story.currentChoices, _currentArticle);
            }
            //articleData.companyName = GetNextStoryText().Replace("This article comes from ", string.Empty);
            //articleData.articleBody = GetNextStoryText();

            _currentArticle.SetArticleData(articleData);
        }

        private void ReadArticle()
        {
            if(story.canContinue) {
                _currentArticle.Data.companyName = GetNextStoryText().Replace("This article comes from ", string.Empty);
                _currentArticle.Data.articleBody = GetNextStoryText();
                _currentArticle.SetArticleData(_currentArticle.Data);
                _currentArticle.ChangeButtonsOnArticleRead();

            }
        }

        private void ShareArticle()
        {

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
            if(_currentArticle == null) 
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