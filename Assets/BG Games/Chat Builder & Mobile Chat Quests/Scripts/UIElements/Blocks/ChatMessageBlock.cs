using System;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIFunctionality;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public class ChatMessageBlock : MessageBlock
    {
        [SerializeField] private Toggle _textToggle;
        [SerializeField] private Toggle _imageToggle;
        [SerializeField] private TMP_Dropdown _blurTypeDropdown;
        [SerializeField] private ImageContent _imageContent;
        [SerializeField] private Outline _outline;
        [SerializeField] private TMP_InputField _imagePriceInputField;

        private string _lastPrice;

        public DragAndDropSystem DragAndDropSystem => _dragAndDropSystem;

        public event Action BlurTypeChanged;
        public event Action<ChatMessageBlock> OnRemoved;
        public event Action<ChatMessageBlock, Vector3> OnDragging;

        public event Action<ChatMessageBlock, bool> SelectedMessageBlock;
        public bool HasContent => _textToggle.isOn || _imageToggle.isOn;

        public ContentType ContentType => _textToggle.isOn ? ContentType.Text : ContentType.Image;
        public int Price => int.Parse(_imagePriceInputField.text);

        public override string Content
        {
            get
            {
                if (ContentType == ContentType.Text)
                {
                    return LastText;
                }

                string id = _imageContent.CashedImage.Id;
                return id;
            }
        }


        protected override void Start()
        {
            base.Start();
            _blurTypeDropdown.onValueChanged.AddListener(OnBlurTypeValueChanged);
            LastText = InputField.text;
            _lastPrice = _imagePriceInputField.text;
            InputField.onEndEdit.AddListener(OnInputFieldChanged);
            _imagePriceInputField.onEndEdit.AddListener(OnImagePriceChanged);
            _imageContent.Init(ImageSavingService, CommandsHandler, DroppedImageManager);
            _dragAndDropSystem.OnDragging += OnDraggingHandler;
        }

        protected override void RemoveBlock()
        {
            base.RemoveBlock();
            OnRemoved?.Invoke(this);
        }

        public void Remove()
        {
            base.RemoveBlock();
        }

        public void HandleObjectSelection(bool isMultipleSelection)
        {
            EnableOutline();
            SelectedMessageBlock?.Invoke(this, isMultipleSelection);
        }

        public void EnableOutline()
        {
            _outline.enabled = true;
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }

        public void LoadData(MessageSolutionInfo messageSolutionInfo, ImageSavingService imageSavingService)
        {
            ImageSavingService = imageSavingService;
            LoadData(messageSolutionInfo);
        }

        public void LoadData(MessageSolutionInfo messageSolutionInfo)
        {
            _imageContent.Init(ImageSavingService, CommandsHandler, DroppedImageManager);
            _blurTypeDropdown.value = messageSolutionInfo.BlurType ? (int)BlurType.Blur : (int)BlurType.NoBlur;
            transform.position = messageSolutionInfo.Position;
            
            LocalisationDictionary = messageSolutionInfo
                .LocalisationDictionary
                .ToDictionary(entry => entry.Key,
                entry => entry.Value);
            
            LocalisationDictionary.TryAdd(LanguageSwitch.StartLanguage, "");

            InputField.text = LocalisationDictionary[LanguageSwitch.StartLanguage];
            _imagePriceInputField.text = messageSolutionInfo.ImagePrice.ToString();
            _textToggle.isOn = messageSolutionInfo.IsText;
            _imageToggle.isOn = messageSolutionInfo.IsImage;
            if (_imageToggle.isOn)
            {
                _imageContent.LoadImage(messageSolutionInfo.ImageId, messageSolutionInfo.ImageFormat);
            }
        }

        public MessageSolutionInfo GetMessageSolutionInfo()
        {
            MessageSolutionInfo messageSolutionInfo = GetMessageBlockMainInfo();
            if (_imageToggle.isOn)
            {
                _imageContent.GetImageData(out var id, out ImageFormat format);

                messageSolutionInfo.ImageId = id;
                messageSolutionInfo.ImageFormat = format;
                messageSolutionInfo.ImagePrice = int.Parse(_imagePriceInputField.text);
            }

            messageSolutionInfo.AnswerInfos = GetAnswersData();
            return messageSolutionInfo;
        }

        private MessageSolutionInfo GetMessageBlockMainInfo()
        {
            MessageSolutionInfo messageSolutionInfo = new MessageSolutionInfo
            {
                Id = Id,
                Position = transform.position,
                IsText = _textToggle.isOn,
                IsImage = _imageToggle.isOn,
                LocalisationDictionary = LocalisationDictionary,
                NextMessageId = ConnectionStartPoint.Id,
                BlurType = _blurTypeDropdown.value == (int)BlurType.Blur,
            };
            
            return messageSolutionInfo;
        }

        private void OnBlurTypeValueChanged(int arg0)
        {
            BlurTypeChanged?.Invoke();
        }

        private void OnDisable()
        {
            _blurTypeDropdown.onValueChanged.RemoveListener(OnBlurTypeValueChanged);
        }

        private void OnDestroy()
        {
            _dragAndDropSystem.OnDragging -= OnDraggingHandler;
        }

        private void OnDraggingHandler(Vector3 offset)
        {
            OnDragging?.Invoke(this, offset);
        }
        
        private void OnImagePriceChanged(string newText)
        {
            ChangeTextCommand command = new ChangeTextCommand(_imagePriceInputField, _lastPrice, newText);
            command.Execute();
            _lastPrice = newText;
            CommandsHandler.AddCommand(command);
        }
    }
}