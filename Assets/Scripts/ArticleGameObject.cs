using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Button _increaseTextButton = null;

    [Header("Feedback")]
    [SerializeField] GameObject _verificationFeedbackGO = null;
    [SerializeField] LocalizeStringEvent _verificationTextLocalized = null;

    [Header("Data")]
    public ArticleData Data = null;

    [SerializeField]
    private float _maxTextSize = 120;

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

    ChatManager _convManager = null;

    ArticleFeed _articleFeed = null;

    private LocalizeStringEvent _company;
    private LocalizeStringEvent _title;
    private LocalizeStringEvent _body;

    bool[] _sharedWithGroups;
    int _numGroupsToShareWith;

    GameObject _shareArticleButtons = null;

    [Header("Gradients")]
    [SerializeField] GradientObject correctA;
    [SerializeField] GradientObject correctB;
    [SerializeField] GradientObject incorrectA;
    [SerializeField] GradientObject incorrectB;

    [Header("GameObjects")]
    [SerializeField] GameObject gButtonSkip;
    [SerializeField] GameObject gButtonShare;
    [SerializeField] GameObject gArticleBackground;

    public bool IsTrue { get { return Data.isTrue; } }

    public bool[] HasSharedWithGroups
    {
        get {  return _sharedWithGroups; }
    }

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

    bool dataAlreadySet = false;
    private void Start()
    {
        _articleFeed = GetComponentInParent<ArticleFeed>();

        _convManager = FindAnyObjectByType<ChatManager>();

        _articleBody.SetActive(false);

        _company = _companyText.GetComponent<LocalizeStringEvent>();
        _title = _articleTitle.GetComponent<LocalizeStringEvent>();
        _body = _bodyText.GetComponent<LocalizeStringEvent>();

        if (Data != null && !dataAlreadySet)
        {
            SetArticleData(Instantiate(Data));
        }
    }

    public string GetBodyString()
    {
        return _articleBody.GetComponentInChildren<TextMeshProUGUI>().text;
    }


    #region Article Actions
    public void OnShareWithGroups(int numGroups)
    {
        _shareArticleButtons = _convManager.SpawnShareButtons();
        Button[] buttons = _shareArticleButtons.GetComponentsInChildren<Button>();

        buttons[buttons.Length-1].onClick.AddListener(() =>
        {
            _hasSharedArticle = true;

            buttons[buttons.Length - 1].transform.parent.parent.parent.gameObject.SetActive(false);

            OnShare.Invoke();
            
            _convManager.ChangeToMainChat();

            LevelManager.Instance.ShowNextArticle();
            
            Destroy(_shareArticleButtons);
            _shareArticleButtons = null;

        });

        // nos saltamos el último botón, que es el de no compartir
        for (int i = 0; i <  buttons.Length - 1; i++) {
            Button bt = buttons[i];

            if (i < numGroups)
            {
                bt.interactable = !_sharedWithGroups[i];
                if (bt.interactable)
                {
                    AddShareArticleListenerToButton(i, bt);
                }
            }
            else
            {
                bt.gameObject.SetActive(false);
            }
        }
    }

    public void AddShareArticleListenerToButton(int group, Button button)
    {
        button.onClick.RemoveAllListeners();
        int tempInt = group;
        Conversation conversation = null;
        if (Data.conversation != null && Data.conversation.Count > group) { conversation = Data.conversation[group]; }
        button.onClick.AddListener(() => ShareArticle(tempInt, _shareArticleButtons, conversation));
        button.onClick.AddListener(() => button.transform.parent.parent.parent.parent.gameObject.SetActive(false));

    }

    /// <summary>
    /// TODO: Skip article, changing points and general player score if skipped article was false or true
    /// </summary>
    bool _skipped = false;
    public void SkipArticle()
    {
        if (_skipped) return;

        // ANIMACION 
        if(IsTrue)
        {
            GetComponent<ElectionVFX>().setGradient(false);
            GetComponent<Animator>().SetTrigger("incorrect"); // CONTESTA MAL
            gButtonSkip.GetComponent<Animator>().SetTrigger("incorrect");
            gButtonSkip.GetComponent<ElectionVFX>().setGradient(false);
                
        }
        else
        {
            GetComponent<ElectionVFX>().setGradient(true);
            GetComponent<Animator>().SetTrigger("correct"); // CONTESTA BIEN
            gButtonSkip.GetComponent<Animator>().SetTrigger("correct");
            gButtonSkip.GetComponent<ElectionVFX>().setGradient(true);
            gButtonSkip.GetComponent<ElectionVFX>().setParticles();
        }

        _skipped = true;
    }

    public void InvokeOnSkip()
    {
        OnSkip.Invoke();
    }

    public void IncreaseBodySize()
    {
        _bodyText.fontSize = Mathf.Min(_bodyText.fontSize * 1.2f, _maxTextSize);

        _increaseTextButton.interactable = _bodyText.fontSize != _maxTextSize;
        
        // _articleTitle.fontSize *= 1.2f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        foreach (Transform tr in transform.parent.GetComponentsInChildren<Transform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tr as RectTransform);
        }
        RebuildAllLayouts();
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

        _convManager.ChangeGroup(groupID + 1);
        // Data should be an instance

        _sharedWithGroups[groupID] = true;
        Data.sharedWithGroups = _sharedWithGroups;
        _convManager.SendArticle(Data);

        if(conv == null)
        {
            conv = ConversationCompendium.Instance.GetConversation(groupID, Data.companyName, Data.theme, Data.convType);
        }

        _convManager.SetConversation(conv, true);

        Destroy(shareButtons);

        _hasSharedArticle = true;
    
        OnShare.Invoke();
    }

    public void ReadArticle()
    {
        _articleBody.SetActive(true);
        //_readButton.interactable = false;
        _readButton.gameObject.SetActive(false);

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
        if (Data.companyName.ToLower() == "newspaper" ||
            Data.companyName.ToLower() == "social" || 
            Data.companyName.ToLower() == "blog" ||
            Data.companyName.ToLower() == "web")
        {
            _company.StringReference.SetReference("Translation", "SOURCE/" + Data.companyName.ToUpper());
        }
        else
        {
            _company.enabled = false;
            _companyText.text = Data.companyName.ToUpper();
        }

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

        _numGroupsToShareWith = data.numGroupsToShareWith;
        
        _sharedWithGroups = Data.sharedWithGroups;

        if (!data.canBeSharedWithGroups)
        {
            SetupShareButtonForVerification();
        }
        else SetupShareButtonForGroups(_numGroupsToShareWith);

        dataAlreadySet = true;

        RebuildAllLayouts();
    }

    public void DestroyArticle()
    {
        if(_shareArticleButtons != null) Destroy(_shareArticleButtons);
        Destroy(gameObject);
    }

    public void SetupShareButtonForGroups(int numGroups)
    {
        _numGroupsToShareWith = numGroups;
        _shareButton.onClick.RemoveAllListeners();
        _shareButton.onClick.AddListener(() => OnShareWithGroups(numGroups));
    }

    public void SetupShareButtonForVerification()
    {
        // play animation to the right and a blueish tone :3
        // on animation end -> shownextarticle

        _shareButton.onClick.RemoveAllListeners();
        //_shareButton.onClick.AddListener(() => GetComponent<Animator>().SetTrigger("Share"));
        _shareButton.onClick.AddListener(sharebuttonVFX);
    }

    public void VerifyArticleSharing()
    {
        _hasSharedArticle = true; OnShare.Invoke();
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
        _shareButton.onClick.AddListener(() => OnShareWithGroups(3));
    }
    #endregion

    public void sharebuttonVFX()
    {
        // ANIMACION 
        if (IsTrue)
        {
            GetComponent<ElectionVFX>().setGradient(true);
            GetComponent<Animator>().SetTrigger("correct"); // CONTESTA BIEN
            gButtonShare.GetComponent<Animator>().SetTrigger("correct");
            gButtonShare.GetComponent<ElectionVFX>().setGradient(true);
            gButtonShare.GetComponent<ElectionVFX>().setParticles();
        }
        else
        {
            GetComponent<ElectionVFX>().setGradient(false);
            GetComponent<Animator>().SetTrigger("incorrect"); // CONTESTA MAL
            gButtonShare.GetComponent<Animator>().SetTrigger("incorrect");
            gButtonShare.GetComponent<ElectionVFX>().setGradient(false);
        }
    }
}
