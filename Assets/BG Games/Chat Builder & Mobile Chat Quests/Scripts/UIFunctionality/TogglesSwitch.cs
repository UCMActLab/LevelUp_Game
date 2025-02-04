using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality
{
    public class TogglesSwitch : MonoBehaviour
    {
        [SerializeField] private Toggle _textToggle;
        [SerializeField] private Toggle _imageToggle;

        [SerializeField] private GameObject _inputField;
        [SerializeField] private GameObject _image;

        private void Start()
        {
            _textToggle.onValueChanged.AddListener(OnTextToggleSwitch);
            _imageToggle.onValueChanged.AddListener(OnImageToggleSwitch);
        }

        private void OnTextToggleSwitch(bool isOn)
        {
            if(isOn)        _imageToggle.isOn = false;
            _inputField.SetActive(isOn);
        }

        private void OnImageToggleSwitch(bool isOn)
        {if(isOn)
                _textToggle.isOn = false;
            _image.SetActive(isOn);
        }
    }
}