using TMPro;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class EmojiUtils
    {
        public static bool IsOneEmoji(this string text)
        {
            int unicode = text[0];
            if (char.IsSurrogatePair(text, 0))
            {
                if (text.Length > 2)
                {
                    return false;
                }

                unicode = char.ConvertToUtf32(text, 0);
            }
            else if (text.Length > 1)
            {
                return false;
            }

            TMP_SpriteAsset spriteAsset = TMP_Settings.defaultSpriteAsset;

            foreach (TMP_SpriteAsset fallback in spriteAsset.fallbackSpriteAssets)
            {
                foreach (TMP_SpriteCharacter spriteChar in fallback.spriteCharacterTable)
                {
                    if (spriteChar.unicode == unicode)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}