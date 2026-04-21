using DA_Assets.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class ConversationCompendium : Singleton<ConversationCompendium>
{
    [SerializeField]
    Conversation[] _allAvaliableConversations;

    [SerializeField]
    VerificationFeedback[] _allAvaliableVerification;

    Dictionary<ConversationType, List<Conversation>> _conversations = new Dictionary<ConversationType, List<Conversation>>();
    List<List<LocalizedString>> _verifications = new List<List<LocalizedString>>();

    protected override void Awake()
    {
        _destroyOnLoad = true;
        base.Awake();
    }

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

        _verifications.Add(new List<LocalizedString>());
        _verifications.Add(new List<LocalizedString>());

        foreach(VerificationFeedback feedback in _allAvaliableVerification)
        {
            if (feedback.IsTrue)
            {
                _verifications[0].AddRange(feedback.Feedback.ToList());
            }
            else
            {
                _verifications[1].AddRange(feedback.Feedback.ToList());
            }
        }
    }

    public LocalizedString GetVerification(bool isTrue)
    {
        LocalizedString message = null;

        int index = 1;
        if (isTrue) index = 0;

        message = _verifications[index][Random.Range(0, _verifications[index].Count)];

        return message;
    }

    /// <summary>
    /// This erases a conversation from the list
    /// </summary>
    /// <returns> a Conversation of the given Type </returns>
    public Conversation GetConversation(int groupIndex, string source, string theme, ConversationType conversationType = ConversationType.NONE)
    {
        if(conversationType == ConversationType.REACTION_GOOD_ARTICLE ||
            conversationType == ConversationType.REACTION_BAD_ARTICLE)
        {
            Conversation cv = new Conversation();
            cv.Messages = new List<Messages>();
            cv.Type = conversationType;

            string veracity = conversationType == ConversationType.REACTION_BAD_ARTICLE ? "FALSE" : "TRUE";

            string table = conversationType == ConversationType.REACTION_BAD_ARTICLE ? "NEGATIVE_REACTIONS" : "POSITIVE_REACTIONS";

            //string key = "GROUP_" + groupIndex.ToString() + "_" + source.ToUpper() + "_" + theme.ToUpper() + "_" + veracity;

            //string stringValue = TranslationManager.Instance.GetLocalizedStringValue(table, key);

            //string[] messages = stringValue.Split("_");

            //foreach (string message in messages)
            //{
            //    Messages msg = new Messages();
            //    string[] split = message.Split(":");
            //    msg.Name = split[0];

            //    string aux = "";
            //    for (int i = 1; i < split.Length; ++i)
            //    {
            //        aux += split[i];
            //    }

            //    msg.MessageList = new List<string>();
            //    msg.MessageList.Add(aux);

            //    cv.Messages.Add(msg);
            //}

            var nameList = TranslationManager.Instance.GetAllTableEntries("NAMES");
            nameList.Shuffle();

            var messageList = TranslationManager.Instance.GetAllTableEntries(table);
            messageList.Shuffle();

            Queue<StringTableEntry> nameEntries = new Queue<StringTableEntry>(nameList);
            Queue<StringTableEntry> messageEntries = new Queue<StringTableEntry>(messageList);

            int n = Random.Range(2, 5);
            for (int i = 0; i < n; ++i)
            {
                Messages messages = new Messages();

                messages.Name = nameEntries.Dequeue().LocalizedValue;
                messages.MessageList = new List<string>();
                messages.MessageList.Add(messageEntries.Dequeue().LocalizedValue);

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
