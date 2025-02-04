using TMPro;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class LetterCounter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TextMeshProUGUI _counter;

        [SerializeField] private int _maxCount = 42;
        [SerializeField] private float _defaultTextSize;
        [SerializeField] private float _warningTextSize;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _warningColor;
        private void Start()
        {
            _inputField.onValueChanged.AddListener(ChangeCounter);
            _inputField.characterLimit = _maxCount;
            ChangeCounter(_inputField.text);
        }
        
        private void ChangeCounter(string value)
        {
            _counter.text = $"{value.Length }/{_maxCount}";
            if (value.Length >= _maxCount)
            {
                _counter.color = _warningColor;
                _counter.fontSize = _warningTextSize;
            }
            else
            {
                _counter.color = _defaultColor;
                _counter.fontSize = _defaultTextSize;


            }
        }
    }
}