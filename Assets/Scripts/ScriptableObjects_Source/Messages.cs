using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="Messages",menuName ="ScriptableObjects/Conversation/Messages")]
public class Messages : ScriptableObject
{
    [Header("Sender")]
    [SerializeField]
    private string _name;

    public string Name { get { return _name; } }

    [Header("Content")]
    [SerializeField]
    private List<string> _messages;

    private int _currentMessage = 0;

    public bool CanContinue { get { return _currentMessage < _messages.Count; } }

    public string GetNextMessage()
    {
        return _messages[_currentMessage++];
    }
}
