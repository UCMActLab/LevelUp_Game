using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public class AnswerBlock : TextHolderBlock
    {
        [SerializeField] private TMP_Dropdown _priceTypeDropdown;
        [SerializeField] private Image _background;
        [SerializeField] private Color _defaultBackground;
        [SerializeField] private Color _costBackground;
        [SerializeField] private Color _warningBackground;

        public event Action PriceTypeChanged;

        protected override void Start()
        {
            base.Start();
            _priceTypeDropdown.onValueChanged.AddListener(OnPriceTypeValueChanged);
        }

        public PriceType PriceType
        {
            get
            {
                string priceTypeString = _priceTypeDropdown.options[_priceTypeDropdown.value].text;
                PriceType priceType = (PriceType)Enum.Parse(typeof(PriceType), priceTypeString);

                return priceType;
            }
        }

        public override string Content => LastText;

        public void ChangeBackground(bool warning)
        {
            _background.color = warning ? _warningBackground : PriceType == PriceType.Free ? _defaultBackground : _costBackground;
        }

        public void SetInfo(AnswerSolutionInfo answerInfo)
        {
            SetId(answerInfo.Id);
            LocalisationDictionary = answerInfo.LocalisationDictionary;
            LocalisationDictionary.TryAdd(LanguageSwitch.StartLanguage, "");

            InputField.text = LocalisationDictionary[LanguageSwitch.StartLanguage];
            _priceTypeDropdown.value = answerInfo.Free ? 1 : 0;
            ConnectionStartPoint.Id = answerInfo.NextMessageId;
        }


        public override void AddNextBlock(BlockObject blockObject)
        {
            NextBlocks.Clear();
            NextBlocks.Add(blockObject);
        }

        public override void AddPreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Clear();
            PreviousBlocks.Add(blockObject);
        }

        public override void RemoveNextBlock(BlockObject blockObject)
        {
            NextBlocks.Clear();
        }

        public override void RemovePreviousBlock(BlockObject blockObject)
        {
            PreviousBlocks.Clear();
        }

        public void Init(MessageConnectionFactory messageConnectionFactory, CommandsHandler commandsHandler)
        {
            MessageConnectionFactory = messageConnectionFactory;
            transform.SetAsFirstSibling();
            CommandsHandler = commandsHandler;

            LastText = InputField.text;
            InputField.onEndEdit.AddListener(OnInputFieldChanged);
        }

        public AnswerSolutionInfo GetAnswerSolutionInfo()
        {
            AnswerSolutionInfo answerSolutionInfo = new AnswerSolutionInfo
            {
                Id = Id,
                LocalisationDictionary = LocalisationDictionary,
                Free = _priceTypeDropdown.value == 1,
                NextMessageId = ConnectionStartPoint.Id
            };
            return answerSolutionInfo;
        }

        protected override void RemoveBlock()
        {
            DeleteDestroyableBlockCommand command = new DeleteDestroyableBlockCommand(this);
            command.Execute();
            CommandsHandler.AddCommand(command);
        }

        private void OnPriceTypeValueChanged(int priceType)
        {
            PriceTypeChanged?.Invoke();
        }
    }
}