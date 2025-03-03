using UnityEngine;
using UnityEngine.SceneManagement;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.SceneManagement
{
   public class EditorSceneChanger : MonoBehaviour
   {
      private void OnEnable()
      {
         PlayerPrefs.DeleteAll();
      }

      public void RestartScene()
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
