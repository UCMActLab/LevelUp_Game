using UnityEngine;
 
[CreateAssetMenu(fileName ="ArticleData", menuName ="ScriptableObjects/ArticleData")]
public class ArticleData : ScriptableObject
{
    public bool needsTranslation;

    public Sprite companyLogo;
    public string companyName;
    public Sprite articleImage;
    public string articleTitle;
    public string articleBody;
    public string ID;
    public string theme;

    public bool canBeSharedWithGroups; // true -> hay grupos | false -> solo se "valida", no se envía
    public int numGroupsToShareWith; // > 0 si hay grupos | 

    public ConversationType convType = ConversationType.NONE;
    public System.Collections.Generic.List<Conversation> conversation;

    public bool[] sharedWithGroups;

    public bool isTrue = true;
}
