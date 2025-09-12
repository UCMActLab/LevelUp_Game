using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using UnityEngine;
using UnityEngine.Video;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class MessageContainer : MonoBehaviour
    {
        [SerializeField] private ChatMessageSpawner _messageSpawner;

        private List<MessageView> _messageViews = new();

        public void AddMessage(SenderType interlocutor, string name, string messageText, Transform chat, Sprite sprite = null)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, name, messageText, chat, sprite);
            _messageViews.Add(messageView);
        }
    
        public void AddMessage(SenderType interlocutor, string name, Sprite messageSprite, Transform chat, Sprite sprite = null)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, name, messageSprite, chat, sprite);
            _messageViews.Add(messageView);
        }

        public void AddMessageV(SenderType interlocutor, string name, string messageVideo, Transform chat, Sprite sprite = null)
        {
            var messageView = _messageSpawner.SpawnMessageV(interlocutor, name, messageVideo, chat, sprite);
            _messageViews.Add(messageView);
        }

        public void AddMessage(SenderType interlocutor, string name, AudioClip messageAudio, Transform chat, Sprite sprite = null)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, name, messageAudio, chat, sprite);
            _messageViews.Add(messageView);
        }

        public void SendArticle(ArticleData articleData, Transform chat)
        {
            _messageSpawner.SpawnMessage(SenderType.Player, articleData, chat);
        }
    }
}
