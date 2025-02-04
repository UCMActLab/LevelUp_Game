using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices
{
    public struct DroppedImageInfo
    {
        public string Path { get; private set; }
        public Vector2 Point { get; private set; }

        public DroppedImageInfo(string path, Vector2 point)
        {
            Path = path;
            Point = point;
        }
    }
}