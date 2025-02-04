using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.ChatSerialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera
{
    public class CameraControlService : MonoBehaviour
    {
        [SerializeField] private CameraConfigData _cameraConfig;

        private Vector3 _lastMousePosition;

        private CameraDragHandler _dragController;
        private CameraZoomHandler _zoomController;

        private void Start() => InitializeServices();

        private void OnEnable() => AddEventListeners();

        private void OnDisable() => RemoveEventListeners();

        private void Update()
        {
            HandleZoom();
            HandleMovement();
        }

        private void AddEventListeners()
        {
            ChatToJsonValidator.ErrorOccurred += MoveCameraTo;
        }

        private void RemoveEventListeners()
        {
            ChatToJsonValidator.ErrorOccurred -= MoveCameraTo;
        }

        private void InitializeServices()
        {
            _dragController = new CameraDragHandler(_cameraConfig.CameraMovementSpeed);
            _zoomController = new CameraZoomHandler(_cameraConfig);
        }

        private void HandleZoom()
        {
            _zoomController.CheckForZoom(CameraProvider.CameraInstance, CameraProvider.MousePositionToWorldPoint(), InputShortcuts.MouseScrollWheel);
        }

        private void HandleMovement()
        {
            if (InputShortcuts.MouseWheelDown)
            {
                _dragController.PrepareMoving(CameraProvider.TransformMousePosition());
            }

            if (InputShortcuts.MouseWheel)
            {
                _dragController.DragCamera(CameraProvider.CameraInstance, CameraProvider.TransformMousePosition());
            }
        }

        private void MoveCameraTo(BlockObject target)
        {
            var targetPosition = target.transform.position;
            CameraProvider.CameraInstance.transform.position = new Vector3(targetPosition.x, targetPosition.y, -_cameraConfig.DefaultZoom);
        }
    }
}