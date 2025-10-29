using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

public enum ArticleAction
{
    Read,
    Share,
    Skip,

    // this means article's lifetime can be ended
    None = -1
}

public class ArticleDataSetter : MonoBehaviour
{
    [Header("General Info")]
    [SerializeField] Image _companyLogo = null;
    [SerializeField] TextMeshProUGUI _companyText = null;
    [SerializeField] Image _articleImage = null;
    [SerializeField] TextMeshProUGUI _articleTitle = null;

    [Header("Body")]
    [SerializeField] GameObject _articleBody = null;
    [SerializeField] TextMeshProUGUI _bodyText = null;

    [Header("Buttons")]
    public Button _readButton = null;
    [SerializeField] Button _shareButton = null;
    [SerializeField] Button _skipButton = null;

    public ArticleData Data = null;
    public ArticleAction Action;

    public event Action<Choice> OnReadChoice;
    public event Action<Choice> OnSkipChoice;
    public event Action<Choice> OnShareChoice;
    public event Action<Choice> AnswerClicked;

    bool _isFirstShare = true;
    bool _hasReadArticle;
    public bool HasReadArticle {  get { return _hasReadArticle; } }

    [HideInInspector]
    public UnityEvent OnSkip;
    [HideInInspector]
    public UnityEvent OnRead;
    [HideInInspector]
    public UnityEvent OnShare;

    InkConversationManager _inkConvManager = null;
    ChatManager _convManager = null;

    ArticleFeed _articleFeed = null;

    private LocalizeStringEvent _company;
    private LocalizeStringEvent _title;
    private LocalizeStringEvent _body;

    bool[] _sharedWithGroups;

    GameObject _shareArticleButtons = null;
    public bool IsTrue { get { return Data.isTrue; } }

    private void Start()
    {
        _articleFeed = GetComponentInParent<ArticleFeed>();

        _sharedWithGroups = new bool[3] { false, false, false };

        _inkConvManager = FindAnyObjectByType<InkConversationManager>();
        _convManager = FindAnyObjectByType<ChatManager>();

        _articleBody.SetActive(false);

        _company = _companyText.GetComponent<LocalizeStringEvent>();
        _title = _articleTitle.GetComponent<LocalizeStringEvent>();
        _body = _bodyText.GetComponent<LocalizeStringEvent>();

        SetArticleData(Instantiate(Data));
    }

    public string GetBodyString()
    {
        return _articleBody.GetComponentInChildren<TextMeshProUGUI>().text;
    }


    #region Article Actions
    public void ShareButtonsSetUp()
    {
        _shareArticleButtons = _convManager.SpawnShareButtons();
        Button[] buttons = _shareArticleButtons.GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(() =>
        {
            _convManager.ChangeToMainChat();
            LevelManager.Instance.ShowNextArticle();
            // EnableButtonsInteraction(false);
            
            Destroy(_shareArticleButtons);
            _shareArticleButtons = null;
        });

        for (int i = 1; i <  buttons.Length; i++) {
            Button bt = buttons[i];
            
            bt.transform.parent.gameObject.SetActive(!_sharedWithGroups[i - 1]);

            int tempInt = i;
            Conversation conversation = null;
            if(Data.conversation != null && Data.conversation.Count > i - 1) { conversation = Data.conversation[i - 1]; }
            bt.onClick.AddListener(() => ShareArticle(tempInt, _shareArticleButtons, conversation));
        }

        OnShare.Invoke();
    }

    /// <summary>
    /// TODO: Skip article, changing points and general player score if skipped article was false or true
    /// </summary>
    bool _skipped = false;
    public void SkipArticle()
    {
        if (_skipped) return;

        GetComponent<Animator>().SetTrigger("Skip");
        _skipped = true;
    }

    public void InvokeOnSkip()
    {
        OnSkip.Invoke();
    }

    private void ShareArticle(int groupID, GameObject shareButtons, Conversation conv = null)
    {
        if(Data.convType != ConversationType.TUTORIAL)
        {
            if(_isFirstShare && !Data.isTrue)
            {
                ScoreManager.Instance.SharedFalseArticle(_hasReadArticle);
            }
            else if(Data.isTrue && !_hasReadArticle)
            {
                ScoreManager.Instance.SharedUnreadArticle(Data.isTrue);
            }
        }
        _convManager.ChangeGroup(groupID);
        // Data should be an instance
        _convManager.SendArticle(Data);

        if(conv == null)
        {
            conv = ConversationCompendium.Instance.GetConversation(Data.convType);
        }

        _convManager.SetConversation(conv, true);

        Destroy(shareButtons);

        _sharedWithGroups[groupID - 1] = true;

        _isFirstShare = false;
    }

