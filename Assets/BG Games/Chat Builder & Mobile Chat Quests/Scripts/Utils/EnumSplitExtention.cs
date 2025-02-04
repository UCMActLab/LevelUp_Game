using System;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils
{
    public static class EnumSplitExtention 
    {
        public static string SplitWordsInEnumName<TEnum>(TEnum value) where TEnum : Enum
        {
            string enumName = value.ToString();
            string result = "";

            for (int i = 0; i < enumName.Length; i++)
            {
                char currentChar = enumName[i];
                if (i > 0)
                {
                    char previousChar = enumName[i - 1];

                    if (char.IsUpper(currentChar) && char.IsLower(previousChar))
                    {
                        result += " ";
                    }
                    else if (char.IsDigit(currentChar) && char.IsLetter(previousChar))
                    {
                        result += " ";
                    }
                    else if (char.IsLetter(currentChar) && char.IsDigit(previousChar))
                    {
                        result += " ";
                    }
                }
                result += currentChar;
            }

            return result;
        }
    
        public static string SplitWordsInEnumName(string enumName)
        {
            string result = "";

            for (int i = 0; i < enumName.Length; i++)
            {
                char currentChar = enumName[i];
                if (i > 0)
                {
                    char previousChar = enumName[i - 1];

                    if (char.IsUpper(currentChar) && char.IsLower(previousChar))
                    {
                        result += " ";
                    }
                    else if (char.IsDigit(currentChar) && char.IsLetter(previousChar))
                    {
                        result += " ";
                    }
                    else if (char.IsLetter(currentChar) && char.IsDigit(previousChar))
                    {
                        result += " ";
                    }
                }
                result += currentChar;
            }

            return result;
        }
    }
}