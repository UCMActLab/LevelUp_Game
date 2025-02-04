using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class SpriteCreator
    {
        public static Sprite LoadSprite(string filePath, float pixelsPerUnit = 100.0f)
        {
            Texture2D spriteTexture = LoadTexture(filePath);

            Rect rect = new(0, 0, spriteTexture.width, spriteTexture.height);
            Sprite sprite = Sprite.Create(spriteTexture, rect, Vector2.zero, pixelsPerUnit);

            return sprite;
        }
        
        public static Sprite CreateSprite(Texture2D texture2D, float pixelsPerUnit = 100.0f)
        {
            Rect rect = new(0, 0, texture2D.width, texture2D.height);
            Sprite sprite = Sprite.Create(texture2D, rect, Vector2.zero, pixelsPerUnit);

            return sprite;
        }

        public static Texture2D LoadTexture(string filePath)
        {
            Texture2D texture2D;

            if (FileIO.TryReadBytes(filePath, out byte[] fileData))
            {
                texture2D = new Texture2D(2, 2);
                if (texture2D.LoadImage(fileData))
                    return texture2D;
            }

            return null;
        }
    }
}