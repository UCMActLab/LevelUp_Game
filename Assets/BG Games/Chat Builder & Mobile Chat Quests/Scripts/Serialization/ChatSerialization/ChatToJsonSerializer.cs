using System.Collections.Generic;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.Structs.MessageStructs;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.ChatSerialization
{
    public static class ChatToJsonSerializer
    {
        public static LinkedList<ChatMessageStruct> Serialize(StartBlock startBlock, EndBlock endBlock)
        {
            LinkedList<ChatMessageStruct> result = SerializeRecursively(startBlock, endBlock);

            return result;
        }

        private static LinkedList<ChatMessageStruct> SerializeRecursively(StartBlock startBlock, EndBlock endBlock)
        {
            Dictionary<BlockObject, ChatMessageStruct> resultDictionary = new();
            BlockObject block = startBlock.NextBlocks[0];

            SerializeBlock(block, endBlock, resultDictionary);

            var serializationResult = CompleteMessageStructLinks(resultDictionary);
            return serializationResult;
        }

        private static void SerializeBlock(BlockObject currentBlock, EndBlock endBlock, Dictionary<BlockObject, ChatMessageStruct> result)
        {
            if (result.ContainsKey(currentBlock))
            {
                return;
            }

            foreach (BlockObject block in currentBlock.NextBlocks)
            {
                if (block == endBlock)
                {
                    break;
                }

                SerializeBlock(block, endBlock, result);
            }

            if (currentBlock is ChatMessageBlock { HasContent: false })
            {
                return;
            }
            
            ChatMessageStruct messageStruct = ConvertToMessageStruct(currentBlock);
            result.Add(currentBlock, messageStruct);
        }

        private static ChatMessageStruct ConvertToMessageStruct(BlockObject block)
        {
            UserType userType = block is AnswerBlock ? UserType.Player : UserType.Girl;

            PriceType priceType = PriceType.Free;
            if (block is AnswerBlock answerBlock)
            {
                priceType = answerBlock.PriceType;
            }

            ContentType contentType = ContentType.Text;
            int imagePrice = default;
            if (block is ChatMessageBlock messageBlock)
            {
                contentType = messageBlock.ContentType;
                imagePrice = messageBlock.Price;
            }

            object content = (block as TextHolderBlock).Content;
            if (contentType == ContentType.Text)
            {
                content = (block as TextHolderBlock).LocalisationDictionary;
            }

            ChatMessageStruct messageStruct = new()
            {
                Id = block.Id,
                UserType = userType,
                PriceType = priceType,
                ContentType = contentType,
                Content = content,
                ImagePrice = imagePrice 
            };

            return messageStruct;
        }

        private static LinkedList<ChatMessageStruct> CompleteMessageStructLinks(Dictionary<BlockObject, ChatMessageStruct> source)
        {
            LinkedList<ChatMessageStruct> messageStructs = new();

            foreach (var messagePair in source)
            {
                ChatMessageStruct messageStruct = messagePair.Value;

                if (messagePair.Key is ChatMessageBlock { HasContent: false })
                {
                    continue;
                }

                if (messagePair.Key.NextBlocks[0] is ChatMessageBlock { HasContent: false })
                {
                    BlockObject currentBlock = messagePair.Key.NextBlocks[0];
                    
                    messageStruct.NextMessages = currentBlock.NextBlocks
                        .Where(block => block is not EndBlock)
                        .Select(block => source[block].Id)
                        .ToList();

                    currentBlock.NextBlocks
                        .ForEach(message =>
                        {
                            if (message is EndBlock)
                            {
                                return;
                            }

                            source[message].PreviousMessages.Add(messageStruct.Id);
                        });
                }
                else
                {
                    messageStruct.NextMessages = messagePair.Key.NextBlocks
                        .Where(block => block is not EndBlock)
                        .Select(block => source[block].Id)
                        .ToList();

                    messagePair.Key.NextBlocks
                        .ForEach(message =>
                        {
                            if (message is EndBlock)
                            {
                                return;
                            }

                            source[message].PreviousMessages.Add(messageStruct.Id);
                        });
                }

                messageStructs.AddLast(messageStruct);
            }

            return messageStructs;
        }
    }
}