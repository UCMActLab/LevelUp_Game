using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.VisualEffects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.MessageConnection
{
    public class MessageConnector : MonoBehaviour
    {
        [SerializeField] private float _overlapRadius;
        [SerializeField] private int _segmentsAmount = 20;
        [SerializeField] private Vector2 _controlPointOffset;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Button _button;
        [SerializeField] private Vector3 _buttonOffset;

        private CurvedLineRenderer _curvedLineRenderer;
        private ConnectionEndPoint _connectionEndPoint;
        private ConnectionStartPoint _connectionStartPoint;
        public bool IsConnected { get; set; }
        private CommandsHandler _commandsHandler;

        private void Start()
        {
            _button.onClick.AddListener(RemoveConnection);
        }

        public void Init(ConnectionStartPoint connectionStartPoint, CommandsHandler commandsHandler)
        {
            _connectionStartPoint = connectionStartPoint;
            _curvedLineRenderer = new(_lineRenderer, _connectionStartPoint.transform , _endPoint, OffsetType.Start,
                _segmentsAmount);
            _commandsHandler = commandsHandler;
        }

        private void Update()
        {
            CheckForState();
        }

        private void CheckForState()
        {
            if (IsConnected)
            {
                if (CheckOnActive())
                    return;

                MoveTo(_connectionEndPoint.transform.position);
                return;
            }

            if (InputShortcuts.LeftMouseUp)
            {
                TryConnect();
                return;
            }

            MoveToCursor();
        }

        private bool CheckOnActive()
        {
            if (!_connectionEndPoint.Active || !_connectionStartPoint.Active)
            {
                _lineRenderer.enabled = false;
                _button.gameObject.SetActive(false);
                return true;
            }

            _lineRenderer.enabled = true;
            _button.gameObject.SetActive(true);

            return false;
        }

        private void TryConnect()
        {
            var connectionEndPoint = TryGetConnectionEndPoint();
            if (connectionEndPoint == null)
            {
                Destroy(gameObject);
                return;
            }

            _connectionEndPoint = connectionEndPoint;
            _connectionStartPoint.ConnectToEndPoint(_connectionEndPoint);

            MoveTo(_connectionEndPoint.transform.position);

            IsConnected = true;

            CreateConnectionCommand createConnectionCommand =
                new CreateConnectionCommand(this, _connectionStartPoint, _connectionEndPoint);

            _commandsHandler.AddCommand(createConnectionCommand);
        }

        public void Connect(ConnectionEndPoint connectionEndPoint)
        {
            _connectionEndPoint = connectionEndPoint;

            MoveTo(_connectionEndPoint.transform.position);
            IsConnected = true;
        }

        private void RemoveConnection()
        {
            RemoveConnectionCommand removeConnectionCommand =
                new RemoveConnectionCommand(this, _connectionStartPoint, _connectionEndPoint);
            removeConnectionCommand.Execute();
            _commandsHandler.AddCommand(removeConnectionCommand);
        }

        private void MoveToCursor()
        {
            var endPosition = CameraProvider.MousePositionToWorldPoint();
            MoveTo(endPosition);
        }

        private void MoveTo(Vector3 endPosition)
        {
            _button.transform.position = _connectionStartPoint.transform.position + _buttonOffset;
            _endPoint.position = endPosition;

            float xDistance = Mathf.Abs(_endPoint.position.x - _connectionStartPoint.transform.position.x);
            float yDistance = Mathf.Abs(_endPoint.position.y - _connectionStartPoint.transform.position.y);

            var controlPoint = new Vector2
            {
                x = 0,
                y = yDistance
            };

            controlPoint.x += Mathf.Clamp(_controlPointOffset.x, -xDistance, xDistance);
            controlPoint.y += Mathf.Clamp(_controlPointOffset.y, -yDistance, yDistance);

            _curvedLineRenderer.Update(controlPoint);
        }

        private ConnectionEndPoint TryGetConnectionEndPoint()
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResult = new();
            EventSystem.current.RaycastAll(pointerEventData, raycastResult);

            foreach (var objectNearby in raycastResult)
            {
                if (objectNearby.gameObject.TryGetComponent(out ConnectionEndPoint connectionEndPoint))
                {
                    return connectionEndPoint;
                }
            }

            return null;
        }
    }
}