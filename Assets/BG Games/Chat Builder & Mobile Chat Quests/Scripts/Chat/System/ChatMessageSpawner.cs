using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class ChatMessageSpawner : MonoBehaviour
    {
        [SerializeField] private MessageView _messagePlayerViewPrefab;
        [SerializeField] private MessageView _messageInterlocutorViewPrefab;
        [SerializeField] private GameObject _articlePrefab;
        // [SerializeField] private Transform _parent;

        public event Action SpawnedMessage;
        
        public MessageView SpawnMessage(SenderType senderType, string name, string message, Transform chat)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if(chat != null)
            {
                if(message.Length > 0)
                {
                    var newMessage = Instantiate(prefab, chat);
                    newMessage.Setup(name, message);
            
                    SpawnedMessage?.Invoke();
            
                    return newMessage;
                }
            }
            return null;
        }
    
        public MessageView SpawnMessage(SenderType senderType, string name, Sprite sprite, Transform chat)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.Setup(name, sprite);

                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }

        public MessageView SpawnMessageV(SenderType senderType, string name, string video, Transform chat)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.SetupV(name, video);

                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }

        public MessageView SpawnMessage(SenderType senderType, string name, AudioClip audio, Transform chat)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                newMessage.Setup(name, audio);

                SpawnedMessage?.Invoke();

                return newMessage;
            }
            return null;
        }

        public MessageView SpawnMessage(SenderType senderType, ArticleData articleData, Transform chat)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            if (chat != null)
            {
                var newMessage = Instantiate(prefab, chat);
                GameObject article = Instantiate(_articlePrefab, newMessage.transform);
                ArticleDataSetter setter = article.GetComponent<ArticleDataSetter>();
                setter.SetArticleData(articleData);
                setter.DestroyButtons();

                newMessage.Setup("","Have you seen this article?");

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