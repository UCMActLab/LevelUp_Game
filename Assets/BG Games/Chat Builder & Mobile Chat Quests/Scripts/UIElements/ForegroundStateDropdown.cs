using System;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Enums;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ForegroundStateDropdown : MonoBehaviour
    {
        private void Awake()
        {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();

            string[] values = Enum.GetNames(typeof(ForegroundState));
            dropdown.options = values.Select(value => new TMP_Dropdown.OptionData(value)).ToList();
        }
    }
}