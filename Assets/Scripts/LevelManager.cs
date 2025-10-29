using DA_Assets.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField,
        Tooltip("Article feed, where articles are to be displayed")]
    private ArticleFeed _articleFeed;

    [Header("Levels")]
    [SerializeField] private int _articlesPerLevel = 3;
    [SerializeField] private int _maxLevels = 5;

    int _currentLevel = 0;
    List<List<ArticleData>> _levels;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    [SerializeField]
    private GameObject _loadingAnimation = null;

    private GameObject _articleObject = null;
    private ArticleDataSetter _articleData = null;
    private int _currentArticle = 0;

    private void Start()
    {
        _loadingAnimation?.SetActive(false);
        StartCoroutine(GetArticlesFromResources());
    }

    private IEnumerator GetArticlesFromResources()
    {
        _loadingAnimation?.SetActive(true);
        yield return new WaitUntil(() => ArticleManager.Instance.ArticlesCreated);
        _loadingAnimation?.SetActive(false);

        List<ArticleData> data = ArticleManager.Instance.GetAllArticlesByLanguage((int)LanguageSelection.chosenLanguage);

        data.Shuffle();

        _levels = new List<List<ArticleData>>();
        _levels.Add(new List<ArticleData>());
        int currentLevel = 0;
        foreach(ArticleData articleData in data)
        {
            if (_levels[currentLevel].Count >= _articlesPerLevel && currentLevel < _maxLevels)
            {
                currentLevel++;
                if(currentLevel >= _maxLevels)
                {
                    break;
                }
                _levels.Add(new List<ArticleData>());
            }
            _levels[currentLevel].Add(articleData);
        }

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

        if (_currentLevel >= _levels.Count)
        {
            EndLevel();
        }
        else if (_currentArticle >= _levels[_currentLevel].Count)
        {
            _currentLevel++;
            _currentArticle = 0;

            ScoreManager.Instance.ShowPoints();
            
        }
        else
        {
            _articleObject = Instantiate(_articlePrefab, _articleFeed.transform);
            _articleData = _articleObject.GetComponent<ArticleDataSetter>();
            _articleData.OnSkip.AddListener(ShowNextArticle);
            _articleData.SetArticleData(Instantiate(_levels[_currentLevel][_currentArticle++]));
        }
    }

    private void EndLevel()
    {
        SceneChanger.Instance.ChangeScene("EndGame");
    }
}
