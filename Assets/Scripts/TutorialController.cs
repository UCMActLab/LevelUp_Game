using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [Serializable]
    public struct TutorialStepMessages
    {
        public List<string> messages;
        public SerializedCallback<bool> nextStepCondition;
        public UnityEvent onMessagesStart;
        public UnityEvent onMessagesEnd;
    }

    [Header("DEBUG")]
    [SerializeField] private bool DEBUGGING_TUTORIAL = true;
    [SerializeField] private int STARTING_STEP = 5;

    [Header("General")]
    [Tooltip("The greater the value, the greater the time you'll have to wait for the NextStep button to activate"), 
        SerializeField, Range(0.01f, 0.1f)] private float _waitFactor = .01f;

    [SerializeField] private GameObject _articlePrefab;
    [SerializeField] private Transform _chat;
    [SerializeField] private TextMeshProUGUI _messageToUser;
    private LocalizeStringEvent _messageToUserLocalized;
    private Animator _messageAnimator = null;
    private AudioSource _messageAudioSource = null;

    [SerializeField] private Fader _fader;

    [SerializeField] Button _nextStepButton = null;
    [SerializeField] Button _backStepButton = null;

    [SerializeField] private ArticleData _data;

    [SerializeField] private ScrollRect _scrollRect;

    [Header("Events")]
    public UnityEvent OnTutorialEnd = new UnityEvent();

    [Header("Steps")]
    [SerializeField] private List<TutorialStepMessages> _messageSteps;

    private GameObject _scrollContent;

    private ArticleDataSetter[] _articles;


    private bool _buttonWasPressed = false;

    int _currentStep = 0;

    #region Tutorial Checks Variables
    float _initialScrollRectValue;
    bool _hasSkipedAnArticle = false;
    bool _hasReadAnArticle = false;
    bool _hasSharedAnArticle = false;
    #endregion

    [SerializeField, Range(0.001f, 5.0f)] private float _timeBetweenMessages = 1.0f;

    private void Start()
    {
        InitialSetup();

        StartCoroutine(ShowMessages());
    }

    private void InitialSetup()
    {
        _messageToUserLocalized = _messageToUser.GetComponent<LocalizeStringEvent>();

        _scrollContent = _scrollRect.content.gameObject;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_messageToUser.transform.parent as RectTransform);

        _messageAnimator = _messageToUser.transform.parent.GetComponent<Animator>();
        _messageAudioSource = _messageAnimator.GetComponent<AudioSource>();

        _data = Instantiate(_data);

        _articles = _scrollContent.GetComponentsInChildren<ArticleDataSetter>();
        foreach(ArticleDataSetter article in _articles)
        {
            article.ActivateButtons(false);
        }

        _initialScrollRectValue = _scrollRect.verticalNormalizedPosition;

        if (DEBUGGING_TUTORIAL) { 
            for(_currentStep = 0; _currentStep < STARTING_STEP; ++_currentStep)
            {
                _messageSteps[_currentStep].onMessagesStart.Invoke();
                _messageSteps[_currentStep].onMessagesEnd.Invoke();
            }
        }
    }

    private void ActivateNextStepButton(bool active)
    {
        _nextStepButton.interactable = active;
    }

    private bool WasButtonPressed()
    {
        return _buttonWasPressed;
    }

    private void ShowNewMessage(string text)
    {
        _messageToUserLocalized.StringReference.SetReference("TUTORIAL_STEPS", text);
        _messageAnimator.SetTrigger("NewMessage");
        _messageAudioSource.Play();
    }

    IEnumerator ShowMessages()
    {
        _nextStepButton.onClick.RemoveAllListeners();
        _nextStepButton.onClick.AddListener(() => { _buttonWasPressed = true; ActivateNextStepButton(false); });

        TutorialStepMessages stepMessages = _messageSteps[_currentStep];

        stepMessages.onMessagesStart?.Invoke();

        if(!DEBUGGING_TUTORIAL)
        {
            List<string> messages = stepMessages.messages;

            // we show the first message 
            ShowNewMessage(messages[0]);

            int i = 1;
            while (i < messages.Count)
            {
                // Show message after time
                yield return new WaitUntil(WasButtonPressed);
                ShowNewMessage(messages[i]);

                // Activate button after time
                yield return new WaitForSeconds(TimeToReadMessage(messages[i]));
                ActivateNextStepButton(true);
                _buttonWasPressed = false;

                ++i;
            }
        }


        ActivateNextStepButton(false);

        stepMessages.onMessagesEnd?.Invoke();

        yield return new WaitUntil(() => stepMessages.nextStepCondition.Invoke());

        _nextStepButton.onClick.RemoveAllListeners();

        ActivateNextStepButton(true);

        _nextStepButton.onClick.AddListener(NextStep);
    }

    private float TimeToReadMessage(string message)
    {
        return message.Length * _waitFactor;
    }
         
    #region On Messages Ended

    public void ActivateSkipButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.EnableSkipButton(true);
            article.OnSkip.AddListener(SkippedArticle);
        }
    }

    public void ActivateReadButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.EnableReadButton(true);
            article.OnRead.AddListener(ReadArticle);
        }
    }
    
    public void ActivateShareButtons()
    {
        foreach(ArticleDataSetter article in _articles)
        {
            article.EnableShareButton(true);
            article.OnShare.AddListener(ClickedShareButton); 
        }
    }
    #endregion

    #region Blocking Step Callbacks
    // this function only exists for some steps that need no blocking to finish executing succesfully
    public bool DontStopStep()
    {
        return true;
    }

    public bool HasMovedScrollRect()
    {
        return Mathf.Abs(_initialScrollRectValue - _scrollRect.verticalNormalizedPosition) > 0.1f;
    }

    private void SkippedArticle()
    {
        _hasSkipedAnArticle = true;

        foreach (ArticleDataSetter article in _articles)
        {
            article.EnableSkipButton(false);
            article.HighlightSkipButton(false);
        }
    }

    private async void ReadArticle()
    {
        await Task.Delay((int)(TimeToReadMessage(_articles[0].GetBodyString()) * 1000));

        _hasReadAnArticle = true;

        foreach (ArticleDataSetter article in _articles)
        {
            article.EnableReadButton(false);
            article.HighlightReadButton(false);
        }
    }

    private void ClickedShareButton()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.EnableShareButton(false);
            article.HighlightShareButton(false);
        }
    }

    public void CameBackToArticleFeed()
    {
        _hasSharedAnArticle = true;
    }

    public bool HasSkippedArticle()
    {
        return _hasSkipedAnArticle;
    }

    public bool HasReadArticle()
    {
        return _hasReadAnArticle;
    }

    public bool HasSharedAnArticle()
    {
        return _hasSharedAnArticle;
    }

    public void SetupLastQuestions()
    {
        _hasSharedAnArticle = false;
        _hasReadAnArticle = false;
        _hasSkipedAnArticle = false;
    }

    public void SetupSkipButtonsForLastQuestions()
    {
        foreach (ArticleDataSetter art in _articles)
        {
            art.OnSkip.RemoveAllListeners();

            art.OnSkip.AddListener(() => _hasSkipedAnArticle = true);
        }
    }

    public void SetupReadButtonsForLastQuestions()
    {
        foreach (ArticleDataSetter art in _articles)
        {
            art.OnRead.RemoveAllListeners();

            art.OnRead.AddListener(() => _hasReadAnArticle = true);
        }
    }

    public void SetupShareButtonsForLastQuestions()
    {
        foreach (ArticleDataSetter art in _articles)
        {
            art.OnShare.RemoveAllListeners();

            art.OnShare.AddListener(() => _hasSharedAnArticle = true);
        }
    }
    #endregion

    public void FadeIn()
    {
        _fader.StartFade(2.5f, 0.9f, 0.0f);
    }

    public void FadeOut()
    {
        _fader.StartFade(2.5f, 0.0f, 0.9f);
    }

    public void ShowArticleButtonsDisabled()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.ActivateButtons(true);
            article.EnableButtonsInteraction(false);
        }
    }

    public void HighlightSkipButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.HighlightSkipButton(true);
        }
    }

    public void UnhighlightSkipButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.HighlightSkipButton(false);
        }
    }

    public void HighlightReadButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.HighlightReadButton(true);
        }
    }

    public void UnhighlightReadButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.HighlightReadButton(false);
        }
    }

    public void HighlightShareButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.HighlightShareButton(true);
        }
    }

    public void UnhighlightShareButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.HighlightShareButton(false);
        }
    }


    private void NextStep()
    {
        if (++_currentStep < _messageSteps.Count)
        {
            StartCoroutine(ShowMessages());
        }
        else
        {
            OnTutorialEnd.Invoke();
        }
    }
}
