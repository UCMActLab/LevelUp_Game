using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons.EditorTopBar
{
    public class SaveButton : UIButton
    {
        [SerializeField] private ProjectSaver _projectSaver;
        [SerializeField] private TooltipUI _tooltip;
        [SerializeField] private string _tooltipText;
        
        protected override void Awake()
        {
            base.Awake();

            AssignAction(SaveChatEditorFile);
        }

        private void SaveChatEditorFile()
        {
            _projectSaver.SaveChatEditorFile();
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

