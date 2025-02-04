using System;
using System.Threading.Tasks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.PlayerPrefsSystem;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem
{
    public static class EditorFilePathSaverSystem
    {
        public static async Task<(bool, string)> TryAddFilePath(Action<string> onSucceeded = null)
        {
            var (success, filePath) = await RootPathLocator.TryGetChatEditorSavePath();
            if (!success || string.IsNullOrEmpty(filePath))
            {
                Debug.Log("Save operation canceled or file path invalid.");
                return (false, string.Empty);
            }

            EditorFilePathSaver.Save(filePath);
            onSucceeded?.Invoke(filePath);

            return (true, filePath);
        }
    }
}