    public void ReadArticle()
    {
        _articleBody.SetActive(true);
        _readButton.interactable = false;

        _hasReadArticle = true;

        OnRead.Invoke();
    }
    #endregion

    #region Activate or Destroy Buttons
    public void DestroyButtons()
    {
        Destroy(_readButton.gameObject);
        Destroy(_skipButton.gameObject);
        Destroy(_shareButton.gameObject);
    }

    public void ActivateButtons(bool active)
    {
        _readButton.gameObject.SetActive(active);
        _skipButton.gameObject.SetActive(active);
        _shareButton.gameObject.SetActive(active);

        RebuildAllLayouts();
    }
    #endregion

    #region Button Interaction 
    public void EnableButtonsInteraction(bool active)
    {
        EnableSkipButton(active);
        EnableReadButton(active);
        EnableShareButton(active);
    }

    public void EnableReadButton(bool active)
    {
        EnableButtonInteraction(_readButton, active);
    }
    public void EnableShareButton(bool active)
    {
        EnableButtonInteraction(_shareButton, active);
    }
    public void EnableSkipButton(bool active)
    {
        EnableButtonInteraction(_skipButton, active);
    }

    private void EnableButtonInteraction(Button bt, bool active)
    {
        bt.interactable = active;
    }
    #endregion

    #region Button Highlight
    public void HighlightSkipButton(bool active)
    {
        HighlightButton(_skipButton, active);
    }

    public void HighlightReadButton(bool active)
    {
        HighlightButton(_readButton, active);
    }

    public void HighlightShareButton(bool active)
    {
        HighlightButton(_shareButton, active);
    }

    private void HighlightButton(Button bt, bool active)
    {
        bt.GetComponent<Animator>().SetBool("Highlighted", active);
    }


    #endregion

