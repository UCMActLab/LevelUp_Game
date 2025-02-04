using System;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public class GuidUtils
    {
        public static string GetGuidString()
        {
            return Guid.NewGuid().ToString();
        }

        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}