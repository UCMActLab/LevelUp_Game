using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class ChatMessageSpawner : MonoBehaviour
    {
        [SerializeField] private MessageView _messagePlayerViewPrefab;
        [SerializeField] private MessageView _messageInterlocutorViewPrefab;
        [SerializeField] private GameObject _articlePrefab;
        // [SerializeField] private Transform _parent;

        private Dictionary<Language, string> HAVEYOUSEENTHIS_KEYWORD;

        private void Start()
        {
            HAVEYOUSEENTHIS_KEYWORD = new Dictionary<Language, string>();
            HAVEYOUSEENTHIS_KEYWORD.Add(Language.spanish, "¿Habéis visto esto?");
            HAVEYOUSEENTHIS_KEYWORD.Add(Language.english, "Have you seen this?");
        }


        public event Action SpawnedMessage;

        public MessageView SpawnMessage(SenderType senderType, string name, string message, Transform chat, Sprite sprite = null)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if(chat != null)
            {
                if(message.Length > 0)
                {
                    var newMessage = Instantiate(prefab, chat);
                    newMessage.Setup(name, message);
                    if(sprite != null) newMessage.GetComponentInChildren<Image>().sprite = sprite;

                    SpawnedMessage?.Invoke();
            
                    return newMessage;
                }
            }
            return null;
        }
    
        public MessageView SpawnMessage(SenderType senderType, string name, Sprite sprite, Transform chat, Sprite speakerSprite = null)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.Setup(name, sprite);
                if (speakerSprite != null) newMessage.GetComponentInChildren<Image>().sprite = speakerSprite; 
                
                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }

        public MessageView SpawnMessageV(SenderType senderType, string name, string video, Transform chat, Sprite sprite = null)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.SetupV(name, video);
                if (sprite != null) newMessage.GetComponentInChildren<Image>().sprite = sprite;

                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }

        public MessageView SpawnMessage(SenderType senderType, string name, AudioClip audio, Transform chat, Sprite sprite = null)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.Setup(name, audio);
                if (sprite != null) newMessage.GetComponentInChildren<Image>().sprite = sprite;

                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }

        public MessageView SpawnMessage(SenderType senderType, ArticleData articleData, Transform chat, Sprite sprite = null)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.Setup("", HAVEYOUSEENTHIS_KEYWORD[LanguageSelection.chosenLanguage]);

                GameObject article = Instantiate(_articlePrefab, newMessage.transform);
                ArticleDataSetter setter = article.GetComponent<ArticleDataSetter>();
                articleData.articleBody = string.Empty;
                setter.SetArticleData(articleData);
                setter.DestroyButtons();

                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }
    }

    public enum SenderType
    {
        Player = 0,
        Interlocutor = 1
    }
}