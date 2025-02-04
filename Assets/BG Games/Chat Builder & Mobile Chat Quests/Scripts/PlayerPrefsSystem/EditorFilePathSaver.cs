using System.IO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem
{
    public static class EditorFilePathSaver
    {
        public static void Save(string filePath)
        {
            string key = PlayerPrefsKeysCreator.CreateEditorFilePathKey();
            PlayerPrefs.SetString(key, filePath);
        }

        public static bool TryGetPath(out string filePath)
        {
            string key = PlayerPrefsKeysCreator.CreateEditorFilePathKey();

            if (!PlayerPrefs.HasKey(key))
            {
                filePath = string.Empty;
                return false;
            }

            filePath = PlayerPrefs.GetString(key);
            return true;
        }

        public static bool TryLoadDirectoryPath(out string directoryPath)
        {
            if (!TryGetPath(out string filePath))
            {
                directoryPath = string.Empty;
                return false;
            }

            directoryPath = Path.GetDirectoryName(filePath);
            return true;
        }
    }
}