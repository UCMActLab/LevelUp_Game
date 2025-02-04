using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System
{
    public class ChatMessageSpawner : MonoBehaviour
    {
        [SerializeField] private CurrencyService _currencyService;
        [SerializeField] private MessageView _messagePlayerViewPrefab;
        [SerializeField] private MessageView _messageInterlocutorViewPrefab;
        [SerializeField] private Transform _parent;

        public event Action SpawnedMessage;
        
        public MessageView SpawnMessage(SenderType senderType, string message, bool isBlur)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            var newMessage = Instantiate(prefab, _parent);
            newMessage.Setup(message, isBlur);
            
            SpawnedMessage?.Invoke();
            
            return newMessage;
        }
    
        public MessageView SpawnMessage(SenderType senderType, Sprite sprite, bool isBlur, int imagePrice)
        {
            var prefab = senderType == SenderType.Interlocutor ? _messageInterlocutorViewPrefab : _messagePlayerViewPrefab;
            var newMessage = Instantiate(prefab, _parent);
            newMessage.Setup(sprite, _currencyService, isBlur, imagePrice);
            
            SpawnedMessage?.Invoke();
            
            return newMessage;
        }
    }

    public enum SenderType
    {
        Player = 0,
        Interlocutor = 1
    }
}