using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera
{
    public class CameraZoomHandler
    {
        private readonly float _zoomSpeed;
        private readonly float _closestZ;
        private readonly float _furthestZ;

        public CameraZoomHandler(CameraConfigData configData)
        {
            _zoomSpeed = configData.CameraZoomSpeed;
            _closestZ =  configData.ClosestZ;
            _furthestZ = configData.FurthestZ;
        }

        public void CheckForZoom(UnityEngine.Camera camera, Vector3 mousePosToWorldPoint, float mouseScrollWheel)
        {
            if (mouseScrollWheel == 0) return;

            Vector3 oldWorldMousePosition = mousePosToWorldPoint;

            Vector3 newCameraPosition = camera.transform.position + _zoomSpeed * mouseScrollWheel * Vector3.forward;
            newCameraPosition.z = CalculateZValue(newCameraPosition.z);

            camera.transform.position = newCameraPosition;

            Vector3 worldMousePosition = CameraProvider.MousePositionToWorldPoint();
            Vector3 worldPositionDelta = oldWorldMousePosition - worldMousePosition;

            camera.transform.position += worldPositionDelta;
        }

        private float CalculateZValue(float currentZ)
        {
            float minZ = Mathf.Min(_closestZ, _furthestZ);
            float maxZ = Mathf.Max(_closestZ, _furthestZ);

            return Mathf.Clamp(currentZ, minZ, maxZ);
        }
    }
}
