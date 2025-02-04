using System;
using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Shortcuts
{
    public class ShortcutDetector : MonoBehaviour
    {
        [SerializeField] private ShortcutData shortcutData;

        private Dictionary<ShortcutAction, HashSet<Action>> _shortcutEvents;

        private void Awake()
        {
            InitializeShortcutEvents();
        }

        private void Update()
        {
            foreach (var shortcut in shortcutData.Shortcuts)
            {
                if (Input.GetKey(shortcut.MainKey) && Input.GetKeyDown(shortcut.AdditionalKey) && _shortcutEvents.TryGetValue(shortcut.Action, out var actions))
                {
                    foreach (var action in actions)
                    {
                        action?.Invoke();
                    }
                }
            }
        }

        private void InitializeShortcutEvents()
        {
            _shortcutEvents = new Dictionary<ShortcutAction, HashSet<Action>>();
            foreach (ShortcutAction action in Enum.GetValues(typeof(ShortcutAction)))
            {
                _shortcutEvents[action] = new HashSet<Action>();
            }
        }

        public void AddListener(ShortcutAction shortcutAction, Action action)
        {
            if (_shortcutEvents.ContainsKey(shortcutAction))
            {
                _shortcutEvents[shortcutAction].Add(action);
            }
        }

        public void RemoveListener(ShortcutAction shortcutAction, Action action)
        {
            if (_shortcutEvents.ContainsKey(shortcutAction))
            {
                _shortcutEvents[shortcutAction].Remove(action);
            }
        }
    }
}
