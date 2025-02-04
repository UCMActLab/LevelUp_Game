using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera
{
    public class CameraDragHandler
    {
        private Vector3 _lastMousePosition;
        private readonly float _movementSpeed;

        public CameraDragHandler( float movementSpeed)
        {
            _movementSpeed = movementSpeed;
        }

        public void PrepareMoving(Vector3 transformMousePos)
        {
            _lastMousePosition = transformMousePos;
        }

        public void DragCamera(UnityEngine.Camera camera, Vector3 transformMousePos)
        {
            var mousePosition = transformMousePos;
            var worldMousePosition = camera.ScreenToWorldPoint(mousePosition);
            var lastWorldMousePosition = camera.ScreenToWorldPoint(_lastMousePosition);

            var position = camera.transform.position;
            position += (lastWorldMousePosition - worldMousePosition) * _movementSpeed;
            position.z = camera.transform.position.z;
            camera.transform.position = position;

            _lastMousePosition = mousePosition;
        }
    }
}

