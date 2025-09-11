using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Chat.System;
using DA_Assets.Extensions;

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
            Choice skip = _convManager.story.currentChoices[3];

            _skipButton.onClick.RemoveAllListeners();
            _shareButton.onClick.RemoveAllListeners();

            _skipButton.onClick.AddListener(() =>
                SkipArticle(skip)
            );
            _shareButton.onClick.AddListener(() =>
            {
                ShareButton(_convManager.story.currentChoices);
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

        OnSkip?.Invoke(skip);
    }

    public void ShareButton(System.Collections.Generic.List<Choice> choices)
    {
        if(choices.Count == 1)
        {
            // escogemos automáticamente no enviar más artículos si no quedan grupos
            GameObject share = _convManager.SpawnShareButtons();
            int i = 0;
            Button[] buttons = share.GetComponentsInChildren<Button>();
            foreach(Button bt in buttons)
            {
                bt.transform.parent.gameObject.SetActive(false);
            }
            buttons[0].transform.parent.gameObject.SetActive(true);
            buttons[0].onClick.AddListener(() =>
            {
                _convManager.ChangeGroup(null);
                OnShare.Invoke(choices[0]);
                Destroy(share); 
                Action = ArticleAction.None;
            });
        }
        else
        {
            GameObject share = _convManager.SpawnShareButtons();
            int i = 0;
            Button[] buttons = share.GetComponentsInChildren<Button>();

            buttons[0].onClick.AddListener(() => {
                _convManager.ChangeGroup(null);
                OnShare.Invoke(choices[choices.Count - 1]); 
                Destroy(share); 
                Action = ArticleAction.None; 
            });

            string text = choices[0].text.Replace("Send it to your ", string.Empty).Trim('.').ToUpper();
            buttons[1].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
            buttons[1].onClick.AddListener(() => {
                _convManager.ChangeGroup(choices[0]);
                _convManager.SendArticle(Data);
                OnShare.Invoke(choices[0]); 
                Destroy(share); 
            });

            if (choices.Count > 2) {
                text = choices[1].text.Replace("Send it to your ", string.Empty).Trim('.').ToUpper();
                buttons[2].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
                buttons[2].onClick.AddListener(() => {
                    _convManager.ChangeGroup(choices[1]);
                    _convManager.SendArticle(Data);
                    OnShare.Invoke(choices[1]); 
                    Destroy(share); 
                });
            } 
            else buttons[2].transform.parent.gameObject.SetActive(false);

            if (choices.Count > 3) {
                text = choices[2].text.Replace("Send it to your ", string.Empty).Trim('.').ToUpper();
                buttons[3].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
                buttons[3].onClick.AddListener(() => { 
                    _convManager.ChangeGroup(choices[2]);
                    _convManager.SendArticle(Data);
                    OnShare.Invoke(choices[2]);
                    Destroy(share); 
                });
            } 
            else buttons[3].transform.parent.gameObject.SetActive(false);
        }
    }

    public void DeactivateButtons()
    {
        Destroy(_readButton.gameObject);
        Destroy(_skipButton.gameObject);
        Destroy(_shareButton.gameObject);
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

    public void SetUpButtons(Choice read, Choice skip)
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
