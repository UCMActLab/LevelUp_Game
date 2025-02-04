using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class BlockUtils
    {
        public static List<AnswerBlock> GetAnswerBlocks(this List<BlockObject> blockObjects)
        {
            List<AnswerBlock> answerBlocks = new List<AnswerBlock>();
            foreach (var blockObject in blockObjects)
            {
                if (blockObject is AnswerBlock answerBlock)
                {
                    answerBlocks.Add(answerBlock);
                } 
            }

            return answerBlocks;

        }
    }
}