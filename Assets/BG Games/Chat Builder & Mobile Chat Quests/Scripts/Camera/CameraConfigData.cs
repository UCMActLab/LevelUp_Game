using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Camera
{
    [CreateAssetMenu(fileName = "CameraConfigData", menuName = "Configs/CameraConfigData", order = 0)]
    public class CameraConfigData : ScriptableObject
    {
        [SerializeField] private float _cameraZoomSpeed = 40;
        [SerializeField] private float _cameraMovementSpeed = 1;
        [SerializeField] private float _closestZ = -5;
        [SerializeField] private float _furthestZ = -1500;
        [SerializeField] private float _defaultZoom = 25;

        public float CameraZoomSpeed => _cameraZoomSpeed;
        public float CameraMovementSpeed => _cameraMovementSpeed;
        public float ClosestZ => _closestZ;
        public float FurthestZ => _furthestZ;
        public float DefaultZoom => _defaultZoom;
    }

}
