using System;
using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization.ChatSerialization
{
    public static class ChatToJsonValidator
    {
        public static event Action<BlockObject> ErrorOccurred;
        public static bool Validate(StartBlock startBlock, EndBlock endBlock)
        {
            bool validationResult = ValidateRecursively(startBlock, endBlock);
            return validationResult;
        }

        private static bool ValidateRecursively(StartBlock startBlock, EndBlock endBlock)
        {
            if (startBlock.NextBlocks.Count == 0)
            {
                return false;
            }

            Dictionary<BlockObject, bool> visitedWithResult = new();
            BlockObject block = startBlock.NextBlocks[0];

            return ValidateBlock(block, endBlock, visitedWithResult);
        }

        private static bool ValidateBlock(BlockObject currentBlock, EndBlock endBlock, Dictionary<BlockObject, bool> visitedWithResult)
        {
            if (visitedWithResult.ContainsKey(currentBlock))
            {
                return visitedWithResult[currentBlock];
            }

            if (currentBlock.NextBlocks.Count == 0)
            {
                visitedWithResult.Add(currentBlock, false);
                ErrorOccurred?.Invoke(currentBlock);
                return false;
                
            }

            bool currentBranchsIsCorrect = ValidateNextBlocks(currentBlock.NextBlocks, endBlock, visitedWithResult);
            
            visitedWithResult.Add(currentBlock, currentBranchsIsCorrect);
            return currentBranchsIsCorrect;
        }

        private static bool ValidateNextBlocks(List<BlockObject> nextBlocks, EndBlock endBlock, Dictionary<BlockObject, bool> visitedWithResult)
        {
            foreach (BlockObject block in nextBlocks)
            {
                if (block == endBlock)
                {
                    if (nextBlocks.Count > 1)
                    {
                        return false;
                    }

                    continue;
                }

                bool innerBranchIsCorrect = ValidateBlock(block, endBlock, visitedWithResult);
                if (!innerBranchIsCorrect)
                {
                    return false;
                }
            }

            return true;
        }
    }
}