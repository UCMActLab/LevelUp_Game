using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories
{
    public class MessageBlockFactory : BlockFactory
    {
        [SerializeField] private ChatMessageBlock blockPrefab;

        [SerializeField] private AnswerFactory _answerFactory;
        [SerializeField] private MessageConnectionFactory _messageConnectionFactory;
        [SerializeField] private ImageSavingService _imageSavingService;
        [SerializeField] private DroppedImageManager _droppedImageManager;


        public Action<ChatMessageBlock> CreatedMessageBlock;

        public List<ChatMessageBlock> AllMessages { get; } = new();

        public Dictionary<string, ChatMessageBlock> AvailableMessages { get; } = new();


        public ChatMessageBlock Create(string id = default)
        {
            ChatMessageBlock block = Instantiate(blockPrefab, transform);
            block.Init(_answerFactory, _messageConnectionFactory, CommandsHandler, _imageSavingService, _droppedImageManager);

            block.SetId(string.IsNullOrEmpty(id) ? GuidUtils.GetGuidString() : id);
            CreatedMessageBlock?.Invoke(block);
            return block;
        }

        public void AddAvailableMessage(DestroyableBlock block) =>
            AvailableMessages.Add(block.Id, block as ChatMessageBlock);

        public void RemoveAvailableMessage(DestroyableBlock block) => AvailableMessages.Remove(block.Id);

        public override List<TextHolderBlock> GetTextBlocks()
        {
            return new List<TextHolderBlock>(AvailableMessages.Values.ToList());
        }
    }
}