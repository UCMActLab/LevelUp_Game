using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization
{
    public abstract class SerializationController : MonoBehaviour
    {
        public abstract bool TrySerialize(out string[] serializationResult);
        public abstract void SerializeEditorFiles(out string serializationResult, out string id);
    }
}