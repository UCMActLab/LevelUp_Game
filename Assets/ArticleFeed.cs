using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ArticleFeed : MonoBehaviour
{
    List<ArticleDataSetter> _articles;

    private void Start()
    {
        _articles = GetComponentsInChildren<ArticleDataSetter>().ToList();
    }

    public void SkipArticle(ArticleDataSetter art)
    {
        art.SkipArticle();

        _articles.Remove(art);
    }
}
