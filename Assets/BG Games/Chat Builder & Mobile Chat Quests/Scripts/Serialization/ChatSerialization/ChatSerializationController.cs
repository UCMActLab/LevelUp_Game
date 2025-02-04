using System;
using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Dictionaries;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.ChatSerialization
{
    public class ChatSerializationController : SerializationController
    {
        [SerializeField] protected ChatBlocksContainer _chatBlockContainer;
        [SerializeField] private LanguageSwitch _languageSwitch;

        public override bool TrySerialize(out string[] serializationResult)
        {
            StartBlock startBlock = _chatBlockContainer.StartBlock;
            EndBlock endBlock = _chatBlockContainer.EndBlock;

            bool validationSucceeded = ChatToJsonValidator.Validate(startBlock, endBlock);
            if (!validationSucceeded)
            {
                serializationResult = null;
                return false;
            }
            List<ChatMessageStruct> messages = ChatToJsonSerializer.Serialize(startBlock, endBlock).ToList();

            ChatStruct chatStruct = new()
            {
                Id = _chatBlockContainer.GetChatId(),
                FirstMessagesIds = GetFirstIds(startBlock.NextBlocks[0]),
                LastMessagesIds = endBlock.PreviousBlocks.Select(block=>block.Id).ToArray(),
                Messages = messages.ToList()
            };

            serializationResult = _languageSwitch.LanguagesListSo.LanguageDataElements
                .Where(languageData => languageData.IsActive)
                .Select(languageData => 
                {
                    ChatStruct copy = CopyChatWithLanguage(chatStruct, languageData.LanguageData.Language);
                    ChatDictionary chatDictionary = new ChatDictionary(copy);
                    string json = JsonConvert.SerializeObject(chatDictionary, Formatting.Indented);

                    return json;
                }).ToArray();

            return true;
        }

        private ChatStruct CopyChatWithLanguage(ChatStruct dateStruct, LanguageType languageType)
        {
            ChatStruct copy = new()
            {
                Id = dateStruct.Id,
                FirstMessagesIds = dateStruct.FirstMessagesIds,
                LastMessagesIds = dateStruct.LastMessagesIds,
                Messages = dateStruct.Messages
                    .Select(message => CopyMessageWithLanguage(message, languageType))
                    .ToList()
            };

            return copy;
        }

        private ChatMessageStruct CopyMessageWithLanguage(ChatMessageStruct messageStruct, LanguageType languageType)
        {
            ChatMessageStruct copy = new()
            {
                Id = messageStruct.Id,
                ContentType = messageStruct.ContentType,
                NextMessages = messageStruct.NextMessages,
                PreviousMessages = messageStruct.PreviousMessages,
                UserType = messageStruct.UserType,
                PriceType = messageStruct.PriceType
            };

            if (messageStruct.ContentType == ContentType.Image)
            {
                copy.Content = messageStruct.Content;
            }
            else
            {
                copy.Content = messageStruct.ContentType == ContentType.Image
                    ? messageStruct.Content
                    :
                    (messageStruct.Content as Dictionary<LanguageType, string>)?.ContainsKey(languageType) == true
                        ?
                        (messageStruct.Content as Dictionary<LanguageType, string>)?[languageType]
                        : null;
            }

            return copy;
        }

        public override void SerializeEditorFiles(out string serializationResult, out string id)
        {
            Dictionary<string, ChatMessageBlock> messageBlocks = _chatBlockContainer.ActiveMessageBlocks;

            List<MessageSolutionInfo> messageSolutionInfos = new List<MessageSolutionInfo>();
            foreach (ChatMessageBlock messageBlock in messageBlocks.Values)
            {
                MessageSolutionInfo messageSolutionInfo = messageBlock.GetMessageSolutionInfo();
                messageSolutionInfos.Add(messageSolutionInfo);
            }

            id = _chatBlockContainer.HaveChatId() ? GuidUtils.GetGuidString() : _chatBlockContainer.GetChatId();
            _chatBlockContainer.SetChatId(id);

            ChatSolutionInfo chatSolutionInfo = new ChatSolutionInfo
            {
                ChatId = id,
                MessageSolutionInfos = messageSolutionInfos.ToArray(),
                StartPointLinkId = _chatBlockContainer.StartBlock.ConnectionStartPoint.Id,
                StartPosition = _chatBlockContainer.StartBlock.transform.position,
                EndPosition = _chatBlockContainer.EndBlock.transform.position
            };

            serializationResult = JsonConvert.SerializeObject(chatSolutionInfo, Formatting.Indented);
        }

        private string[] GetFirstIds(BlockObject block )
        {
            if(block is ChatMessageBlock messageBlock)
            {
                return messageBlock.HasContent ? new[] { messageBlock.Id } : messageBlock.NextBlocks.Select(answer => answer.Id).ToArray();
            }
            return null;
        }

        private Guid GetChatId()
        {
            return GuidUtils.GetGuid();
        }
    }
}