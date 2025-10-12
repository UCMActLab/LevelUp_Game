using System.Collections.Generic;
using UnityEngine;

public class ConversationCompendium : Singleton<ConversationCompendium>
{
    [SerializeField]
    Conversation[] _allAvaliableConversations;

    Dictionary<ConversationType, List<Conversation>> _conversations = new Dictionary<ConversationType, List<Conversation>>();

    private void Start()
    {
        foreach(Conversation cv in _allAvaliableConversations)
        {
            if(_conversations.TryGetValue(cv.Type, out List<Conversation> conversations))
            {
                conversations.Add(cv);
            }
            else
            {
                List<Conversation> list = new List<Conversation>();
                list.Add(cv);
                _conversations.Add(cv.Type, list);
            }
        }
    }

    /// <summary>
    /// This erases a conversation from the list
    /// </summary>
    /// <returns> a Conversation of the given Type </returns>
    public Conversation GetConversation(ConversationType conversationType = ConversationType.NONE)
    {
        if (_conversations.TryGetValue(conversationType, out List<Conversation> convs))
        {
            Conversation conv = convs[0];
            convs.RemoveAt(0);
            return conv;
        }
        else return null;
    }
}
