using System.ComponentModel;
using UnityEngine;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices
{
    public record CashedImage(string Id, Sprite Sprite, ImageFormat ImageFormat);
}