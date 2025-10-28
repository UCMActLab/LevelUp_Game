using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void goGameSceneSpanish()
    {
        LanguageSelection.chosenLanguage = Language.spanish;
        SceneManager.LoadScene("MainMenu");
    }

    public void goGameSceneCroatian()
    {
        LanguageSelection.chosenLanguage = Language.czech;
        SceneManager.LoadScene("LoadingScene");
    }

    public void goGameSceneBulgarian()
    {
        LanguageSelection.chosenLanguage = Language.bulgarian;
        SceneManager.LoadScene("LoadingScene");
    }

    public void goGeneralScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void goLanguageScene()
    {
        SceneManager.LoadScene("LanguageMenu");
    }

}
