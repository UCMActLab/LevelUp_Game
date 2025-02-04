using System;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        public PopupType type;
        public event Action<PopupBase> OnClose;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnClose?.Invoke(this);
        }
    }

}
