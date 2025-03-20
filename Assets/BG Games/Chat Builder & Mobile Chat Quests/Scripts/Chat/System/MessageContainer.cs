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

        public void AddMessage(SenderType interlocutor, string messageText)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, messageText);
            _messageViews.Add(messageView);
        }
    
        public void AddMessage(SenderType interlocutor, Sprite messageSprite)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, messageSprite);
            _messageViews.Add(messageView);
        }

        public void AddMessage(SenderType interlocutor, VideoClip messageVideo)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, messageVideo);
            _messageViews.Add(messageView);
        }

        public void AddMessage(SenderType interlocutor, AudioClip messageAudio)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, messageAudio);
            _messageViews.Add(messageView);
        }
    }
}
