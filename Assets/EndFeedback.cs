using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _articlesRead;
    [SerializeField] private TextMeshProUGUI _falseArticlesShared;
    [SerializeField] private TextMeshProUGUI _trueArticlesShared;
    [SerializeField] private Image _medalImage;

    public void SetTitle(string text)
    {
        _titleText.SetText(text);
    }

    public void SetArticlesRead(int read, int total)
    {
        _articlesRead.SetText("Has leído " + read + "/" + total + " artículos");
    }

    public void SetFalseShared(int falseShared)
    {
        _falseArticlesShared.SetText("Has compartido " + falseShared + " artículos falsos");
    }

    public void SetTrueShared(int trueShared) 
    {
        _trueArticlesShared.SetText("Has compartido " + trueShared + " artículos verdaderos");
    }

    public void SetImage(Sprite image)
    {
        _medalImage.sprite = image;
    }
}
