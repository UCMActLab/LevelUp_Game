using System;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Shortcuts
{
    [Serializable]
    public class ShortcutStruct
    {
        public KeyCode MainKey;
        public KeyCode AdditionalKey;

        public ShortcutAction Action;
    }
}
