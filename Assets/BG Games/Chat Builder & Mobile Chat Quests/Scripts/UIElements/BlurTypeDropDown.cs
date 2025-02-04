using System;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class BlurTypeDropDown : MonoBehaviour
    {
        private void Awake()
        {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();

            string[] values = Enum.GetNames(typeof(BlurType));
            dropdown.options = values
                .Select(value => new TMP_Dropdown.OptionData(EnumSplitExtention.SplitWordsInEnumName(value)))
                .ToList();
        }
    }
}
