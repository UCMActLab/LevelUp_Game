using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArticleDataSetter : MonoBehaviour
{
    [SerializeField] Image _companyLogo = null;
    [SerializeField] TextMeshProUGUI _companyText = null;
    [SerializeField] TextMeshProUGUI _postedHoursAgo = null;
    [SerializeField] Image _articleImage = null;
    [SerializeField] TextMeshProUGUI _articleTitle = null;
    [SerializeField] TextMeshProUGUI _likes = null;
    [SerializeField] TextMeshProUGUI _reposts = null;

    ArticleData _data = null;

    public void SetArticleData(ArticleData data)
    {
        if (data == null) return;

        _data = data;
        
        _companyLogo.sprite = _data.companyLogo;
        _companyText.text = _data.companyName;
        _postedHoursAgo.text = _data.hoursAgoPosted + "h";
        _articleImage.sprite = _data.articleImage;
        _articleTitle.text = _data.articleTitle;
        _likes.text = _data.likes.ToString();
        _reposts.text = _data.reposts.ToString();
    }
}
