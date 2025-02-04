using System.IO;
using System.Linq;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Data;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.EditorChatData;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.ImageServices;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Utils;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.Utils
{
    public class ChatDataLoader
    {
        private const string ScriptableObjectSavePath = "Assets/BG Games/Chat Builder & Mobile Chat Quests/ChatDataObjects";

        public void LoadJsonAndCreateScriptableObject(string jsonContent,string fileName)
        {
            if (!string.IsNullOrEmpty(jsonContent))
            {
                ChatSolutionInfo chatDataJson = JsonConvert.DeserializeObject<ChatSolutionInfo>(jsonContent);
            
                ChatData chatData = SetupChatData(chatDataJson);
            
                if (!Directory.Exists(ScriptableObjectSavePath))
                    Directory.CreateDirectory(ScriptableObjectSavePath);
            
                SaveChatDataAsAsset(chatData, fileName);
            }
            else
            {
                Debug.LogError("File not selected or does not exist!");
            }
        }

        private void SaveChatDataAsAsset(ChatData chatData,string filePath)
        {
#if UNITY_EDITOR
        
            string fileName = Path.GetFileNameWithoutExtension(filePath).Replace(".chateditor", "");
            string assetPath = Path.Combine(ScriptableObjectSavePath, $"{fileName}.asset");
        
            AssetDatabase.CreateAsset(chatData, assetPath);
            AssetDatabase.SaveAssets();
#endif
        }
    
        private ChatData SetupChatData(ChatSolutionInfo chatDataJson)
        {
            ChatData chatData = ScriptableObject.CreateInstance<ChatData>();

            chatData.ChatId = chatDataJson.ChatId;
            chatData.StartPointLinkId = chatDataJson.StartPointLinkId;
        
        
            chatData.MessageSolutionInfos = chatDataJson.MessageSolutionInfos.Select(messageInfo =>
            {
                var messageSolution = new MessageSolution
                {
                    Id = messageInfo.Id,
                    BlurType = messageInfo.BlurType,
                    NextMessageId = messageInfo.NextMessageId,
                    IsText = messageInfo.IsText,
                    IsImage = messageInfo.IsImage,
                    ImageId = messageInfo.ImageId,
                    ImagePrice = messageInfo.ImagePrice,
                    ImageFormat = (int)messageInfo.ImageFormat,
                    AnswerInfos = messageInfo.AnswerInfos.Select(answerInfo => new AnswerInfo
                    {
                        Id = answerInfo.Id,
                        Free = answerInfo.Free,
                        NextMessageId = answerInfo.NextMessageId,

                        LocalisationDictionary = answerInfo.LocalisationDictionary.Select(entry => new LocalisationEntry 
                        {
                            Key = entry.Key,
                            Value = entry.Value
                        }).ToList() 
                    }).ToArray(),

                    LocalisationDictionary = messageInfo.LocalisationDictionary.Select(entry => new LocalisationEntry
                    {
                        Key = entry.Key,
                        Value = entry.Value
                    }).ToList(),
                    Texture2D = GetTexture(messageInfo.ImageId, messageInfo.ImageFormat)
                };

                return messageSolution;
            }).ToArray();
       
            return chatData;
        }
    
        private Texture2D GetTexture(string id, ImageFormat imageFormat)
        {
            Texture2D texture = new Texture2D(2, 2);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
            string fileFullPath = FileNamesCreator.CreateImageNameWithPath(FileNamesCreator.ImagesFolderName, id, imageFormat.ToString());
            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(fileFullPath);
#endif

            return texture;
        }
    }
}

