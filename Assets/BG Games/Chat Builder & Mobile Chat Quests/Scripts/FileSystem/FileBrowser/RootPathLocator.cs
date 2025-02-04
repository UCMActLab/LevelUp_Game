using System.Threading.Tasks;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using SimpleFileBrowser;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.FileSystem
{
    public static class RootPathLocator
    {
        public static async Task<(bool success, string filePath)> TryGetChatEditorLoadPathAsync()
        {
            TaskCompletionSource<(bool success, string filePath)> tcs = new();

            string[] chosenPaths;

            FileBrowser.SetFilters(false, FileNamesCreator.EditorFileExtension);
            FileBrowser.ShowLoadDialog(
                paths =>
                {
                    Debug.Log("success");
                    chosenPaths = paths;
                    tcs.SetResult((true, chosenPaths[0]));
                },
                () =>
                {
                    Debug.Log("canceled");
                    tcs.SetResult((false, ""));
                },
                FileBrowser.PickMode.Files,
                true,
                Application.dataPath,
                "",
                "Select Chat Builder File",
                "Import"
            );

            return await tcs.Task;
        }
        
        public static async Task<(bool success, string filePath)> TryGetImageLoadPath()
        {
            TaskCompletionSource<(bool success, string filePath)> tcs = new();

            string[] chosenPaths;

            FileBrowser.SetFilters(false, FileNamesCreator.ImagesFileExtension);
            FileBrowser.ShowLoadDialog(
                paths =>
                {
                    Debug.Log("success");
                    chosenPaths = paths;
                    tcs.SetResult((true, chosenPaths[0]));
                },
                () =>
                {
                    Debug.Log("canceled");
                    tcs.SetResult((false, ""));
                },
                FileBrowser.PickMode.Files,
                true,
                Application.dataPath,
                "",
                "Select PNG File",
                "Load"
            );

            return await tcs.Task;
        }

        public static async Task<(bool success, string filePath)> TryGetChatEditorSavePath()
        {
            TaskCompletionSource<(bool success, string filePath)> tcs = new();

            string[] chosenPaths;

            FileBrowser.SetFilters(false, FileNamesCreator.EditorFileExtension);
            FileBrowser.ShowSaveDialog(
                paths =>
                {
                    Debug.Log("success");
                    chosenPaths = paths;
                    tcs.SetResult((paths.Length > 0, chosenPaths[0]));
                },
                () =>
                {
                    Debug.Log("canceled");
                    tcs.SetResult((false, ""));
                },
                FileBrowser.PickMode.Files,
                true,
                Application.dataPath,
                "",
                "Select Chat Builder File",
                "Save"
            );

            return await tcs.Task;
        }
    }
}