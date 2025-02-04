using System.IO;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class FileNamesCreator
    {
        public const string EditorFileExtension = ".chatbuilder";
        public const string ImagesFileExtension = "png";
        public const string ImagesFolderName = "Assets/BG Games/Chat Builder & Mobile Chat Quests/ChatDataObjects/Images";

        public static string CreateImageNameWithPath(string path, string name, string format)
        {
            string imageFullName = CreateImageName(name, format);

            return Path.Combine(path, imageFullName);
        }

        public static string CreateImageName(string name, string format)
        {
            return $"{name}.{format}";
        }

        public static string CreateChatEditorFileName(string name)
        {
            return $"{name}.{EditorFileExtension}";
        }

        public static string CreateDirectoryPath(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        public static string CreateJsonFileName(string fileName)
        {
            return $"{fileName}.json";
        }
    }
}