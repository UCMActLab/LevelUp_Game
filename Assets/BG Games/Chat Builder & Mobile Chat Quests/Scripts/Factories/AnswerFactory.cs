using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories
{
    public class AnswerFactory : BlockFactory
    {
        [SerializeField] private AnswerBlock blockPrefab;
        [SerializeField] private MessageConnectionFactory _messageConnectionFactory;

        private List<TextHolderBlock> _allAnswers = new List<TextHolderBlock>();

        public AnswerBlock Create(Transform parent, Guid id = default)
        {
            AnswerBlock block = Instantiate(blockPrefab, parent);
            _allAnswers.Add(block);
            block.Init(_messageConnectionFactory, CommandsHandler);
            if (id == Guid.Empty)
                block.SetId(GuidUtils.GetGuidString());

            return block;
        }

        public override List<TextHolderBlock> GetTextBlocks()
        {
            return _allAnswers;
        }
    }
}