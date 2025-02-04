using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements
{
    public abstract class BlockObject : MonoBehaviour
    {
        [SerializeField] private string _id;
        public void SetId(string id) => _id = id;
        public string Id => _id;

        public List<BlockObject> NextBlocks { get; protected set; } = new();
        public List<BlockObject> PreviousBlocks { get; protected set; } = new();

        public abstract void AddNextBlock(BlockObject blockObject);
        public abstract void AddPreviousBlock(BlockObject blockObject);

        public abstract void RemoveNextBlock(BlockObject blockObject);
        public abstract void RemovePreviousBlock(BlockObject blockObject);
    }
}