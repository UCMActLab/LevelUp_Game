using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [Serializable]
    public struct TutorialStepMessages
    {
        public List<string> messages;
        public SerializedCallback<bool> nextStepCondition;
        public UnityEvent onMessagesEnd;
    }

    [Tooltip("The greater the value, the greater the time you'll have to wait for the NextStep button to activate"), 
        SerializeField, Range(0.01f, 0.1f)] private float _waitFactor = .01f;

    [SerializeField] private GameObject _articlePrefab;
    [SerializeField] private Transform _chat;
    [SerializeField] private TextMeshProUGUI _messageToUser;
    private Animator _messageAnimator = null;

    [SerializeField] private Image _fadeImage;

    [SerializeField] Button _nextStepButton = null;
    [SerializeField] Button _backStepButton = null;

    [SerializeField] private List<TutorialStepMessages> _messageSteps;

    [SerializeField] private ArticleData _data;

    [SerializeField] private ScrollRect _scrollRect;
    private GameObject _scrollContent;

    private ArticleDataSetter[] _articles;

    public UnityEvent OnTutorialEnd = new UnityEvent();

    private bool _buttonWasPressed = false;

    int _currentStep = 0;

    #region Tutorial Checks Variables
    float _initialScrollRectValue;
    bool _hasSkipedAnArticle = false;
    #endregion

    [SerializeField, Range(0.001f, 5.0f)] private float _timeBetweenMessages = 1.0f;

    private void Start()
    {
        InitialSetup();

        StartCoroutine(ShowMessages());
    }

    private void InitialSetup()
    {
        _scrollContent = _scrollRect.content.gameObject;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_messageToUser.transform.parent as RectTransform);

        _messageAnimator = _messageToUser.transform.parent.GetComponent<Animator>();

        _data = Instantiate(_data);

        _fadeImage.color = Color.black;

        _articles = _scrollContent.GetComponentsInChildren<ArticleDataSetter>();
        foreach(ArticleDataSetter article in _articles)
        {
            article.ActivateButtons(false);
        }

        _initialScrollRectValue = _scrollRect.verticalNormalizedPosition;
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
        _messageToUser.text = text;
        _messageAnimator.SetTrigger("NewMessage");
    }

    IEnumerator ShowMessages()
    {
        _nextStepButton.onClick.RemoveAllListeners();
        _nextStepButton.onClick.AddListener(() => { _buttonWasPressed = true; ActivateNextStepButton(false); });

        List<string> messages = _messageSteps[_currentStep].messages;

        // we show the first message 
        ShowNewMessage(messages[0]);

        int i = 1;
        while (i < messages.Count)
        {
            // Show message after time
            yield return new WaitUntil(WasButtonPressed);
            ShowNewMessage(messages[i]);

            // Activate button after time
            yield return new WaitForSeconds(messages[i].Length * _waitFactor);
            ActivateNextStepButton(true);
            _buttonWasPressed = false;

            ++i;
        }

        ActivateNextStepButton(false);

        _messageSteps[_currentStep].onMessagesEnd?.Invoke();

        yield return new WaitUntil(() => _messageSteps[_currentStep].nextStepCondition.Invoke());

        _nextStepButton.onClick.RemoveAllListeners();
        _nextStepButton.onClick.AddListener(DoStep);

        ActivateNextStepButton(true);
    }

    #region On Messages Ended

    public void ActivateSkipButtons()
    {
        foreach (ArticleDataSetter article in _articles)
        {
            article.EnableSkipButton(true);
            article.OnSkip += SkippedArticle;
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

    public bool HasSkippedArticle()
    {
        return _hasSkipedAnArticle;
    }
    #endregion


    public void DoStep()
    {
        switch(_currentStep)
        {
            case 0:
                // aquí hacer el fade
                _fadeImage.color = new Color(0,0,0,0);
                break;
            case 1:
                foreach(ArticleDataSetter article in  _articles)
                {
                    article.ActivateButtons(true);
                    article.EnableButtonsInteraction(false);
                }
                break;
            case 2:
                foreach (ArticleDataSetter article in _articles)
                {
                    article.HighlightSkipButton(true);
                }
                break;
            case 3:
                foreach (ArticleDataSetter article in _articles)
                {
                    article.HighlightSkipButton(false);
                    article.HighlightReadButton(true);
                }
                break;
        }
        _currentStep++;

        // Esto hay que cambiarlo. Hay que hacer que, si no han cumplido una condición concreta, no puedan ir al siguiente paso :p
        NextStep();
    }

    private void NextStep()
    {
        if (_currentStep < _messageSteps.Count)
        {
            StartCoroutine(ShowMessages());
        }
        else
        {
            OnTutorialEnd.Invoke();
        }
    }
}
