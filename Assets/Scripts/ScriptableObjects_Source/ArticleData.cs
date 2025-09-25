using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="ArticleData", menuName ="ScriptableObjects/ArticleData")]
public class ArticleData : ScriptableObject
{
    public Sprite companyLogo;
    public string companyName;
    public float hoursAgoPosted;
    public Sprite articleImage;
    public string articleTitle;
    public string articleBody;
}
