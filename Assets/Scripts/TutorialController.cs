using DA_Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [Serializable]
    public struct Choice
    {
        public string button;
        public Vector2Int toChoiceStep;
        public Color color;

        Choice(string name, Vector2Int choice, Color buttonColor)
        {
            button = name;
            toChoiceStep = choice;
            color = buttonColor;
        }
    }
    [Serializable]
    public struct TutorialStepMessages
    {
        public List<string> messages;
        public bool isLinear;
        public Vector2Int toChoiceStep;
        public bool hasChoice;
        public Choice[] buttons;
        public bool goNextOnActionDone;
        public SerializedCallback<bool> nextStepCondition;
        public UnityEvent onMessagesStart;
        public UnityEvent onMessagesEnd;
    }

    [Serializable]
    public struct ChoiceTutorialStepMessages
    {
        public List<TutorialStepMessages> steps;
    }

    [Header("DEBUG")]
    [SerializeField] private bool DEBUGGING_TUTORIAL = true;
    [SerializeField] private int STARTING_STEP = 5;
    [SerializeField] private TextMeshProUGUI _currentStepText = null;

    [Header("General")]
    [Tooltip("The greater the value, the greater the time you'll have to wait for the NextStep button to activate"), 
        SerializeField, Range(0.00f, 0.1f)] private float _waitFactor = .01f;

    [SerializeField] private int _initialStepIndex = 12;
    [SerializeField] private GameObject _articlePrefab;
    [SerializeField] private GameObject _choiceButtons;
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private Transform _chat;
    [SerializeField] private TextMeshProUGUI _messageToUser;
    [SerializeField] private GameObject _secondArticle;
    private LocalizeStringEvent _messageToUserLocalized;
    private Animator _messageAnimator = null;
    private FMODUnity.StudioEventEmitter _messageAudioSource = null;

    [SerializeField] private Fader _fader;

    [SerializeField] Button _nextStepButton = null;
    [SerializeField] Button _backStepButton = null;

    [SerializeField] private ArticleData _data;

    [SerializeField] private ScrollRect _scrollRect;

    [SerializeField] private InstantFeedback _feedback = null;

    [Header("Events")]
    public UnityEvent OnTutorialEnd = new UnityEvent();

    [Header("Steps")]
    // [SerializeField] private List<TutorialStepMessages> _messageSteps;
    [SerializeField] private List<ChoiceTutorialStepMessages> _choicesMessageSteps;

    private int _currentChoice = 0;
    private bool _lastStepWasLinear = true;
    private bool _choiceIsMade = false;

    private GameObject _scrollContent;

    private ArticleGameObject[] _articles;


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
        _currentChoice = _initialStepIndex;

        _messageToUserLocalized = _messageToUser.GetComponent<LocalizeStringEvent>();

        _scrollContent = _scrollRect.content.gameObject;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_messageToUser.transform.parent as RectTransform);

        _messageAnimator = _messageToUser.transform.parent.parent.GetComponent<Animator>();
        _messageAudioSource = _messageAnimator.GetComponent<FMODUnity.StudioEventEmitter>();

        _data = Instantiate(_data);

        _articles = _scrollContent.GetComponentsInChildren<ArticleGameObject>();
        foreach(ArticleGameObject article in _articles)
        {
            article.ActivateButtons(false);
        }

        _initialScrollRectValue = _scrollRect.verticalNormalizedPosition;

        if (DEBUGGING_TUTORIAL) { 
            for(_currentStep = 0; _currentStep < STARTING_STEP; ++_currentStep)
            {
                _choicesMessageSteps[_currentChoice].steps[_currentStep].onMessagesStart.Invoke();
                _choicesMessageSteps[_currentChoice].steps[_currentStep].onMessagesEnd.Invoke();
            }
        }
        else
        {
            if (_currentStepText != null) _currentStepText.gameObject.SetActive(false);
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

        TutorialStepMessages stepMessages = _choicesMessageSteps[_currentChoice].steps[_currentStep];

        stepMessages.onMessagesStart?.Invoke();

        List<string> messages = stepMessages.messages;

        // we show the first message 
        ShowNewMessage(messages[0]);

        int i = 1;
        while (i < messages.Count)
        {
            // Show message after time
            ActivateNextStepButton(true);
            yield return new WaitUntil(WasButtonPressed);
            // action was made
            SubmitTutorialInteractionEvent();

            ShowNewMessage(messages[i]);

            // Activate button after time
            yield return new WaitForSeconds(TimeToReadMessage(_messageToUser.text));
            ActivateNextStepButton(true);
            _buttonWasPressed = false;

            ++i;
        }

        ActivateNextStepButton(false);

        stepMessages.onMessagesEnd?.Invoke();

        if(!stepMessages.isLinear && !stepMessages.hasChoice)
        {
            ToChoice(stepMessages.toChoiceStep);

            yield return new WaitUntil(() => stepMessages.nextStepCondition.Invoke());
        }
        else if (stepMessages.hasChoice)
        {
            SetUpChoiceButtons(stepMessages.buttons);

            yield return new WaitUntil(() => _choiceIsMade);
            _choiceIsMade = false;
        }
        else
        {
            _lastStepWasLinear = true;
            yield return new WaitUntil(() => stepMessages.nextStepCondition.Invoke());
        }
        SubmitTutorialInteractionEvent();

        _nextStepButton.onClick.RemoveAllListeners();

        if(stepMessages.goNextOnActionDone || stepMessages.hasChoice)
        {
            NextStep();
        }
        else
        {
            ActivateNextStepButton(true);

            _nextStepButton.onClick.AddListener(NextStep);
        }

    }

    public void SetUpChoiceButtons(Choice[] buttons)
    {
        _choiceIsMade = false;
        _choiceButtons.gameObject.DestroyChilds();

        foreach (Choice button in buttons)
        {
            Button newButton = Instantiate(_choiceButtonPrefab, _choiceButtons.transform).GetComponent<Button>();
            newButton.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("Translation", button.button);
            Vector2Int choice = button.toChoiceStep;
            newButton.onClick.AddListener(() =>
            {
                ToChoice(choice);
                _choiceButtons.gameObject.SetActive(false);
                _choiceIsMade = true;
            }
            );
        }

        _choiceButtons.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_choiceButtons.transform as RectTransform);
    }

    private void ToChoice(Vector2Int choice)
    {
        _lastStepWasLinear = false;
        Debug.Log("CHOICE: " + choice);
        _currentChoice = choice.x;
        _currentStep = choice.y;
    }

    private float TimeToReadMessage(string message)
    {
        return message.Length * _waitFactor;
    }
    
    #region On Messages Ended
    public void RestartVarialbes()
    {
        _hasReadAnArticle = false;
        _hasSkipedAnArticle = false;
        _hasSharedAnArticle = false;
    }

    public void InstantiateSecondArticle()
    {
        Transform parent = null;
        foreach (ArticleGameObject article in _articles)
        {
            parent = article.transform.parent;
            Destroy(article.gameObject);
        }

        _secondArticle.gameObject.SetActive(true);
        ArticleGameObject art = _secondArticle.GetComponent<ArticleGameObject>();
        art.ActivateShareButton(false);
        art.SetUpButtons();
        art.OnRead.AddListener(() => _hasReadAnArticle = true);

        _articles = new ArticleGameObject[1];
        _articles[0] = art;

        LayoutRebuilder.ForceRebuildLayoutImmediate(art.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.transform as RectTransform);

        AddSecondListenerToSkipButton();
    }

    public void AddFirstListenerToSkipButton()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.OnSkip.AddListener(() =>
            {
                SkippedArticle();
                ToChoice(new Vector2Int(3, 0));
            });
        }
    }

    public void AddSecondListenerToSkipButton()
    {
        _secondArticle.GetComponent<ArticleGameObject>().OnSkip.AddListener(() => {
            SkippedArticle();
            ToChoice(new Vector2Int(7, 0));
        });
    }

    public void AddListenerToReadButton()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.OnRead.AddListener(ReadArticle);
        }
    }

    //public void AddReadFirstFeedbackToSkipButton()
    //{
    //    foreach (ArticleGameObject article in _articles)
    //    {
    //        article.OnSkip.AddListener(() =>
    //        {
    //            _feedback.Setup("Deberías leer antes de saltarte un artículo, no puedes sacar toda la información de su título.");

    //        });
    //    }
    //}

    public void AddListenerToSkipButton()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.OnSkip.AddListener(SkippedArticle);
        }
    }

    public void AddListenerToShareButton()
    {
        foreach(ArticleGameObject article in _articles)
        {
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

        foreach (ArticleGameObject article in _articles)
        {
            article.EnableSkipButton(false);
            article.HighlightSkipButton(false);
        }
    }

    private async void ReadArticle()
    {
        await Task.Delay((int)(TimeToReadMessage(_articles[0].GetBodyString())));

        _hasReadAnArticle = true;

        foreach (ArticleGameObject article in _articles)
        {
            article.EnableReadButton(false);
            article.HighlightReadButton(false);
        }
    }

    private void ClickedShareButton()
    {
        foreach (ArticleGameObject article in _articles)
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

    public bool HasSkippedOrReadArticle()
    {
        return _hasReadAnArticle || _hasSkipedAnArticle;
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
        foreach (ArticleGameObject art in _articles)
        {
            art.OnSkip.RemoveAllListeners();

            art.OnSkip.AddListener(() => _hasSkipedAnArticle = true);
        }
    }

    public void SetupReadButtonsForLastQuestions()
    {
        foreach (ArticleGameObject art in _articles)
        {
            art.OnRead.RemoveAllListeners();

            art.OnRead.AddListener(() => _hasReadAnArticle = true);
        }
    }

    public void SetupShareButtonsForLastQuestions()
    {
        foreach (ArticleGameObject art in _articles)
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
        foreach (ArticleGameObject article in _articles)
        {
            article.ActivateButtons(true);
            article.EnableButtonsInteraction(false);
        }
    }

    public void HighlightSkipButtons()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.HighlightSkipButton(true);
        }
    }

    public void UnhighlightSkipButtons()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.HighlightSkipButton(false);
        }
    }

    public void HighlightReadButtons()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.HighlightReadButton(true);
        }
    }

    public void UnhighlightReadButtons()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.HighlightReadButton(false);
        }
    }

    public void HighlightShareButtons()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.HighlightShareButton(true);
        }
    }

    public void UnhighlightShareButtons()
    {
        foreach (ArticleGameObject article in _articles)
        {
            article.HighlightShareButton(false);
        }
    }

    private void SubmitTutorialInteractionEvent()
    {
        CustomEvent customEvent = new CustomEvent("TUTORIAL_INTERACTIONS")
        {
            { "Tutorial_Step", _currentChoice.ToString() + "_" + _currentStep.ToString() }
        };
        AnalyticsManager.Instance.SubmitEvent(customEvent);
    }

    private void NextStep()
    {
        SubmitTutorialInteractionEvent();

        if (_lastStepWasLinear && ++_currentStep < _choicesMessageSteps[_currentChoice].steps.Count)
        {
            if (DEBUGGING_TUTORIAL) _currentStepText.SetText("CHOICE: " + _currentChoice + " STEP: " + _currentStep);
            StartCoroutine(ShowMessages());
        } 
        else if (_currentStep < _choicesMessageSteps[_currentChoice].steps.Count)
        {
            StartCoroutine(ShowMessages());
            if (DEBUGGING_TUTORIAL) _currentStepText.SetText("CHOICE: " + _currentChoice + " STEP: " + _currentStep);
        }
        else
        {
            Destroy(gameObject);
            OnTutorialEnd.Invoke();
        }
    }
}
