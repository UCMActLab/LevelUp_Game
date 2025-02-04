using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.SceneManagement
{
    public class EditorStarter : MonoBehaviour
    {
        private void Start()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}