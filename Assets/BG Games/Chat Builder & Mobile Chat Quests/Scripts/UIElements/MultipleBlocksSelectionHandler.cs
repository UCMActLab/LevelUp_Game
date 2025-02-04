using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils.MessageBlockUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class MultipleBlocksSelectionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _blockLayerMask;
        [SerializeField] private RectTransform _canvasMain;
        [SerializeField] private RectTransform _selectionBox;
        [SerializeField] private MessageBlockHandler _messageBlockHandler;
   
        private Vector2 _startPos;
        private Vector2 _endPos;
        private bool _isSelecting = false;
   
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsClickOnBlockOrUI()) 
                {
                    _startPos = Input.mousePosition;
                    _selectionBox.gameObject.SetActive(true);
                    _isSelecting = true;
                }
            }
            if (Input.GetMouseButton(0) && _isSelecting) 
            {
                _endPos = Input.mousePosition;
                UpdateSelectionBox();
            }
            if (Input.GetMouseButtonUp(0) && _isSelecting) 
            {
                _selectionBox.gameObject.SetActive(false);
                SelectBlocksInArea();
                _isSelecting = false;
            }
        }
   
        private bool IsClickOnBlockOrUI()
        {
            var pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var results = new List<RaycastResult>();
       
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                if ((_blockLayerMask & (1 << result.gameObject.layer)) != 0)
                    return true; 
            }
            return false; 
        }
   
        private void UpdateSelectionBox()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasMain, _startPos, null, out var startLocalPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasMain, _endPos, null, out var endLocalPos);

            Vector2 boxCenter = (startLocalPos + endLocalPos) / 2;
            Vector2 sizeDelta = new Vector2(Mathf.Abs(startLocalPos.x - endLocalPos.x), Mathf.Abs(startLocalPos.y - endLocalPos.y));

            _selectionBox.localPosition = boxCenter;
            _selectionBox.sizeDelta = sizeDelta;
        }
   
        private void SelectBlocksInArea()
        {
            if (_messageBlockHandler.AllMessages.Count == 0)
                return;
       
            _messageBlockHandler.ClearSelection();
       
            foreach (var block in _messageBlockHandler.AllMessages)
            {
                Vector3 screenPos = CameraProvider.CameraInstance.WorldToScreenPoint(block.transform.position);
                if (IsWithinSelectionBounds(screenPos) && block.gameObject.activeSelf)
                {
                    block.HandleObjectSelection(true);
                }
            }
        }

        private bool IsWithinSelectionBounds(Vector3 screenPosition)
        {
            float minX = Mathf.Min(_startPos.x, _endPos.x);
            float maxX = Mathf.Max(_startPos.x, _endPos.x);
            float minY = Mathf.Min(_startPos.y, _endPos.y);
            float maxY = Mathf.Max(_startPos.y, _endPos.y);

            return screenPosition.x > minX && screenPosition.x < maxX && 
                   screenPosition.y > minY && screenPosition.y < maxY;
        }
    }
}
