using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.EditorTopBar
{
    public class UndoButton : UIButton
    {
        [SerializeField] private CommandsHandler _commandsHandler;
        [SerializeField] private TooltipUI _tooltip;
        [SerializeField] private string _tooltipText;
       
        protected override void Awake()
        {
            base.Awake();

            AssignAction(Undo);
        }

        private void Undo()
        {
            _commandsHandler.UndoCommand();
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
            var heightOffset =  Vector3.down *rect.height;
            
            return transform.position + heightOffset;
        }
    }
}


