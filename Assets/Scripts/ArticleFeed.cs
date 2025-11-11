using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ArticleFeed : MonoBehaviour
{
    List<ArticleGameObject> _articles;

    private void Start()
    {
        _articles = GetComponentsInChildren<ArticleGameObject>().ToList();
    }

    public void SkipArticle(ArticleGameObject art)
    {
        art.SkipArticle();

        _articles.Remove(art);
    }
}
