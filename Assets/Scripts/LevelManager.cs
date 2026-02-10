using DA_Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct LevelInfo
{
    public Test test;
    public int numArticles;
}

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField,
        Tooltip("Article feed, where articles are to be displayed")]
    private ArticleFeed _articleFeed;

    [Header("Levels")]
    [SerializeField] private List<LevelInfo> _levelsInfo = null;
    //[SerializeField] private int _articlesPerLevel = 3;
    //[SerializeField] private int _maxLevels = 5;

    int _currentLevel = 0;
    public int CurrentLevel { get { return _currentLevel; } }
    List<List<ArticleData>> _levels;

    [SerializeField]
    private GameAssistant _gameAssistant = null;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    [SerializeField]
    private GameObject _loadingAnimation = null;

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

        _numLevels = _levelsInfo.Count;

        _loadingAnimation?.SetActive(false);
        StartCoroutine(GetArticlesFromResources());

        if (!_testLogic) _testLogic = GameObject.FindFirstObjectByType<TestLogic>();

        onLevelStart.AddListener(ScoreManager.Instance.ReachedNewLevel);
        onLevelEnd.AddListener(ShowEndLevel);

        CustomEvent newEvent = new CustomEvent("FreeMode_Start");
        AnalyticsManager.Instance.SubmitEvent(newEvent);
    }

    private void BuildLevels()
    {
        List<ArticleData> data = ArticleManager.Instance.GetAllArticlesByLanguage((int)LanguageSelection.chosenLanguage);

        data.Shuffle();
        
        _levels = new List<List<ArticleData>>();
        int currentLevel = 0;
        int articlesTotal = 0;
        int articlesCanRead = 0;
        int trueArticles = 0;

        ScoreManager.Instance.SetNumLevels(_numLevels);

        foreach (LevelInfo level in _levelsInfo)
        {
            int articlesInLevel = level.numArticles;
            int articlesToReadInLevel = articlesInLevel;
            int trueArticlesInLevel = 0;
            int articlesCantRead = 0;

            _levels.Add(new List<ArticleData>());
            for (int i = 0; i < level.numArticles; ++i)
            {
                _levels[currentLevel].Add(data[0]);
                if (data[0].isTrue) trueArticlesInLevel++;
                if (data[0].articleBody == string.Empty) articlesToReadInLevel--;
                data.RemoveAt(0);
            }

            trueArticles += trueArticlesInLevel;

            articlesCanRead += articlesInLevel - articlesCantRead;
            articlesTotal += articlesInLevel;
            
            ScoreManager.Instance.SetLevelInfo(currentLevel++, articlesInLevel, articlesToReadInLevel, trueArticlesInLevel, level.test.TotalQuestions);
        }

        ScoreManager.Instance.SetMaxScore();
    }

    private IEnumerator GetArticlesFromResources()
    {
        _loadingAnimation?.SetActive(true);
        yield return new WaitUntil(() => ArticleManager.Instance.ArticlesCreated);
        _loadingAnimation?.SetActive(false);

        BuildLevels();

        if (_startOnAwake)
        {
            ShowNextArticle();
        }
    }

    public void SkipArticleIfCantShare()
    {
        if(_articleData.HasSharedWithAllGroups)
        {
            ShowNextArticle();
        }
    }

    public void ShowTest()
    {
        _testLogic.SetTest(_levelsInfo[_currentLevel].test, true);
        _currentLevel++;
        _currentArticle = 0;
        _endLevelScreen.SetActive(false);
    }

    public void ShowEndLevel(int level)
    {
        // _endLevelScreen.SetActive(true);
        _fader.StartFade(0.8f, 0.0f, 0.8f);
        string message = TranslationManager.Instance.GetLocalizedStringValue("Translation", "END_LEVEL/AVATAR_MESSAGE");
        _gameAssistant.ShowMessageOneShot(message, ShowTest);
    }

    private void PostTotalScoreToDatabase()
    {
        int finalScore = ScoreManager.Instance.Score;

        StartCoroutine(ServerManager.Instance.PostScoreToDatabase(finalScore, TranslationManager.Instance.GetCurrentCountryLabel()));
    }

    public void ShowNextArticle()
    {
        if(_currentLevel >= _levels.Count)
        {
            _progressTracker.UpdateValue();
            EndLevel();
            return;
        }

        if (_articleObject != null)
        {
            ScoreManager.Instance.CalculateArticlePoints(_articleData);
            _articleData.OnSkip.RemoveAllListeners();
            _articleData.DestroyArticle();
        }

        if (_currentArticle >= _levels[_currentLevel].Count)
        {
            onLevelEnd.Invoke(_currentLevel);
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

            if (_currentArticle++ == 0) {
                // tell ScoreManager that a new Level was reached 
                _fader.StartFade(0.8f, 0.8f, 0.0f);
                onLevelStart.Invoke(_currentLevel);
            }

            onNewArticleSpawned.Invoke(_articleData);
        }
    }

    private void EndLevel()
    {
        PostTotalScoreToDatabase();

        SceneChanger.Instance.ChangeScene("EndGame");

        AnalyticsManager.Instance.SubmitEvent("FreeMode_End");
    }
}
