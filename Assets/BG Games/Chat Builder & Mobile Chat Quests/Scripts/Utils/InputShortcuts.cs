using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class InputShortcuts
    {
        public static float MouseScrollWheel => Input.GetAxis("Mouse ScrollWheel");
        public static bool MouseWheelDown => Input.GetMouseButtonDown(2);
        public static bool MouseWheel => Input.GetMouseButton(2);
        public static bool LeftMouseDown => Input.GetMouseButtonDown(0);
        public static bool LeftMouseUp => Input.GetMouseButtonUp(0);
        public static bool LeftMouse => Input.GetMouseButton(0);
    }
}
