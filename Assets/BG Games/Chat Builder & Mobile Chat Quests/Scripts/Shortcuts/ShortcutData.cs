using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Shortcuts
{
    [CreateAssetMenu(fileName = "ShortcutData", menuName = "Shortcut/ShortcutData", order = 0)]
    public class ShortcutData : ScriptableObject
    {
        [SerializeField] private List<ShortcutStruct> shortcuts;

        public List<ShortcutStruct> Shortcuts => shortcuts;
    }
}
