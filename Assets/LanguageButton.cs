using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    public void ChangeLangauge(int newLanguage)
    {
        LanguageSelection.chosenLanguage = (Language)newLanguage;
        TranslationManager.Instance.ChangeLanguage(newLanguage);
    }
}
