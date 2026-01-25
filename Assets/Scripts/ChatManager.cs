using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Message References")]
    [SerializeField] private Conversation _currentConversation = null;
    [SerializeField] private MessageWritingAnimator _messageWritingAnimator = null;
    [SerializeField] private GameObject _messagePrefab = null;
    [SerializeField] private GameObject _playerMessagePrefab = null;
    [SerializeField] private GameObject _articlePrefab = null;
    [SerializeField] private GameObject _tutorialArticlePrefab = null;

    [Header("Share References")]
    [SerializeField] private GameObject _shareButtonsPrefab = null;

    [Header("Chat References")]
    [SerializeField] private List<GameObject> _groupChats = null;
    [SerializeField] private GameObject _mainChat = null;
    [SerializeField] private ScrollRect _scrollRect = null;
    [SerializeField] private Transform _shareButtonsParent = null;
    [SerializeField] private ChatScrollAnimation _chatScrollAnimation = null;
    [SerializeField] private GameObject _header = null;
    [SerializeField] private GameObject _keepSharingButton = null;
    [SerializeField] private GameObject _backToChatButton = null;

    private GameObject _currentChat;

    private GameObject _lastSharedArticle = null;

    [Header("Parameters")]
    [SerializeField, Range(0.0f, 5.0f)] private float _waitingBetweenMessages = 0.1f;
    [SerializeField] private bool _playOnAwake = false;

    IEnumerator _displayConvCoroutine = null;

    public UnityEvent<int> OnChatChanged = new UnityEvent<int>();

    private void StartConversation()
    {
        StopConversation();
        _displayConvCoroutine = DisplayConversation();
        StartCoroutine(_displayConvCoroutine);
    }

    private void StopConversation()
    {
        if (_displayConvCoroutine != null) { StopCoroutine(_displayConvCoroutine); }
        _messageWritingAnimator?.Disable();
    }

    public void SetConversation(Conversation conversation, bool startConversation)
    {
        _currentConversation = Instantiate(conversation);
        if(startConversation)
        {
            StartConversation();
        }
    }

    public void SendArticle(ArticleData articleData)
    {
        MessageView newMessage = Instantiate(_playerMessagePrefab, _currentChat.transform).GetComponent<MessageView>();
        // TRADUCCIÓN
        newMessage.Setup("", "Translation", "ARTICLE/PLAYER_MESSAGE");

        GameObject prefabToUse = _articlePrefab;
        if (articleData.convType == ConversationType.TUTORIAL) prefabToUse = _tutorialArticlePrefab;

        _lastSharedArticle = Instantiate(prefabToUse, _currentChat.transform);
        ArticleGameObject setter = _lastSharedArticle.GetComponent<ArticleGameObject>();
        articleData.articleBody = string.Empty;
        setter.SetArticleData(articleData);
        setter.DestroyButtons();
    }

    public GameObject SpawnShareButtons()
    {
        GameObject gO = Instantiate(_shareButtonsPrefab, _shareButtonsParent);
        foreach (RectTransform rect in gO.GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
        return gO;
    }

    public void ChangeGroup(int groupID)
    {
        _currentChat.SetActive(false);
        if (groupID < 0)
        {
            _currentChat.GetComponent<GroupSettings>().ActivateGroupInfo(false);
            _currentChat.SetActive(false);
            ChangeCurrentChat(_mainChat);
            if (_header != null) _header?.SetActive(true);
            StopConversation();
        }
        else
        {
            GameObject group = _groupChats[groupID];
            ChangeCurrentChat(group);
            _currentChat.GetComponent<GroupSettings>().ActivateGroupInfo(true);
            if(_header!=null) _header?.SetActive(false);
        }

        OnChatChanged.Invoke(groupID);
    }
    
    public void ChangeToMainChat()
    {
        if (_currentChat == _mainChat) return;
        ChangeGroup(-1);
    }

    private void ChangeCurrentChat(GameObject newChat)
    {
        _currentChat = newChat;
        _currentChat.SetActive(true);
        _scrollRect.content = _currentChat.transform as RectTransform;
        _messageWritingAnimator.transform.SetParent(_currentChat.transform);

        _keepSharingButton.SetActive(false);
        _keepSharingButton.transform.parent = _currentChat.transform.parent;
    }

    IEnumerator DisplayConversation()
    {
        while (_currentConversation != null && _currentConversation.CanContinue)
        {
            string messageTable;
            switch(_currentConversation.Type)
            {
                case ConversationType.REACTION_GOOD_ARTICLE:
                    messageTable = "POSITIVE_REACTIONS";
                    break;
                case ConversationType.REACTION_BAD_ARTICLE:
                    messageTable = "NEGATIVE_REACTIONS";
                    break;
                default:
                    messageTable = "Translation";
                    break;
            }
            Messages currentMessages = Instantiate(_currentConversation.GetNextMessages());
            while(currentMessages.CanContinue)
            {
                _messageWritingAnimator?.Enable();
                yield return new WaitForSeconds(_waitingBetweenMessages);
                _messageWritingAnimator?.Disable();

                _chatScrollAnimation?.PlayAnimation();

                MessageView newMessage = Instantiate(_messagePrefab, _currentChat.transform).GetComponent<MessageView>();
                if(!currentMessages.NeedsTranslation && _currentConversation.Type == ConversationType.NONE)
                {
                    newMessage.Setup(currentMessages.Name, currentMessages.GetNextMessage());
                }
                else
                {
                    newMessage.Setup(currentMessages.Name, messageTable, currentMessages.GetNextMessage());
                }
            }
        }

        if(_keepSharingButton != null)
        {
            ActivateKeepSharingButtons();
        }
    }

    private void ActivateKeepSharingButtons()
    {
        ArticleGameObject article = _lastSharedArticle.GetComponent<ArticleGameObject>();
        bool[] sharedWithGroups = article.HasSharedWithGroups;

        bool canShare = !(sharedWithGroups[0] && sharedWithGroups[1] && sharedWithGroups[2]);

        if(canShare)
        {
            _keepSharingButton.SetActive(true);
            _keepSharingButton.transform.parent = _currentChat.transform;

            Button[] buttons = _keepSharingButton?.GetComponentsInChildren<Button>();
            _keepSharingButton.SetActive(true);
            for (int i = 0; i < sharedWithGroups.Length; ++i) {
                buttons[i].interactable = !sharedWithGroups[i];

                if (buttons[i].interactable)
                {
                    article.AddShareArticleListenerToButton(i + 1, buttons[i]);
                }
            }
        }
        else
        {
            _backToChatButton.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeReferences();

        ChangeToMainChat();

        if (_playOnAwake) StartConversation();
    }

    private void InitializeReferences()
    {
        _currentChat = _mainChat;
        _messageWritingAnimator.transform.SetParent(_currentChat.transform);

        // hacemos una copia de la conversación para no modificar la original
        if(_currentConversation != null) _currentConversation = Instantiate(_currentConversation);
    }
}
