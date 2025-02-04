using System.Collections.Generic;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class MessageContainer : MonoBehaviour
    {
        [SerializeField] private ChatMessageSpawner _messageSpawner;

        private List<MessageView> _messageViews = new();

        public void AddMessage(SenderType interlocutor, string messageText, bool isBlur=false)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, messageText, isBlur);
            _messageViews.Add(messageView);
        }
    
        public void AddMessage(SenderType interlocutor, Sprite messageSprite, int imagePrice, bool isBlur=false)
        {
            var messageView = _messageSpawner.SpawnMessage(interlocutor, messageSprite, isBlur, imagePrice);
            _messageViews.Add(messageView);
        }
    }
}
