using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using DA_Assets.Extensions;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class ConversationManager : MonoBehaviour
    {
        [Header("Entry point class")] [Space] [SerializeField]
        private AnswerOptionController _answerOptionController;

        [SerializeField] private MessageWritingAnimator _messageWritingAnimator;
        [SerializeField] private MessageContainer _messageContainer;
        [SerializeField] private GameObject _articlePrefab;
        [SerializeField] private GameObject _shareButtonsPrefab;

        [SerializeField] private Transform _articleParent;
        [SerializeField] private Transform _shareButtonsParent;

        [SerializeField] private ScrollRect _scrollRect;

        [SerializeField] private GameObject _groupChats;
        [SerializeField] private GameObject _mainFeedChat;

        // ink
        [SerializeField] public TextAsset inkJSONAsset;
        public Story story;

        public UnityEvent OnGameEnd = new UnityEvent();

        [Space] [Range(0.2f, 10f)] [SerializeField]
        private float _responseTimeInSeconds;

        private MessageSolution _currentMessage;

        private ArticleDataSetter _currentArticle;

        private GameObject _currentChat;

        // Llega este texto por Ink cuando se va a recibir un art�culo
        private Dictionary<Language, string> ARTICLE_RECEIVED_FLAG;
        private Dictionary<Language, string> ARTICLE_HEADLINE_TEXT;
        private Dictionary<Language, string> SENT_TO_TEXT;
        private Dictionary<Language, string> FAMILY_KEYWORD;
        private Dictionary<Language, string> FRIENDS_KEYWORD;
        private Dictionary<Language, string> NEIGHBOURS_KEYWORD;
        private Dictionary<Language, string> SOURCE_KEYWORD;

        private void OnDisable() => _answerOptionController.SelectedAnswer -= SubmitAnswer;

        private void Start()
        {
            inkJSONAsset = new TextAsset(ServerManager.Instance.inkText);

            SetUpLanguageKeyWord();

            InitializeChats();

            StartConversation();
        }

        private void SetUpLanguageKeyWord()
        {
            ARTICLE_RECEIVED_FLAG = new Dictionary<Language, string>();
            ARTICLE_RECEIVED_FLAG.Add(Language.spanish, "ARTÍCULO RECIBIDO");
            ARTICLE_RECEIVED_FLAG.Add(Language.english, "ARTICLE RECEIVED");

            ARTICLE_HEADLINE_TEXT = new Dictionary<Language, string>();
            ARTICLE_HEADLINE_TEXT.Add(Language.english, "Article headline: ");
            ARTICLE_HEADLINE_TEXT.Add(Language.spanish, "Titular: ");

            SENT_TO_TEXT = new Dictionary<Language, string>();
            SENT_TO_TEXT.Add(Language.english, "Sent to ");
            SENT_TO_TEXT.Add(Language.spanish, "Enviado a ");

            FAMILY_KEYWORD = new Dictionary<Language, string> ();
            FAMILY_KEYWORD.Add(Language.english, "family");
            FAMILY_KEYWORD.Add(Language.spanish, "familia");

            FRIENDS_KEYWORD = new Dictionary<Language, string>();
            FRIENDS_KEYWORD.Add(Language.english, "friends");
            FRIENDS_KEYWORD.Add(Language.spanish, "amigos");

            NEIGHBOURS_KEYWORD = new Dictionary<Language, string>();
            NEIGHBOURS_KEYWORD.Add(Language.english, "neighbours");
            NEIGHBOURS_KEYWORD.Add(Language.spanish, "vecinos");

            SOURCE_KEYWORD = new Dictionary<Language, string>();
            SOURCE_KEYWORD.Add(Language.english, "This article comes from: ");
            SOURCE_KEYWORD.Add(Language.spanish, "Fuente: ");
        }

        private void InitializeChats()
        {
            for (int i = 0; i < _groupChats.transform.childCount; ++i)
            {
                _groupChats.transform.GetChild(i).gameObject.SetActive(false);
            }

            _currentChat = _mainFeedChat;
            _currentChat.SetActive(true);
            _scrollRect.content = _currentChat.transform as RectTransform;
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
                            // escogemos saltarnos el art�culo
                            _answerOptionController.OnAnswerClicked(story.currentChoices[3]);
                        }
                        break;
                    case ArticleAction.Share:
                        // handle group reaction: cambiar el chat y eso
                        if(story.currentChoices.Count > 0)
                        {
                            _currentArticle.ShareButton(story.currentChoices);
                        }
                        else
                        {
                            return true;
                        }
                        break;
                    case ArticleAction.None:
                        _currentArticle = null;
                        break;
                }
            }

            // devuelve true si ya no queremos hacer nada con el art�culo
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
                    // si ya no queremos el art�culo que tenemos a mano, hacemos el flujo normal de la historia
                    if (DoArticle())
                    {
                        string text = GetNextStoryText();

                        if (text == ARTICLE_RECEIVED_FLAG[LanguageSelection.chosenLanguage])
                        {
                            HandleArticleReceived();
                            article = true;
                        }
                        else if (!text.Contains(SENT_TO_TEXT[LanguageSelection.chosenLanguage]))
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

                _alreadyUpdatingDialogues = false;
                // Display all the choices, if there are any!
                if (!article && story.currentChoices.Count > 0)
                {
                    if(_currentArticle && _currentArticle.Action == ArticleAction.Share)
                    {
                        _currentArticle.ShareButton(story.currentChoices);
                    }
                    else DisplayAnswers(story.currentChoices);
                }
                // If we've read all the content and there's no choices, the story is finished!
                else if(story.currentChoices.Count == 0)
                {
                    yield return new WaitForSeconds(3.0f);
                    OnGameEnd.Invoke();
                }
            }
        }

        public GameObject SpawnShareButtons()
        {
            GameObject gO = Instantiate(_shareButtonsPrefab, _currentChat.transform);
            foreach(RectTransform rect in gO.GetComponentsInChildren<RectTransform>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
            return gO;
        }

        private void HandleArticleReceived()
        {
            ArticleData articleData = new ArticleData();
            articleData.articleTitle = GetNextStoryText().Replace(ARTICLE_HEADLINE_TEXT[LanguageSelection.chosenLanguage], string.Empty);

            _currentArticle = Instantiate(_articlePrefab, _articleParent).GetComponent<ArticleDataSetter>();
            // Aqu� se presenta la decisi�n de si se quiere leer o no el art�culo
            if(story.currentChoices.Count > 0)
            {
                _answerOptionController.ArticleReadOptions(story.currentChoices, _currentArticle);
            }
            //articleData.companyName = GetNextStoryText().Replace("This article comes from ", string.Empty);
            //articleData.articleBody = GetNextStoryText();

            _currentArticle.SetArticleData(articleData);
        }

        public void ChangeGroup(Choice groupToSend)
        {
            if(groupToSend == null)
            {
                _currentChat.SetActive(false);

                _currentChat = _mainFeedChat;
                _currentChat.SetActive(true);
                _scrollRect.content = _currentChat.transform as RectTransform;

                return;
            }

            string[] words = groupToSend.text.Split(' ');
            string whichGroup = words[words.Length - 1].Trim('.');
            _currentChat.SetActive(false);

            GameObject group = null;
            if (whichGroup == FAMILY_KEYWORD[LanguageSelection.chosenLanguage]) group = _groupChats.transform.GetChild(0).gameObject;
            else if (whichGroup == FRIENDS_KEYWORD[LanguageSelection.chosenLanguage]) group = _groupChats.transform.GetChild(1).gameObject;
            else if (whichGroup == NEIGHBOURS_KEYWORD[LanguageSelection.chosenLanguage]) group = _groupChats.transform.GetChild(2).gameObject;

            group.SetActive(true);
            _currentChat = group;
            _scrollRect.content = _currentChat.transform as RectTransform;
            // hacer los grupos como tal (? o probarlo, creo que ya est� todo hecho
        }

        public void SendArticle(ArticleData articleData)
        {
            _messageContainer.SendArticle(articleData, _currentChat.transform);
        }

        private void ReadArticle()
        {
            if(story.canContinue) {
                _currentArticle.Data.companyName = GetNextStoryText().Replace(SOURCE_KEYWORD[LanguageSelection.chosenLanguage], string.Empty);
                _currentArticle.Data.articleBody = GetNextStoryText();
                _currentArticle.SetArticleData(_currentArticle.Data);
                _currentArticle.ChangeButtonsOnArticleRead();
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
                string[] message = _currentMessage.text.Split(':');
                string name;
                string text;
                if(message.Length > 1)
                {
                    name = _currentMessage.text.Split(':')[0];
                    text = _currentMessage.text.Substring(name.Length + 2);
                }
                else
                {
                    name = _currentChat.GetComponent<GroupSettings>().GetRandomName();
                    text = _currentMessage.text;
                }
                if (_currentMessage.VideoURL != null)
                {
                    _messageContainer.AddMessageV(SenderType.Interlocutor, name, _currentMessage.VideoURL, _currentChat.transform);
                }
                else if (_currentMessage.AudioClip != null)
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, name, _currentMessage.AudioClip, _currentChat.transform);
                }
                else if (_currentMessage.Texture2D != null)
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, name, GetSprite(_currentMessage), _currentChat.transform);
                }
                else
                {
                    _messageContainer.AddMessage(SenderType.Interlocutor, name, text, _currentChat.transform);
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
                _messageContainer.AddMessage(SenderType.Player, "", answerChoice.text, _currentChat.transform);
            story.ChooseChoiceIndex(answerChoice.index);
            StartCoroutine(UpdateDialogueView());
        }

        IEnumerator StartPrintingSimulation()
        {
            _messageWritingAnimator = _currentChat.GetComponent<GroupSettings>().GetWritingAnimator();
            _messageWritingAnimator.Enable();
            _scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x, 0.0f);
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