using DA_Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    List<List<ArticleData>> _levels;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    [SerializeField]
    private GameObject _loadingAnimation = null;

    private GameObject _articleObject = null;
    private ArticleGameObject _articleData = null;
    private int _currentArticle = 0;

    [SerializeField]
    private TestLogic _testLogic = null;

    private void Start()
    {
        _loadingAnimation?.SetActive(false);
        StartCoroutine(GetArticlesFromResources());

        if (!_testLogic) _testLogic = GameObject.FindFirstObjectByType<TestLogic>();
    }

    private IEnumerator GetArticlesFromResources()
    {
        _loadingAnimation?.SetActive(true);
        yield return new WaitUntil(() => ArticleManager.Instance.ArticlesCreated);
        _loadingAnimation?.SetActive(false);

        List<ArticleData> data = ArticleManager.Instance.GetAllArticlesByLanguage((int)LanguageSelection.chosenLanguage);

        data.Shuffle();

        _levels = new List<List<ArticleData>>();
        int currentLevel = 0;
        int numArticlesTotal = 0;

        foreach(LevelInfo level in _levelsInfo)
        {
            _levels.Add(new List<ArticleData>());
            for (int i = 0; i < level.numArticles; ++i)
            {
                _levels[currentLevel].Add(data[0]);
                data.RemoveAt(0);
            }

            numArticlesTotal += level.numArticles;
            currentLevel++;
        }

        ScoreManager.Instance.SetMaxScore(numArticlesTotal);

        if (_startOnAwake)
        {
            ShowNextArticle();
        }
    }

    public void ShowNextArticle()
    {

        if (_articleObject != null)
        {
            _articleData.OnSkip.RemoveAllListeners();
            _articleData.DestroyArticle();
        }

        if (_currentArticle >= _levels[_currentLevel].Count)
        {
            // ScoreManager.Instance.ShowPoints();
            if (_currentLevel + 1 >= _levels.Count)
            {
                EndLevel();
            }
            else
            {
                _testLogic.SetTest(_levelsInfo[_currentLevel].test, true);
                _currentLevel++;
                _currentArticle = 0;
            }
        }
        else
        {
            if(_currentArticle == 0) ScoreManager.Instance.SetLevelInfo(_levelsInfo[_currentLevel].numArticles);

            _articleObject = Instantiate(_articlePrefab, _articleFeed.transform);
            _articleData = _articleObject.GetComponent<ArticleGameObject>();
            _articleData.OnSkip.AddListener(ShowNextArticle);
            _articleData.SetArticleData(Instantiate(_levels[_currentLevel][_currentArticle++]));

        }
    }

    private void EndLevel()
    {
        SceneChanger.Instance.ChangeScene("EndGame");
    }
}
