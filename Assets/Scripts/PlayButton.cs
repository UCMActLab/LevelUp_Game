using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void goGameSceneEnglish()
    {
        LanguageSelection.chosenLanguage = Language.english;
        SceneManager.LoadScene("ChatDemo");
    }

    public void goGameSceneSpanish()
    {
        LanguageSelection.chosenLanguage = Language.spanish;
        SceneManager.LoadScene("ChatDemo");
    }

    public void goGameSceneOther()
    {
        LanguageSelection.chosenLanguage = Language.other;
        SceneManager.LoadScene("ChatDemo");
    }

    public void goLanguageScene()
    {
        SceneManager.LoadScene("LanguageMenu");
    }
}
