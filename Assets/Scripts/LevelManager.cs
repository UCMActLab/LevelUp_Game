using DA_Assets.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//[Serializable]
//public struct LevelInfo
//{
//    public Test test;
//    public int numArticles;
//    public int numTrueArticles;
    
//    public bool articleIsSharedWithGroups;

//    [Range(1, 3)]
//    public bool numGroupsToShareWith;
//}

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
    private GameAssistant _gameAssistant = null;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    [SerializeField]
    private GameObject _loadingAnimation = null;

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

    int _numLevels = 0;

    bool _assistantWaitsForGame = false;

    Slider _gameProgressSlider = null;

    public UnityEvent<int> onLevelStart = null;
    public UnityEvent<int> onLevelEnd = null;
    public UnityEvent<ArticleGameObject> onNewArticleSpawned = null;

    private GameProgressTracker _progressTracker = null;

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

    private void BuildLevels()
    {
        Queue<ArticleData> queueTrueArticles = ArticleManager.Instance.GetTrueArticlesByLanguage((int)LanguageSelection.chosenLanguage);
        Queue<ArticleData> queueFalseArticles = ArticleManager.Instance.GetFalseArticlesByLanguage((int)LanguageSelection.chosenLanguage);

        _levels = new List<List<ArticleData>>();
        _quests = new List<Quest>();

        int currentLevel = 0;
        int articlesTotal = 0;
        int articlesCanRead = 0;
        int trueArticles = 0;

        ScoreManager.Instance.SetNumLevels(_numLevels);

        int maxScore = 0;

        for (int o = 0; o < _levelsInfo.Count; ++o)
        {
            LevelInfo level = _levelsInfo[o];
            int maxArticles = queueTrueArticles.Count + queueFalseArticles.Count;

            int articlesInLevel = level.numArticles;
            int articlesToReadInLevel = articlesInLevel;
            int trueArticlesInLevel = 0;
            int articlesCantRead = 0;

            _levels.Add(new List<ArticleData>());

            if (maxArticles < level.numArticles)
            {
                level.numArticles = maxArticles;
            }

            for (int i = 0; i < level.numArticles; ++i)
            {
                int choose = UnityEngine.Random.Range(0, 2);
                ArticleData data = null;

                if (queueFalseArticles.Count == 0 || (trueArticlesInLevel < level.numTrueArticles && queueTrueArticles.Count > 0)) {
                    data = queueTrueArticles.Dequeue();
                    trueArticlesInLevel++;
                }
                else
                {
                    data = queueFalseArticles.Dequeue();
                }

                data.canBeSharedWithGroups = level.articleIsSharedWithGroups;
                if (data.articleBody == string.Empty) articlesToReadInLevel--;
                _levels[currentLevel].Add(data);
            }

            _levels[currentLevel].Shuffle();

            trueArticles += trueArticlesInLevel;

            articlesCanRead += articlesInLevel - articlesCantRead;
            articlesTotal += articlesInLevel;

            int totalQuestions = level.test ? level.test.TotalQuestions : -1;

            int numGroups = level.articleIsSharedWithGroups ? level.numGroupsToShareWith : 0;

            // quest generation
            Quest quest = new Quest();
            quest.BuildQuest(trueArticlesInLevel, articlesInLevel, numGroups, articlesInLevel - articlesCantRead);
            _quests.Add(quest);

            maxScore += quest.GetMaxPossibleScore();

            currentLevel++;

            // ScoreManager.Instance.SetLevelInfo(currentLevel++, articlesInLevel, articlesToReadInLevel, trueArticlesInLevel, totalQuestions);
        }

        ScoreManager.Instance.SetMaxScore(maxScore);
    }

    private IEnumerator GetArticlesFromResources()
    {
        _gameLoaded = false;
        _loadingAnimation?.SetActive(true);
        List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WELCOME_", 8, 0);
        _gameAssistant.ShowMessagesOneShot(messages.ToArray(), WorryAssistantTutorial);
        yield return new WaitUntil(() => ArticleManager.Instance.ArticlesCreated);
        BuildLevels();
        
        _gameLoaded = true;
        _loadingAnimation?.SetActive(false);
        _fader.StartFade(1.5f, 0.8f, 0.0f);
        
        if (_assistantWaitsForGame)
        {
            _gameAssistant.HideMessage();
            StartLevel();
        }
    }

    private void WorryAssistantTutorial()
    {
        _gameAssistant.WorryAssistant(TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED_TUTORIAL_", 4, 0));

        _gameAssistant.onStateChanged.AddListener(LastWelcomeMessages);
    }

    private void LastWelcomeMessages(GameAssistantState state)
    {
        if (state != GameAssistantState.NORMAL) return;

        List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WELCOME_", 3, 8);
        _gameAssistant.ShowMessagesOneShot(messages.ToArray(), AvatarWelcomesToGame);

        _gameAssistant.onStateChanged.RemoveListener(LastWelcomeMessages);
    }

    private void AvatarWelcomesToGame()
    {
        if (_gameLoaded)
        {
            _assistantWaitsForGame = false;
            _gameAssistant.HideMessage();

            StartLevel();
        }
        else
        {
            string messages = "El juego comenzará cuando esté cargado";
            _gameAssistant.ShowMessage(messages);
            _assistantWaitsForGame = true;
        }
    }

    private void StartLevel() 
    {
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

    public void SkipArticleIfCantShare()
    {
        if(_articleData.HasSharedWithAllGroups)
        {
            ShowNextArticle();
        }
    }

    public void ShowTest(Test test)
    {
        _testLogic.SetTest(test, true);
        ResetLevelStats();
    }

    private void ResetLevelStats()
    {
        _currentLevel++;
        _currentArticle = 0;
        _endLevelScreen.SetActive(false);
    }

    public void ShowEndLevel(int level)
    {
        // _endLevelScreen.SetActive(true);
        _fader.StartFade(0.8f, 0.0f, 0.8f);

        // ScoreManager.Instance.CalculateArticlePoints()

        Test test = _levelsInfo[level].test;
        if (test)
        {
            string message = TranslationManager.Instance.GetLocalizedStringValue("Translation", "END_LEVEL/AVATAR_MESSAGE");
            _gameAssistant.ShowMessageOneShot(message, () => ShowTest(test));
        }
        else
        {
            Debug.LogError("TODO: Change Assistant Message");
            _gameAssistant.ShowMessageOneShot("ola!", () => ScoreManager.Instance.ShowPoints(_quests[level]));
            ResetLevelStats();
        }
    }

    private void PostTotalScoreToDatabase()
    {
        int finalScore = ScoreManager.Instance.Score;

        StartCoroutine(ServerManager.Instance.PostScoreToDatabase(finalScore, TranslationManager.Instance.GetCurrentCountryLabel()));
    }

    bool _levelStarted = false;
    public void ShowNextArticle()
    {
        if(_currentLevel >= _levels.Count)
        {
            _progressTracker.UpdateValue();
            EndAllLevels();
            return;
        }

        if (_articleObject != null)
        {
            _quests[_currentLevel].EvaluateArticle(_articleData);

            // ScoreManager.Instance.CalculateArticlePoints(_articleData);
            _articleData.OnSkip.RemoveAllListeners();
            _articleData.DestroyArticle();
        }

        if (_currentArticle >= _levels[_currentLevel].Count)
        {
            _levelStarted = false;

            Debug.LogError("Te quedaste aquí!!!!!!!!! -> Tienes que hacer que se sumen los puntos de la quest");
            ScoreManager.Instance.EvaluateQuest(_quests[_currentLevel]);
            onLevelEnd.Invoke(_currentLevel);
        }
        else if (!_levelStarted)
        {
            StartLevel();
        }
        else
        {
            _progressTracker.UpdateValue();
            _articleObject = Instantiate(_articlePrefab, _articleFeed.transform);
            _articleData = _articleObject.GetComponent<ArticleGameObject>();
            _articleData.OnSkip.AddListener(ShowNextArticle);
            ArticleData data = Instantiate(_levels[_currentLevel][_currentArticle]);

#if UNITY_EDITOR
            if (data.isTrue)
            {
                data.articleTitle = "IS TRUE: " + data.articleTitle;
            }
#endif
            _articleData.SetArticleData(data);

            if (!data.canBeSharedWithGroups) 
            { 
                _articleData.SetupShareButtonForVerification();
                _articleData.OnShare.AddListener(ShowNextArticle);
            }
            else _articleData.SetupShareButtonForGroups(1);

            if (_currentArticle++ == 0) {
                // tell ScoreManager that a new Level was reached 
                // _fader.StartFade(0.8f, 0.8f, 0.0f);
                onLevelStart.Invoke(_currentLevel);
            }

            onNewArticleSpawned.Invoke(_articleData);
        }
    }

    private void EndAllLevels()
    {
        PostTotalScoreToDatabase();

        SceneChanger.Instance.ChangeScene("EndGame");

        AnalyticsManager.Instance.SubmitEvent("FreeMode_End");
    }
}
