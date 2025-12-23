using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using System.Collections;
using Unity.Services.Analytics;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.View;

public class ArticleGameObject : MonoBehaviour
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
    [SerializeField] Button _verifyButton = null;

    [Header("Feedback")]
    [SerializeField] GameObject _verificationFeedbackGO = null;
    [SerializeField] LocalizeStringEvent _verificationTextLocalized = null;

    [Header("Data")]
    public ArticleData Data = null;

    public event Action<Choice> OnReadChoice;
    public event Action<Choice> OnSkipChoice;
    public event Action<Choice> OnShareChoice;
    public event Action<Choice> AnswerClicked;

    bool _hasReadArticle;
    public bool HasReadArticle {  get { return _hasReadArticle; } }

    bool _hasSharedArticle;
    public bool HasSharedArticle { get { return _hasSharedArticle; } }

    [HideInInspector]
    public UnityEvent OnSkip;
    [HideInInspector]
    public UnityEvent OnRead;
    [HideInInspector]
    public UnityEvent OnShare;

    // InkConversationManager _inkConvManager = null;
    ChatManager _convManager = null;

    ArticleFeed _articleFeed = null;

    private LocalizeStringEvent _company;
    private LocalizeStringEvent _title;
    private LocalizeStringEvent _body;

    bool[] _sharedWithGroups;

    GameObject _shareArticleButtons = null;
    public bool IsTrue { get { return Data.isTrue; } }

    public bool HasSharedWithAllGroups { get
        {
            bool sharedAll = true;
            foreach (bool b in _sharedWithGroups)
            {
                sharedAll &= b;
                if (!sharedAll) break;
            }

            return sharedAll;
        } 
    }

    private void Start()
    {
        _articleFeed = GetComponentInParent<ArticleFeed>();

        _sharedWithGroups = new bool[3] { false, false, false };

        // _inkConvManager = FindAnyObjectByType<InkConversationManager>();
        _convManager = FindAnyObjectByType<ChatManager>();

        _articleBody.SetActive(false);

        _company = _companyText.GetComponent<LocalizeStringEvent>();
        _title = _articleTitle.GetComponent<LocalizeStringEvent>();
        _body = _bodyText.GetComponent<LocalizeStringEvent>();

        if (Data != null)
        {
            SetArticleData(Instantiate(Data));
        }
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
            // LevelManager.Instance.ShowNextArticle();
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

        StartCoroutine(SkipArticle_Wait());
    }

    IEnumerator SkipArticle_Wait()
    {
        yield return new WaitForSeconds(1.25f);

        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CalculateArticlePoints(this);
        }

        CustomEvent newEvent = new CustomEvent("Skip_Action")
        {
            {"IsTrue", Data.isTrue },
            {"NewsID", Data.ID }
        };
        AnalyticsManager.Instance.SubmitEvent(newEvent);

        InvokeOnSkip();
    }

    public void InvokeOnSkip()
    {
        OnSkip.Invoke();
    }

    private void ShareArticle(int groupID, GameObject shareButtons, Conversation conv = null)
    {
        CustomEvent newEvent = new CustomEvent("Share_Action")
        {
            {"ToWhom", groupID },
            {"IsTrue", Data.isTrue },
            {"NewsID", Data.ID }
        };
        AnalyticsManager.Instance.SubmitEvent(newEvent);

        _convManager.ChangeGroup(groupID);
        // Data should be an instance
        _convManager.SendArticle(Instantiate(Data));

        if(conv == null)
        {
            conv = ConversationCompendium.Instance.GetConversation(Data.convType);
        }

        _convManager.SetConversation(conv, true);

        Destroy(shareButtons);

        _sharedWithGroups[groupID - 1] = true;

        _hasSharedArticle = true;
    }

    public void ReadArticle()
    {
        ChatScrollAnimation anim = GameObject.FindAnyObjectByType<ChatScrollAnimation>();

        _articleBody.SetActive(true);
        _readButton.interactable = false;

        anim.PlayAnimation(0.75f);

        _hasReadArticle = true;

        RebuildAllLayouts();

        OnRead.Invoke();

        CustomEvent newEvent = new CustomEvent("Read_Action")
        {
            {"IsTrue", Data.isTrue },
            {"NewsID", Data.ID }
        };
        AnalyticsManager.Instance.SubmitEvent(newEvent);
    }

    public void VerifyArticle()
    {
        // add text to verification feedback
        _verificationTextLocalized.StringReference = ConversationCompendium.Instance.GetVerification(IsTrue);

        // activate feedback prefab
        _verificationFeedbackGO.SetActive(true);

        ChatScrollAnimation anim = FindAnyObjectByType<ChatScrollAnimation>();
        if(anim != null) anim.PlayAnimation(1.0f);

        CustomEvent newEvent = new CustomEvent("Verify_Action")
        {
            {"IsTrue", Data.isTrue },
            {"NewsID", Data.ID }
        };
        AnalyticsManager.Instance.SubmitEvent(newEvent);
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

    private void ActivateButton(Button bt, bool active)
    {
        bt.gameObject.SetActive(active);
        RebuildAllLayouts();
    }

    public void ActivateReadButton(bool active)
    {
        ActivateButton(_readButton, active);
    }

    public void ActivateSkipButton(bool active)
    {
        ActivateButton(_skipButton, active);
    }

    public void ActivateShareButton(bool active)
    {
        ActivateButton(_shareButton, active);
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

        if(_company == null) _company = _companyText.GetComponent<LocalizeStringEvent>();
        _company.StringReference.SetReference("Translation", "SOURCE/" + Data.companyName.ToUpper());

        if (Data.needsTranslation)
        {
            if(_title == null)
            {
                _title = _articleTitle.GetComponent<LocalizeStringEvent>();
            }
            if(_body == null)
            {
                _body = _bodyText.GetComponent<LocalizeStringEvent>();
            }
            _title.StringReference.SetReference("Translation", Data.articleTitle);
            _body.StringReference.SetReference("Translation", Data.articleBody);
        }
        else { 
            _articleTitle.text = Data.articleTitle;
            _bodyText.text = Data.articleBody;
        } 
        
        if(Data.articleBody == string.Empty)
        {
            ActivateReadButton(false);
        }

        RebuildAllLayouts();
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
}
