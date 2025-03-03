using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void onClick()
    {
        SceneManager.LoadScene("ChatDemo");
    }
}
