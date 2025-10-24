using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    public void ChangeLangauge(int langauge)
    {
        TranslationManager.Instance.ChangeLanguage(langauge);
    }
}
