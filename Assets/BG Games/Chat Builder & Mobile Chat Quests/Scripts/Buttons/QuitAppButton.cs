using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons
{
    public class QuitAppButton : UIButton
    {
        protected override void Awake()
        {
            base.Awake();

            AssignAction(QuitApp);
        }

        private void QuitApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}