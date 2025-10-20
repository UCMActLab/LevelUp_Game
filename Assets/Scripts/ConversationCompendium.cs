using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

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
        if(conversationType == ConversationType.REACTION_GOOD_ARTICLE ||
            conversationType == ConversationType.REACTION_BAD_ARTICLE)
        {
            Conversation cv = new Conversation();
            cv.Messages = new List<Messages>();
            cv.Type = conversationType;

            string table = conversationType == ConversationType.REACTION_BAD_ARTICLE ? "NEGATIVE_REACTIONS" : "POSITIVE_REACTIONS";
            
            int n = Random.Range(2, 5);
            for (int i = 0; i < n; ++i)
            {
                Messages messages = new Messages();

                messages.Name = TranslationManager.Instance.GetRandomEntryKey("NAMES");
                messages.MessageList = new List<string>();
                messages.MessageList.Add(TranslationManager.Instance.GetRandomEntryKey(table));

                cv.Messages.Add(messages);
            }

            return cv;
        }
        else if (_conversations.TryGetValue(conversationType, out List<Conversation> convs))
        {
            Conversation conv = convs[0];
            convs.RemoveAt(0);
            return conv;
        }
        else return null;
    }
}
