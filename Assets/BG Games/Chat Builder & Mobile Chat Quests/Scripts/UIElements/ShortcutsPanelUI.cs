using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class ShortcutsPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject _shortcutsPanel;

        private bool _isOpen = false;

        public void HandleButtonAction()
        {
            _shortcutsPanel.SetActive(!_isOpen);
            _isOpen = !_isOpen;
        }
    }
}