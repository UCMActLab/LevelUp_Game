using System.IO;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Serialization
{
    public static class JsonSavingService
    {
        public static void SaveJsonFile(string json, string folderPath, string fileName)
        {
            string fullFileName = FileNamesCreator.CreateJsonFileName(fileName);
            string fullPath = Path.Combine(folderPath, fullFileName);

            FileIO.WriteText(fullPath, json);
        }

        public static void SaveJsonEditorFile(string json, string folderPath, string name)
        {
            string fullPath = Path.Combine(folderPath, name);
            FileIO.WriteText(fullPath, json);
        }
    }
}