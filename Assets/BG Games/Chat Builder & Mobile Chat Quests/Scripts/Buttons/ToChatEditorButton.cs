using UnityEngine;
using UnityEngine.SceneManagement;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons
{
    public class ToChatEditorButton : UIButton
    {
        [Header("ToChatEditorButton"), SerializeField] private string _chatEdtorSceneName = "ChatBuilder";
        
        protected override void Awake()
        {
            base.Awake();
            
            AssignAction(OpenChatEditor);
        }
        
        private void OpenChatEditor()
        {
            SceneManager.LoadScene(_chatEdtorSceneName);
        }
    }
}