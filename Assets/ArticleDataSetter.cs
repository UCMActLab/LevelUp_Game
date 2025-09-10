using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;

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
    [SerializeField] TextMeshProUGUI _postedHoursAgo = null;
    [SerializeField] Image _articleImage = null;
    [SerializeField] TextMeshProUGUI _articleTitle = null;
    [SerializeField] TextMeshProUGUI _likes = null;
    [SerializeField] TextMeshProUGUI _reposts = null;

    [Header("Body")]
    [SerializeField] GameObject _articleBody = null;
    [SerializeField] TextMeshProUGUI _bodyText = null;

    [Header("Buttons")]
    [SerializeField] Button _readButton = null;
    [SerializeField] Button _shareButton = null;
    [SerializeField] Button _skipButton = null;

    private Choice read;
    private Choice skip;

    public ArticleData Data = null;
    public ArticleAction Action;

    public event Action<Choice> OnRead;
    public event Action<Choice> OnSkip;
    public event Action<Choice> OnShare;
    public event Action<Choice> AnswerClicked;

    ConversationManager _convManager = null;

    private void Start()
    {
        _convManager = FindAnyObjectByType<ConversationManager>();
    }

    public void ChangeButtonsOnArticleRead()
    {
        _readButton.interactable = false;

        if(_convManager.story.currentChoices.Count > 0)
        {
            Choice sendGroup0 = _convManager.story.currentChoices[0];
            Choice sendGroup1 = _convManager.story.currentChoices[1];
            Choice sendGroup2 = _convManager.story.currentChoices[2];
            Choice skip = _convManager.story.currentChoices[3];

            _skipButton.onClick.RemoveAllListeners();

            _skipButton.onClick.AddListener(() =>
            {
                SkipArticle(skip);
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

        OnSkip?.Invoke(skip);
    }

    private void ShareButton(Choice group0, Choice group1, Choice group2)
    {
        
    }

    public void SetArticleData(ArticleData data)
    {
        if (data == null) return;

        Data = data;
        
        _companyLogo.sprite = Data.companyLogo;
        _companyText.text = Data.companyName;
        _postedHoursAgo.text = Data.hoursAgoPosted + "h";
        _articleImage.sprite = Data.articleImage;
        _articleTitle.text = Data.articleTitle;
        _likes.text = Data.likes.ToString();
        _reposts.text = Data.reposts.ToString();

        if(data.articleBody != null && data.articleTitle != string.Empty)
        {
            _articleBody.SetActive(true);
            _bodyText.text = data.articleBody;
        }
        else
        {
            _articleBody.SetActive(false);
            _bodyText.text = string.Empty;
        }
    }

    public void SetUpReadSkipButtons(Choice read, Choice skip)
    {
        _readButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Read;
            OnRead?.Invoke(read);
        });
        _skipButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Skip;
            OnSkip?.Invoke(skip);
        });
        _shareButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Share;
            OnShare?.Invoke(skip);
        });
    }
}
