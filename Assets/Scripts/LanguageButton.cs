using Unity.Services.Analytics;
using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    public void ChangeLangauge(int newLanguage)
    {
        LanguageSelection.chosenLanguage = (Language)newLanguage;
        TranslationManager.Instance.ChangeLanguage(newLanguage);

        CustomEvent newEvent = new CustomEvent("LANGUAGE_SELECTOR")
        {
            { "chosenLanguage", newLanguage }
        };

        AnalyticsManager.Instance.SubmitEvent(newEvent);
    }
}