    private void RebuildAllLayouts()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        foreach (Transform tr in transform.GetComponentsInChildren<Transform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tr as RectTransform);
        }
    }

    /// <summary>
    /// Sets the ArticleData and changes the Article's title, image, etc.
    /// 
    /// WARNING: The ArticleData received by this method SHOULD BE AN INSTANCE, not the original ScriptableObject,
    /// because it may be modified by other methods.
    /// </summary>
    /// <param name="data"></param>
    public void SetArticleData(ArticleData data)
    {
        if (data == null) return;

        Data = data;
        
        _companyLogo.sprite = Data.companyLogo;
        _articleImage.sprite = Data.articleImage;
        _articleImage.gameObject.SetActive(_articleImage.sprite != null);

        if (_company != null) _company.StringReference.SetReference("Translation", Data.companyName);
        else _companyText.text = Data.companyName;

        if (_title != null) _title.StringReference.SetReference("Translation", Data.articleTitle);
        else _articleTitle.text = Data.articleTitle;

        if (_body != null) _body.StringReference.SetReference("Translation", Data.articleBody);
        else _bodyText.text = Data.articleBody;
    }

    public void DestroyArticle()
    {
        if(_shareArticleButtons != null) Destroy(_shareArticleButtons);
        Destroy(gameObject);
    }

    #region Tutorial
    public void RemoveListenersFromButtons()
    {
        _skipButton.onClick.RemoveAllListeners();
        _skipButton.onClick.AddListener(() => OnSkip.Invoke());

        _readButton.onClick.RemoveAllListeners();
        _readButton.onClick.AddListener(() => OnRead.Invoke());

        _shareButton.onClick.RemoveAllListeners();
        _shareButton.onClick.AddListener(() => OnShare.Invoke());
    }

    public void SetUpButtons()
    {
        _skipButton.onClick.AddListener(() => _articleFeed.SkipArticle(this));
        _readButton.onClick.AddListener(ReadArticle);
        _shareButton.onClick.AddListener(ShareButtonsSetUp);
    }
    #endregion
    #region INK
    public void ChangeButtonsOnArticleRead()
    {
        _readButton.interactable = false;

        if (_inkConvManager.story.currentChoices.Count > 0)
        {
            Choice skip = _inkConvManager.story.currentChoices[3];

            _skipButton.onClick.RemoveAllListeners();
            _shareButton.onClick.RemoveAllListeners();

            _skipButton.onClick.AddListener(() =>
                SkipArticle(skip)
            );
            _shareButton.onClick.AddListener(() =>
            {
                ShareButton(_inkConvManager.story.currentChoices);
                _shareButton.interactable = false;
                Action = ArticleAction.Share;
            }
            );
        }
    }
    private void SkipArticle(Choice skip)
    {
        this.Action = ArticleAction.None;
        _skipButton.interactable = false;
        _skipButton.onClick.RemoveAllListeners();
        _readButton.interactable = false;
        _readButton.onClick.RemoveAllListeners();
        _shareButton.interactable = false;
        _shareButton.onClick.RemoveAllListeners();

        OnSkipChoice?.Invoke(skip);
    }
    public void ShareButton(System.Collections.Generic.List<Choice> choices)
    {
        if (choices.Count == 1)
        {
            // escogemos automáticamente no enviar más artículos si no quedan grupos
            GameObject share = _inkConvManager.SpawnShareButtons();
            Button[] buttons = share.GetComponentsInChildren<Button>();
            foreach (Button bt in buttons)
            {
                bt.transform.parent.gameObject.SetActive(false);
            }
            buttons[0].transform.parent.gameObject.SetActive(true);
            buttons[0].onClick.AddListener(() =>
            {
                _inkConvManager.ChangeGroup(null);
                OnShareChoice.Invoke(choices[0]);
                Destroy(share);
                _readButton.interactable = false;
                _skipButton.interactable = false;
                Action = ArticleAction.None;
            });
        }
        else
        {
            GameObject share = _inkConvManager.SpawnShareButtons();
            Button[] buttons = share.GetComponentsInChildren<Button>();

            buttons[0].onClick.AddListener(() => {
                _inkConvManager.ChangeGroup(null);
                OnShareChoice.Invoke(choices[choices.Count - 1]);
                Destroy(share);
                _readButton.interactable = false;
                _skipButton.interactable = false;
                Action = ArticleAction.None;
            });

            string[] words = choices[0].text.Split(' ');
            string text = words[words.Length - 1].Trim('.').ToUpper();
            buttons[1].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
            buttons[1].onClick.AddListener(() => {
                _inkConvManager.ChangeGroup(choices[0]);
                _inkConvManager.SendArticle(Data);
                OnShareChoice.Invoke(choices[0]);
                Destroy(share);
                _readButton.interactable = false;
                _skipButton.interactable = false;
            });

            if (choices.Count > 2)
            {
                words = choices[1].text.Split(' ');
                text = words[words.Length - 1].Trim('.').ToUpper();
                buttons[2].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
                buttons[2].onClick.AddListener(() => {
                    _inkConvManager.ChangeGroup(choices[1]);
                    _inkConvManager.SendArticle(Data);
                    OnShareChoice.Invoke(choices[1]);
                    Destroy(share);
                    _readButton.interactable = false;
                    _skipButton.interactable = false;
                });
            }
            else buttons[2].transform.parent.gameObject.SetActive(false);

            if (choices.Count > 3)
            {
                words = choices[2].text.Split(' ');
                text = words[words.Length - 1].Trim('.').ToUpper();
                buttons[3].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
                buttons[3].onClick.AddListener(() => {
                    _inkConvManager.ChangeGroup(choices[2]);
                    _inkConvManager.SendArticle(Data);
                    OnShareChoice.Invoke(choices[2]);
                    Destroy(share);
                    _readButton.interactable = false;
                    _skipButton.interactable = false;
                });
            }
            else buttons[3].transform.parent.gameObject.SetActive(false);
        }
    }
    public void SetUpButtons(Choice read, Choice skip)
    {
        _readButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Read;
            OnReadChoice?.Invoke(read);
        });
        _skipButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Skip;
            OnSkipChoice?.Invoke(skip);
        });
        _shareButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Share;
            OnShareChoice?.Invoke(skip);
        });
    }

    #endregion
}
