using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField, 
        Tooltip("Articles that will take part in this level. First article in the" +
        " array is the first to show.")]
    // ============================================
    // 
    // ESTO EN ALGÚN MOMENTO LO TIENE QUE RECIBIR 
    // DE LA BASE DE DATOS QUE HAGA ANDREA. DEBEN
    // SER INSTANCIAS DE ArticleData GENERADOS
    // CON LOS DATOS DEL DRIVE HIHI
    //
    // ============================================
    private ArticleData[] _articles;
    [SerializeField,
        Tooltip("Article feed, where articles are to be displayed")]
    private ArticleFeed _articleFeed;

    [SerializeField]
    private GameObject _articlePrefab;

    [SerializeField]
    private bool _startOnAwake = true;

    private GameObject _articleObject = null;
    private ArticleDataSetter _articleData = null;
    private int _currentArticle = 0;

    private bool _pointsShowedToPlayer = true;

    private void Start()
    {
        if(_startOnAwake)
        {
            ShowNextArticle();
        }
    }

    public void ShowNextArticle()
    {
        if (_articleObject != null)
        {
            _articleData.OnSkip.RemoveAllListeners();
            Destroy(_articleObject);
        }

        if (!_pointsShowedToPlayer)
        {
            ScoreManager.Instance.ShowPoints();
            _pointsShowedToPlayer = true;
        }
        else if (_currentArticle >= _articles.Length)
        {
            EndLevel();
        }
        else
        {
            _articleObject = Instantiate(_articlePrefab, _articleFeed.transform);
            _articleData = _articleObject.GetComponent<ArticleDataSetter>();
            _articleData.OnSkip.AddListener(ShowNextArticle);
            _articleData.SetArticleData(Instantiate(_articles[_currentArticle++]));
            _pointsShowedToPlayer = false;
            // _articleData.OnShare.AddListener(ArticleShared);
        }
    }

    private void EndLevel()
    {
        SceneChanger.Instance.ChangeScene("EndGame");
    }
}
