using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons
{
    public class TooltipUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void Show(string tooltipText, Vector3 newPosition)
        {
            text.text = tooltipText;
            transform.position = newPosition;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
