using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class DraggableMessageImage : MessageImage, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
        
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DateImage dateImage = TryGetMessageImage();

            dateImage?.SetCashedImage(CashedImage);
        }
        
        private DateImage TryGetMessageImage()
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out DateImage dateImage))
                {
                    return dateImage;
                }
            }

            return null;
        }
    }
}