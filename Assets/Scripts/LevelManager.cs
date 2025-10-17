using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField, 
        Tooltip("Articles that will take part in this level. First article in the" +
        " array is the first to show.")] 
    private ArticleData[] _articles;
    [SerializeField,
        Tooltip("Article feed, where articles are to be displayed")]
    private ArticleFeed _articleFeed;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    private GameObject _articleObject = null;
    private int _currentArticle = 0;

    private void Start()
    {
        if(_startOnAwake)
        {
            ShowNextArticle();
        }
    }

    public void ShowNextArticle()
    {
        if (_articleObject != null) Destroy(_articleObject);

        if (_currentArticle >= _articles.Length)
        {
            EndLevel();
        }
        else
        {
            _articleObject = Instantiate(_articlePrefab, _articleFeed.transform);
            ArticleDataSetter dataSetter = _articleObject.GetComponent<ArticleDataSetter>();
            dataSetter.SetArticleData(Instantiate(_articles[_currentArticle++]));
            dataSetter.OnShare.AddListener(ArticleShared);
        }
    }

    private void ArticleShared()
    {
        if(_articleObject.GetComponent<ArticleDataSetter>().IsTrue)
        {
            // TODO
        }
        // TODO
    }

    private void EndLevel()
    {
        SceneChanger.Instance.ChangeScene("EndGame");
    }
}
