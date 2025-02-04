using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Commands;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Factories;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Connections;
using UnityEngine;
using UnityEngine.UI;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.UIElements.Blocks
{
    public abstract class DestroyableBlock : BlockObject
    {
        [SerializeField] private Button _deleteButton;

        [SerializeField] private ConnectionStartPoint _connectionStartPoint;
        [SerializeField] private ConnectionEndPoint _connectionEndPoint;

        public ConnectionStartPoint ConnectionStartPoint => _connectionStartPoint;
        public ConnectionEndPoint ConnectionEndPoint => _connectionEndPoint;

        public event Action<DestroyableBlock> Enabled;
        public event Action<DestroyableBlock> Disabled;

        protected MessageConnectionFactory MessageConnectionFactory;
        protected CommandsHandler CommandsHandler;


        protected virtual void Start()
        {
            _connectionStartPoint.Init(MessageConnectionFactory);
            _deleteButton.onClick.AddListener(RemoveBlock);
        }

        protected abstract void RemoveBlock();

      

        public void OnDisabled() => Disabled?.Invoke(this);
        public void OnEnabled() => Enabled?.Invoke(this);
    }
}