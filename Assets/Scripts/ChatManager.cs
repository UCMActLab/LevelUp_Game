using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;
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
    [SerializeField] private GameAssistant _assistant = null;
    [SerializeField] private ToDoMenu _topicMenu = null;

    private GameObject _currentChat;

    private GameObject _lastSharedArticle = null;

    private ArticleGameObject _currentArticle = null;

    [Header("Parameters")]
    [SerializeField, Range(0.0f, 5.0f)] private float _waitingBetweenMessages = 0.6f;
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
        _scrollRect.verticalNormalizedPosition = 1.0f;
        MessageView newMessage = Instantiate(_playerMessagePrefab, _currentChat.transform).GetComponent<MessageView>();

        // pop animation
        StartCoroutine(PopAnimation(newMessage.GetComponent<RectTransform>()));

        // TRADUCCIÓN
        newMessage.Setup("", "Translation", "ARTICLE/PLAYER_MESSAGE", articleData.isTrue);

        GameObject prefabToUse = _articlePrefab;
        if (articleData.convType == ConversationType.TUTORIAL) prefabToUse = _tutorialArticlePrefab;

        _lastSharedArticle = Instantiate(prefabToUse, _currentChat.transform);
        // pop animation
        StartCoroutine(PopAnimation(_lastSharedArticle.GetComponent<RectTransform>()));
        _currentArticle = _lastSharedArticle.GetComponent<ArticleGameObject>();
        articleData.articleBody = string.Empty;
        _currentArticle.SetArticleData(articleData);
        _currentArticle.DestroyButtons();

        LayoutRebuilder.ForceRebuildLayoutImmediate(_currentChat.GetComponent<RectTransform>());
        _scrollRect.verticalNormalizedPosition = 1.0f;
    }

    public GameObject SpawnShareButtons()
    {
        GameObject gO = Instantiate(_shareButtonsPrefab, _shareButtonsParent);
        foreach (RectTransform rect in gO.GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
        _shareButtonsParent.parent.gameObject.SetActive(true);
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
            if(_assistant != null) _assistant.HideMessage();
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
        _scrollRect.verticalNormalizedPosition = 1.0f;
        _messageWritingAnimator.transform.SetParent(_currentChat.transform);

        _keepSharingButton.SetActive(false);
        _keepSharingButton.transform.parent = _currentChat.transform.parent;
    }

    IEnumerator DisplayConversation()
    {
        while (_currentConversation != null && _currentConversation.CanContinue)
        {
            _scrollRect.verticalNormalizedPosition = 0.0f;

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


                MessageView newMessage = Instantiate(_messagePrefab, _currentChat.transform).GetComponent<MessageView>();
                if(!currentMessages.NeedsTranslation && _currentConversation.Type == ConversationType.NONE)
                {
                    newMessage.Setup(currentMessages.Name, currentMessages.GetNextMessage(), _currentArticle.Data.isTrue);
                }
                else
                {
                    newMessage.Setup(currentMessages.Name, messageTable, currentMessages.GetNextMessage(), _currentArticle.Data.isTrue);
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(_currentChat.GetComponent<RectTransform>());
                StartCoroutine(PopAnimation(newMessage.GetComponent<RectTransform>()));

                yield return new WaitForSeconds(0.2f);
                _chatScrollAnimation?.PlayAnimation();  
            }
        }

        if(_keepSharingButton != null)
        {
            yield return new WaitForSeconds(_waitingBetweenMessages);
            _chatScrollAnimation?.PlayAnimation();
            ActivateKeepSharingButtons();
        }
    }

    private void ActivateKeepSharingButtons()
    {
        ArticleGameObject article = _lastSharedArticle.GetComponent<ArticleGameObject>();
        bool[] sharedWithGroups = article.HasSharedWithGroups;

        bool canShare = false;
        // quitamos el canShare porque ahora hacemos que SOLO se puede compartir una vez
        //for (int i = 0; i < sharedWithGroups.Length; ++i)
        //{ 
        //    canShare = canShare || !sharedWithGroups[i];
        //}

        if(canShare)
        {
            _keepSharingButton.SetActive(true);
            _keepSharingButton.transform.parent = _currentChat.transform;

            Button[] buttons = _keepSharingButton?.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < sharedWithGroups.Length; ++i) {
                buttons[i].interactable = !sharedWithGroups[i];

                if (buttons[i].interactable)
                {
                    buttons[i].gameObject.SetActive(true);
                    article.AddShareArticleListenerToButton(i + 1, buttons[i]);
                }
            }
            for (int i = sharedWithGroups.Length; i < buttons.Length - 1; ++i)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        else
        {
            _backToChatButton.SetActive(true);
            _backToChatButton.transform.parent.parent.gameObject.SetActive(true);
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

    public void RandomizeAllGroupTopics(List<Topics> topicPool = null, int numGroups = 3)
    {
        // Creamos la "bolsa" con las 5 opciones.
        if (topicPool == null) topicPool = TopicsDictionary.topics.Values.ToList();
        List<Topics> copy = new List<Topics>(topicPool);

        int i = 0;
        foreach (GameObject groupObj in _groupChats)
        {
            if (i >= numGroups + 1) break;

            if (groupObj == null) continue;

            GroupSettings settings = groupObj.GetComponent<GroupSettings>();
            if (settings != null)
            {
                // El método de GroupSettings escoge uno
                settings.AssignRandomTopic(topicPool);
                _topicMenu.SetTopic(settings.Name, settings.Topic);
            }


            if (topicPool.Count <= 0) topicPool = new List<Topics>(copy);
            i++;
        }

        for (; i < numGroups + 1; ++i)
        {
            _topicMenu.HideToShareWith(_groupChats[i].GetComponent<GroupSettings>().Name);
        }
    }

    public Topics GetGroupTheme(int id)
    {
        return _groupChats[id].GetComponent<GroupSettings>().Topic;
    }

    // Pau
    IEnumerator PopAnimation(RectTransform target)
    {
        Vector3 originalScale = Vector3.one;
        target.localScale = new Vector3(0.8f, 0.8f, 1f); 

        float time = 0f;
        float duration = 0.15f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(0.8f, 1.1f, t);
            target.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        time = 0f;
        while (time < 0.1f)
        {
            time += Time.deltaTime;
            float t = time / 0.1f;
            float scale = Mathf.Lerp(1.1f, 1f, t);
            target.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        target.localScale = originalScale;
    }
}
