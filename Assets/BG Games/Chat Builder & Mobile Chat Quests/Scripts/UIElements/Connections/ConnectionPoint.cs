using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections
{
    public class ConnectionPoint : MonoBehaviour
    {
        public bool Active { get; private set; }

        public void OnDisable()
        {
            Active = false;
        }

        public void OnEnable()
        {
            Active = true;
        }
    }
}