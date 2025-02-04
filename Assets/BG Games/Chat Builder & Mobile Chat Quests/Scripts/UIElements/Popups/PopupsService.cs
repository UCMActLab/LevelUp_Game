using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class PopupsService : MonoBehaviour
    {
        [SerializeField] private Transform popupHolder;
        [SerializeField] private List<PopupBase> popupPrefabs;

        private readonly Stack<PopupBase> _activePopups = new();

        public T ShowPopup<T>(PopupType popupType, System.Action<T> initializer = null) where T : PopupBase
        {
            PopupBase popupPrefab = popupPrefabs.Find(popup => popup.type == popupType) as T;

            if (popupPrefab != null)
            {
                var popupInstance = Instantiate(popupPrefab, popupHolder) as T;
                popupInstance!.OnClose += HandlePopupClose;
                _activePopups.Push(popupInstance);

                initializer?.Invoke(popupInstance);

                popupInstance.Show();
                return popupInstance;
            }
            else
            {
                Debug.LogError($"Missing popup with type {popupType}!");
                return null;
            }
        }

        public void CloseTopPopup()
        {
            if (_activePopups.Count > 0)
            {
                var topPopup = _activePopups.Pop();
                topPopup.Hide();
                topPopup.OnClose -= HandlePopupClose;
                Destroy(topPopup.gameObject);
            }
        }

        private void HandlePopupClose(PopupBase popup)
        {
            CloseTopPopup();
        }
    }

}

