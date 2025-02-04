using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public class EmojiContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private LayoutGroup _layout;
        private TMP_InputField _lastInputField;
        

        private bool _isOpen = false;
        
        public void HandleButtonAction()
        {
            _panel.SetActive(!_isOpen);
            _isOpen = !_isOpen;
        }

        private void Awake()
        {
            CreateEmojis();
        }

        private void Update()
        {
            CheckForInputFieldFocus();
        }

        private void CheckForInputFieldFocus()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent<TMP_InputField>(out var inputField))
                {
                    _lastInputField = inputField;
                    break;
                }
            }
        }

        private void CreateEmojis()
        {
            GetUnicodeList().ForEach(CreateEmoji);
        }

        private void CreateEmoji(uint unicode)
        {
            GameObject emojiObject = new(unicode.ToString());
            emojiObject.transform.parent = _layout.transform;

            string emoji = char.ConvertFromUtf32((int)unicode);

            var text = emojiObject.AddComponent<TextMeshProUGUI>();
            text.text = emoji;
            text.enableAutoSizing = true;

            var button = emojiObject.AddComponent<UIButton>();
            button.AssignAction(() => PasteEmoji(emoji));
        }

        private void PasteEmoji(string emoji)
        {
            if (_lastInputField != null)
            {
                int caretPosition = _lastInputField.caretPosition;
                _lastInputField.text = _lastInputField.text.Insert(caretPosition, emoji);
            }
        }

        private List<uint> GetUnicodeList()
        {
            TMP_SpriteAsset spriteAsset = TMP_Settings.defaultSpriteAsset;

            return spriteAsset.fallbackSpriteAssets
                .SelectMany(fallback => fallback.spriteCharacterTable)
                .Select(character => character.unicode)
                .ToList();
        }
    }
}