using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class EndFeedback : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent _titleText;
    [SerializeField] private TextMeshProUGUI _articlesRead;
    [SerializeField] private LocalizedString _articlesReadLocalized;
    [SerializeField] private TextMeshProUGUI _falseArticlesShared;
    [SerializeField] private LocalizedString _falseArticlesSharedLocalized;
    [SerializeField] private TextMeshProUGUI _trueArticlesShared;
    [SerializeField] private LocalizedString _trueArticlesSharedLocalized;
    [SerializeField] private Image _medalImage;

    public void SetTitle(string text)
    {
        _titleText.StringReference.SetReference("Translation", text);
    }

    public void SetArticlesRead(int read, int total)
    {
        _articlesRead.SetText(string.Format(_articlesReadLocalized.GetLocalizedString(), read, total));
    }

    public void SetFalseShared(int falseShared)
    {
        string text = string.Format(_falseArticlesSharedLocalized.GetLocalizedString(), falseShared);
        _falseArticlesShared.SetText(text);
    }

    public void SetTrueShared(int trueShared) 
    {
        _trueArticlesShared.SetText(string.Format(_trueArticlesSharedLocalized.GetLocalizedString(), trueShared));
    }

    public void SetImage(Sprite image)
    {
        _medalImage.sprite = image;
    }
}
