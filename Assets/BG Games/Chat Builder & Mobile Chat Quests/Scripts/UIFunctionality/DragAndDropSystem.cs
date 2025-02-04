using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality
{
    public class DragAndDropSystem : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Transform _movableObject;

        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerUp;
        public event Action<PointerEventData> Dragged;
        public event Action<Vector3> OnDragging;
        public event Action<Vector3, Vector3> OnDragEnded;

        private bool _isPointerDown;

        private Vector3 _startPosition;
        private bool _dragStarted;
        
        public void MoveBlock(Vector3 delta)
        {
            var position = _movableObject.transform.position;
            position += delta;
            position.z = _movableObject.transform.position.z; 
            _movableObject.transform.position = position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isPointerDown || !InputShortcuts.LeftMouse)
            {
                return;
            }

            var mousePosition = CameraProvider.TransformMousePosition();
            var position = _movableObject.transform.position;
            
            var delta  = CameraProvider.CameraInstance.ScreenToWorldPoint(mousePosition) -
                         CameraProvider.CameraInstance.ScreenToWorldPoint(mousePosition - (Vector3)eventData.delta);
            
            position += delta ;
            position.z = _movableObject.transform.position.z;
            _movableObject.transform.position = position;

            OnDragging?.Invoke(delta);
            Dragged?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
            _dragStarted = true;
            _startPosition = _movableObject.transform.position;
            PointerDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerDown = false;

            PointerUp?.Invoke(eventData);
            if (_dragStarted)
            {
                OnDragEnded?.Invoke(_startPosition, _movableObject.transform.position);
            }

            _dragStarted = false;
        }
    }
}