using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons;
using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Popups
{
    public class ChooseAssetButton : UIButton
    {
        [SerializeField] private TMP_Text assetNameText;
        [SerializeField] private TMP_Text assetDateText;

        public void Setup(string assetName, string assetDate)
        {
            assetNameText.text = assetName;
            assetDateText.text = assetDate;
        }
    }

}
