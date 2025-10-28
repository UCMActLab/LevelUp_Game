using UnityEngine;
using System.Collections.Generic;
using System;

public enum ConversationType
{
    REACTION_GOOD_ARTICLE,
    REACTION_BAD_ARTICLE,
    TUTORIAL,

    // -------------------------------------
    // Añadir KEY_X para cada artículo clave
    // -------------------------------------

    NONE = -1
}

[CreateAssetMenu(fileName ="Conversation",menuName ="ScriptableObjects/Conversation/Conversation")]
[Serializable]
public class Conversation : ScriptableObject
{
    public ConversationType Type = ConversationType.NONE;

    public List<Messages> Messages;

    private int _currentMessage = 0;
    
    public bool CanContinue { get { return _currentMessage < Messages.Count; } }

    public Messages GetNextMessages()
    {
        return Messages[_currentMessage++];
    }
}
