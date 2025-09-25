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

    public event Action<Choice> OnRead;
    public event Action<Choice> OnSkipChoice;
    public event Action<Choice> OnShare;
    public event Action<Choice> AnswerClicked;

    public event Action OnSkip;

    ConversationManager _convManager = null;

    private void Start()
    {
        _convManager = FindAnyObjectByType<ConversationManager>();

        _articleBody.SetActive(false);
        SetArticleData(Data);
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

        OnSkipChoice?.Invoke(skip);
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
                _readButton.interactable = false;
                _skipButton.interactable = false;
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
                _readButton.interactable = false;
                _skipButton.interactable = false;
                Action = ArticleAction.None; 
            });

            string[] words = choices[0].text.Split(' ');
            string text = words[words.Length - 1].Trim('.').ToUpper();
            buttons[1].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
            buttons[1].onClick.AddListener(() => {
                _convManager.ChangeGroup(choices[0]);
                _convManager.SendArticle(Data);
                OnShare.Invoke(choices[0]); 
                Destroy(share);
                _readButton.interactable = false;
                _skipButton.interactable = false;
            });

            if (choices.Count > 2) {
                words = choices[1].text.Split(' ');
                text = words[words.Length - 1].Trim('.').ToUpper();
                buttons[2].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
                buttons[2].onClick.AddListener(() => {
                    _convManager.ChangeGroup(choices[1]);
                    _convManager.SendArticle(Data);
                    OnShare.Invoke(choices[1]); 
                    Destroy(share);
                    _readButton.interactable = false;
                    _skipButton.interactable = false;
                });
            } 
            else buttons[2].transform.parent.gameObject.SetActive(false);

            if (choices.Count > 3) {
                words = choices[2].text.Split(' ');
                text = words[words.Length - 1].Trim('.').ToUpper();
                buttons[3].transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
                buttons[3].onClick.AddListener(() => { 
                    _convManager.ChangeGroup(choices[2]);
                    _convManager.SendArticle(Data);
                    OnShare.Invoke(choices[2]);
                    Destroy(share);
                    _readButton.interactable = false;
                    _skipButton.interactable = false;
                });
            } 
            else buttons[3].transform.parent.gameObject.SetActive(false);
        }
    }

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

    public void SetArticleData(ArticleData data)
    {
        if (data == null) return;

        Data = data;
        
        _companyLogo.sprite = Data.companyLogo;
        _companyText.text = Data.companyName;
        _articleImage.sprite = Data.articleImage;
        _articleImage.gameObject.SetActive(_articleImage.sprite != null);

        _articleTitle.text = Data.articleTitle;
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
            OnSkipChoice?.Invoke(skip);
        });
        _shareButton.onClick.AddListener(() =>
        {
            Action = ArticleAction.Share;
            OnShare?.Invoke(skip);
        });
    }

    public void ReadArticleByButton()
    {
        _articleBody.SetActive(true);
        _readButton.interactable = false;
    }

    /// <summary>
    /// TODO: Skip article, changing points and general player score if skipped article was false or true
    /// </summary>
    bool _skipped = false;
    public void SkipArticle()
    {
        if (_skipped) return;

        OnSkip.Invoke();

        GetComponent<Animator>().SetTrigger("Skip");
    }

    public void DestroyArticle()
    {
        Destroy(gameObject);
    }
}
