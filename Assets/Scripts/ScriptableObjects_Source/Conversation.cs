using UnityEngine;
using System.Collections.Generic;

public enum ConversationType
{
    REACTION_GOOD_ARTICLE,
    REACTION_BAD_ARTICLE,
    TUTORIAL,

    NONE = -1
}

[CreateAssetMenu(fileName ="Conversation",menuName ="ScriptableObjects/Conversation/Conversation")]
public class Conversation : ScriptableObject
{
    public ConversationType Type = ConversationType.NONE;

    [SerializeField]
    List<Messages> _messages;

    private int _currentMessage = 0;
    
    public bool CanContinue { get { return _currentMessage < _messages.Count; } }

    public Messages GetNextMessages()
    {
        return _messages[_currentMessage++];
    }
}
