using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories
{
    public abstract class BlockFactory:MonoBehaviour
    {
        [SerializeField] protected CommandsHandler CommandsHandler;

        public abstract List<TextHolderBlock> GetTextBlocks();
        
        
    }
}