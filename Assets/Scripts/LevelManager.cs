using DA_Assets.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField,
        Tooltip("Article feed, where articles are to be displayed")]
    private ArticleFeed _articleFeed;

    [Header("Levels")]
    [SerializeField] private List<LevelInfo> _levelsInfo = null;

    int _currentLevel = 0;
    public int CurrentLevel { get { return _currentLevel; } }
    List<List<ArticleData>> _levels;

    List<Quest> _quests;

    [SerializeField]
    private Button _todoButton = null;

    [SerializeField]
    private ToDoMenu _todoMenu = null;

    [SerializeField]
    private ChatManager _chatManager = null;

    [SerializeField]
    private GameAssistant _gameAssistant = null;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    [SerializeField]
    private GameObject _loadingAnimation = null;

    [SerializeField]
    private GameObject _receiveExplanationOnLevelRepeat = null;

    private bool _gameLoaded = false;

    [SerializeField]
    private TextMeshProUGUI _newLevelText = null;
    FaderText _faderText = null;
    Fader _faderTextHolder = null;

    [SerializeField]
    private GameObject _endLevelScreen = null;

    private GameObject _articleObject = null;
    private ArticleGameObject _articleData = null;
    private int _currentArticle = 0;

    [SerializeField] private Fader _fader = null;

    public ArticleGameObject CurrentArticle { get { return _articleData; } }

    [SerializeField]
    private TestLogic _testLogic = null;

    [SerializeField]
    private List<LocalizedString> _onLevelFailedMessages = new List<LocalizedString>();

    int _numLevels = 0;

    bool _assistantWaitsForGame = false;

    Slider _gameProgressSlider = null;

    public UnityEvent<int> onLevelStart = null;
    public UnityEvent<int> onLevelEnd = null;
    public UnityEvent<ArticleGameObject> onNewArticleSpawned = null;

    private GameProgressTracker _progressTracker = null;

    Queue<ArticleData> _queueTrueArticles;
    Queue<ArticleData> _queueFalseArticles;

    protected override void Awake()
    {
        _destroyOnLoad = true;
        base.Awake();

        onLevelStart = new UnityEvent<int>();
        onLevelEnd = new UnityEvent<int>();
        onNewArticleSpawned = new UnityEvent<ArticleGameObject>();
    }

    public void SetGameProgressSlider(Slider generalProgress)
    {
        _gameProgressSlider = generalProgress;

        if (_numLevels == 0)
            _numLevels = _levelsInfo.Count;

        _gameProgressSlider.maxValue = _numLevels;
        _gameProgressSlider.value = _currentLevel;

    }

    private void Start()
    {
        if(!_gameAssistant)
        {
            _gameAssistant = FindAnyObjectByType<GameAssistant>();
        }

        _progressTracker = FindAnyObjectByType<GameProgressTracker>();

        if(!_fader) _fader = FindAnyObjectByType<Fader>();

        _faderText = _newLevelText.GetComponent<FaderText>();
        _faderTextHolder = _newLevelText.transform.parent.GetComponent<Fader>();

        _numLevels = _levelsInfo.Count;

        _loadingAnimation?.SetActive(false);
        StartCoroutine(GetArticlesFromResources());

        if (!_testLogic) _testLogic = GameObject.FindFirstObjectByType<TestLogic>();

        // onLevelStart.AddListener(ScoreManager.Instance.ReachedNewLevel);
        onLevelEnd.AddListener(ShowEndLevel);

        CustomEvent newEvent = new CustomEvent("FreeMode_Start");
        AnalyticsManager.Instance.SubmitEvent(newEvent);
    }

    private void SetupBuildLevels()
    {
        _queueTrueArticles = ArticleManager.Instance.GetTrueArticlesByLanguage((int)LanguageSelection.chosenLanguage);
        _queueFalseArticles = ArticleManager.Instance.GetFalseArticlesByLanguage((int)LanguageSelection.chosenLanguage);

        _levels = new List<List<ArticleData>>();
        _quests = new List<Quest>();

        _currentLevel = 0;

        ScoreManager.Instance.FindPointsMenu();
    }

    private bool BuildNextLevel()
    {
        if (_queueFalseArticles.Count == 0 && _queueTrueArticles.Count == 0) return false;

        ScoreManager.Instance.SetNumLevels(_numLevels);

        LevelInfo level = _levelsInfo[_currentLevel];
        int maxArticles = _queueTrueArticles.Count + _queueFalseArticles.Count;

        int articlesInLevel = level.numArticles;
        int articlesToReadInLevel = articlesInLevel;
        int trueArticlesInLevel = 0;
        int articlesCantRead = 0;

        if (_currentLevelCompleted) { _levels.Add(new List<ArticleData>()); _quests.Add(null); }
        else { _levels[_currentLevel].Clear(); _quests[_currentLevel] = null; }

        if (maxArticles < level.numArticles)
        {
            level.numArticles = maxArticles;
        }

        for (int i = 0; i < level.numArticles; ++i)
        {
            ArticleData data = null;

            if (_queueFalseArticles.Count == 0 || (trueArticlesInLevel < level.numTrueArticles && _queueTrueArticles.Count > 0)) {
                data = _queueTrueArticles.Dequeue();
                trueArticlesInLevel++;
            }
            else if (_queueFalseArticles.Count > 0)
            {
                data = _queueFalseArticles.Dequeue();
            }
            else if (_queueFalseArticles.Count == 0 && _queueTrueArticles.Count == 0)
            {
                Quest quest_1 = new Quest();
                quest_1.BuildQuest(trueArticlesInLevel, articlesInLevel, level.articleIsSharedWithGroups ? level.numGroupsToShareWith : 0, articlesInLevel - articlesCantRead, level.groupHavePreferredTheme);
                _quests[_currentLevel] = quest_1;
                return true;
            }

            data.canBeSharedWithGroups = level.articleIsSharedWithGroups;
            articlesCantRead = data.articleBody == string.Empty ? articlesCantRead + 1 : articlesCantRead;
            if (data.articleBody == string.Empty) articlesToReadInLevel--;
            _levels[_currentLevel].Add(data);
        }

        _levels[_currentLevel].Shuffle();

        int numGroups = level.articleIsSharedWithGroups ? level.numGroupsToShareWith : 0;

        // quest generation
        Quest quest = new Quest();
        quest.BuildQuest(trueArticlesInLevel, articlesInLevel, numGroups, articlesInLevel - articlesCantRead, level.groupHavePreferredTheme);
        _quests[_currentLevel] = quest;

        return true;
    }

    private IEnumerator GetArticlesFromResources()
    {
        _gameLoaded = false;
        _loadingAnimation?.SetActive(true);
        List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WELCOME_", 8, 0);
        // _gameAssistant.ShowMessages(messages.ToArray(), WorryAssistantTutorial);
        yield return new WaitUntil(() => ArticleManager.Instance.ArticlesCreated);
        SetupBuildLevels();
        
        _gameLoaded = true;
        _loadingAnimation?.SetActive(false);
        _fader.StartFade(1.5f, 0.8f, 0.0f);
        
        StartLevel();
    }

    private void StartLevel() 
    {
        if (_discardedArticlesDuringLevel != null) _discardedArticlesDuringLevel.Clear();
        _discardedArticlesDuringLevel = new List<ArticleData>();
        
        if(!BuildNextLevel())
        {
            _progressTracker.UpdateValue();
            EndAllLevels();

            return;
        }

        _fader.StartFade(1.0f, 0.8f, 0.0f);
        _levelStarted = true;
        // Aquí deberíamos meter cosas de "Nivel 1!" y eso
        if (_faderTextHolder.Value <= 0.1f) _faderTextHolder.StartFade(1.0f, 0.0f, 1.0f);

        _newLevelText.gameObject.SetActive(true);
        _newLevelText.SetText(string.Format(TranslationManager.Instance.GetLocalizedStringValue("Translation", "CURRENT_LEVEL"), _currentLevel + 1));

        _faderText.StartFade(1.5f, 0.0f, 1.0f);
        _faderText.OnFadeEnd.AddListener(StartLevelAux);
    }

    private void StartLevelAux()
    {
        StartCoroutine(_StartLevelAux());
    }

    IEnumerator _StartLevelAux()
    {
        _faderText.OnFadeEnd.RemoveAllListeners();
        yield return new WaitForSeconds(1.3f);
        _faderText.StartFade(1.5f, 1.0f, 0.0f);
        yield return new WaitForSeconds(0.65f);
        ShowNextArticle();
        _faderTextHolder.StartFade(1.0f, 1.0f, 0.0f);
    }

    public void ShowTest(Test test)
    {
        _testLogic.SetTest(test, true);
        ResetLevelStats();
    }

    private void ResetLevelStats()
    {
        if (_currentLevelCompleted) _currentLevel++;
        _currentArticle = 0;
        _endLevelScreen.SetActive(false);
    }

    public void ShowEndLevel(int level)
    {
        _todoButton.gameObject.SetActive(false);

        _fader.StartFade(0.8f, 0.0f, 0.8f);

        Test test = _levelsInfo[level].test;
        if (test)
        {
            string message = TranslationManager.Instance.GetLocalizedStringValue("Translation", "END_LEVEL/AVATAR_MESSAGE");
            _gameAssistant.ShowMessageOneShot(message, () => ShowTest(test));
        }
        else
        {
            List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("Translation", "END_LEVEL_MESSAGE/", 2, 1);
            _gameAssistant.ShowMessages(messages.ToArray(), () => ScoreManager.Instance.ShowPoints(_quests[level]));
            ResetLevelStats();
        }
    }

    private void PostTotalScoreToDatabase()
    {
        int finalScore = ScoreManager.Instance.Score;

        StartCoroutine(ServerManager.Instance.PostScoreToDatabase(finalScore, TranslationManager.Instance.GetCurrentCountryLabel()));
    }

    bool _levelStarted = false;
    List<ArticleData> _discardedArticlesDuringLevel = null;
    bool _currentLevelCompleted = true;

    public void ShowMessagesEndLevel()
    {
        if (_currentLevelCompleted)
            ShowNextArticle();
        else
            ShowMessagesFailedLevel();
    }

    private bool ShowWorriedMessageBetweenArticles()
    {
        bool hasMessage = _gameAssistant.State == GameAssistantState.BAD;
        if (hasMessage)
        {
            _gameAssistant.ShowEnqueuedMessages(ShowNextArticle);
        }

        return hasMessage;
    }

    private void ShowImmediateFeedback(string[] feedback)
    {
        _gameAssistant.ShowMessages(feedback, ShowNextArticle);
    }

    public void ShowNextArticle()
    {
        if(_currentLevel >= _levelsInfo.Count)
        {
            _progressTracker.UpdateValue();
            EndAllLevels();
            return;
        }

        if (_articleObject != null)
        {
            bool result = _quests[_currentLevel].EvaluateArticle(_articleData);
            if (!result)
            {
                _discardedArticlesDuringLevel.Add(_articleData.Data);
            }

            string[] feedback = _articleData.Data.feedback;

            _articleData.OnSkip.RemoveAllListeners();
            _articleData.DestroyArticle();

            if (feedback == null && ShowWorriedMessageBetweenArticles()) return;
            else if (!result && (feedback != null && feedback.Length > 0)) { ShowImmediateFeedback(feedback); return; }
        }

        if (!_levelStarted)
        {
            StartLevel();
        }
        else if (_currentArticle >= _levels[_currentLevel].Count)
        {
            _levelStarted = false;

            // Debug.LogError("Te quedaste aquí!!!!!!!!! -> Tienes que hacer que se sumen los puntos de la quest");
            _currentLevelCompleted = ScoreManager.Instance.EvaluateQuest(_quests[_currentLevel]);

            if(!_currentLevelCompleted)
            {
                foreach (ArticleData data in _discardedArticlesDuringLevel)
                {
                    if (data.isTrue)
                    {
                        _queueTrueArticles.Enqueue(data);
                    }
                    else
                    {
                        _queueFalseArticles.Enqueue(data);
                    }
                }
            }
            
            onLevelEnd.Invoke(_currentLevel);
        }
        else
        {
            _progressTracker.UpdateValue();
            _articleObject = Instantiate(_articlePrefab, _articleFeed.transform);
            _articleData = _articleObject.GetComponent<ArticleGameObject>();
            ArticleData data = Instantiate(_levels[_currentLevel][_currentArticle]);

#if UNITY_EDITOR
            if (data.isTrue)
            {
                data.articleTitle = "(" + TopicsDictionary.topics[data.theme].ToString() + ") IS TRUE: " + data.articleTitle;
            }
#endif
            if (!data.canBeSharedWithGroups) _articleData.OnShare.AddListener(ShowNextArticle);
            else
            {
                data.numGroupsToShareWith = _levelsInfo[_currentLevel].numGroupsToShareWith;
                data.sharedWithGroups = new bool[data.numGroupsToShareWith];
            }

            _articleData.SetArticleData(data);

            if (_currentArticle++ == 0) {
                // tell ScoreManager that a new Level was reached 
                // _fader.StartFade(0.8f, 0.8f, 0.0f);
                onLevelStart.Invoke(_currentLevel);
                if (!_currentLevelCompleted)
                {
                    PromptChoiceToRepatLevelExplanation();
                }
                else
                {
                    ShowMessagesLevelStart(_levelsInfo[_currentLevel].avatarMessagesOnStart);
                }

                HashSet<Topics> topics = new HashSet<Topics>();
                foreach (ArticleData article in _levels[_currentLevel])
                {
                    if (article.isTrue) topics.Add(TopicsDictionary.topics[article.theme]);
                }

                // añadir mensaje de "ahora a tus grupos les interesa: blabla"
                if (_levelsInfo[_currentLevel].groupHavePreferredTheme)
                {
                    _chatManager.RandomizeAllGroupTopics(topics.ToList(), _levelsInfo[_currentLevel].numGroupsToShareWith);
                }

                _todoMenu.SetValues(_quests[_currentLevel]);
            }

            onNewArticleSpawned.Invoke(_articleData);
        }
    }

    private void ShowMessagesLevelStart(List<LocalizedString> messagesList)
    {
        if (messagesList.Count == 0)
        {
            StartLevelPostMessages();
            return;
        }
        if (messagesList != null && messagesList.Count > 0)
        {
            string[] messages = new string[messagesList.Count];
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = messagesList[i].GetLocalizedString();
            }

            _fader.StartFade(1.0f, 0.0f, 0.8f, true);
            _gameAssistant.ShowMessages(messages, StartLevelPostMessages);
        }
    }

    private void ShowMessagesFailedLevel()
    {
        if (_onLevelFailedMessages != null && _onLevelFailedMessages.Count > 0)
        {
            string[] messages = new string[_onLevelFailedMessages.Count];
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = _onLevelFailedMessages[i].GetLocalizedString();
            }

            _fader.StartFade(1.0f, 0.0f, 0.8f, true);
            if (_levelsInfo[_currentLevel].avatarMessagesOnStart.Count > 0)
            {
                int auxLevel = _currentLevel;
                _gameAssistant.ShowMessages(messages, ShowNextArticle);
            }
            else
                _gameAssistant.ShowMessages(messages, StartLevelPostMessages);
        }
    }

    private void PromptChoiceToRepatLevelExplanation()
    {
        Button[] buttons = _receiveExplanationOnLevelRepeat.GetComponentsInChildren<Button>();

        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();

        buttons[0].onClick.AddListener(() => 
        { ShowMessagesLevelStart(_levelsInfo[_currentLevel].avatarMessagesOnStart); _receiveExplanationOnLevelRepeat.SetActive(false); });

        buttons[1].onClick.AddListener(() => 
        { StartLevelPostMessages(); _receiveExplanationOnLevelRepeat.SetActive(false); });

        _receiveExplanationOnLevelRepeat.SetActive(true);
    }

    private void StartLevelPostMessages()
    {
        _todoButton.onClick.Invoke();
        _gameAssistant.HideMessage();
        _fader.StartFade(1.0f, 0.8f, 0.0f);
    }

    private void EndAllLevels()
    {
        PostTotalScoreToDatabase();

        SceneChanger.Instance.ChangeScene("EndLevels");

        AnalyticsManager.Instance.SubmitEvent("FreeMode_End");
    }

    public Topics GetGroupTheme(int id)
    {
        return _chatManager.GetGroupTheme(id);
    }
}
