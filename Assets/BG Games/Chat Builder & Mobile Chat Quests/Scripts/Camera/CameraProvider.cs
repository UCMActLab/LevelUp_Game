using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera
{
    public class CameraProvider : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        public static UnityEngine.Camera CameraInstance { get; private set; }

        private void Awake()
        {
            CameraInstance = _camera;
        }

        public static Vector3 MousePositionToWorldPoint()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(CameraInstance.transform.position.z);
        
            var worldMousePosition = CameraInstance.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0;

            return worldMousePosition;
        }

        public static Vector3 MousePositionToWorldPoint(Vector2 screenPoint)
        {
            Vector3 mousePosition = screenPoint;
            mousePosition.z = Mathf.Abs(CameraInstance.transform.position.z);

            var worldMousePosition = CameraInstance.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0;

            return worldMousePosition;
        }

        public static Vector3 TransformMousePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(CameraInstance.transform.position.z);

            return mousePosition;
        }

        public static Vector3 TransformMousePosition(Vector2 screenPoint)
        {
            Vector3 mousePosition = screenPoint;
            mousePosition.z = Mathf.Abs(CameraInstance.transform.position.z);

            return mousePosition;
        }
    }
}
