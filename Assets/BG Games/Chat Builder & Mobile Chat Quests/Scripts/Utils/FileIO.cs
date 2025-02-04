using System;
using System.IO;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class FileIO
    {
        public static void CreateFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }
        
        public static void TryCreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void WriteBytes(string path, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save data to file {path}: {e}");
            }
        }

        public static bool TryReadBytes(string path, out byte[] result)
        {
            result = null;

            if (!File.Exists(path))
            {
                return false;
            }

            result = File.ReadAllBytes(path);
            return true;
        }

        public static void WriteText(string path, string text)
        {
            try
            {
                using FileStream stream = new(path, FileMode.Create);
                using StreamWriter writer = new(stream);

                writer.Write(text);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save data to file {path}: {e}");
            }
        }

        public static bool TryReadText(string path, out string result)
        {
            result = string.Empty;

            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                using FileStream stream = new FileStream(path, FileMode.Open);
                using StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to load data from file {path}: {e}");

                return false;
            }

            return true;
        }
    }
}