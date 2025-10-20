using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="Messages",menuName ="ScriptableObjects/Conversation/Messages")]
public class Messages : ScriptableObject
{
    [Header("Sender")]
    [SerializeField]
    private string _name;

    public string Name { get { return _name; } set { _name = value; } }

    [Header("Content")]
    public List<string> MessageList;

    private int _currentMessage = 0;

    public bool CanContinue { get { return _currentMessage < MessageList.Count; } }

    public string GetNextMessage()
    {
        return MessageList[_currentMessage++];
    }
}
