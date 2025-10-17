using UnityEngine;

[CreateAssetMenu(fileName ="ArticleData", menuName ="ScriptableObjects/ArticleData")]
public class ArticleData : ScriptableObject
{
    public Sprite companyLogo;
    public string companyName;
    public Sprite articleImage;
    public string articleTitle;
    public string articleBody;

    public ConversationType convType = ConversationType.NONE;
    public Conversation conversation = null;

    public bool isTrue = true;
}
