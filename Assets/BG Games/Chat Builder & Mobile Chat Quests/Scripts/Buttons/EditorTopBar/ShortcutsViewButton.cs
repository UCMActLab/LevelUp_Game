using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.EditorTopBar
{
    public class ShortcutsViewButton : UIButton
    {
        [SerializeField] private ShortcutsPanelUI _shortcutsPanel;
        [SerializeField] private TooltipUI _tooltip;
        [SerializeField] private string _tooltipText;

        protected override void Awake()
        {
            base.Awake();

            AssignAction(ShowHidePanel);
        }

        private void ShowHidePanel()
        {
            _shortcutsPanel.HandleButtonAction();
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _tooltip.Show(_tooltipText, CalculatedOffset());
        }
        
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _tooltip.Hide();
        }
        
        private Vector3 CalculatedOffset()
        {
            var rect =  GetComponent<RectTransform>().rect;
            var heightOffset =  Vector3.down * rect.height * 1.5f;
            var widthOffset =  Vector3.left * rect.width;
            
            return transform.position + heightOffset + widthOffset;
        }
    }
}

