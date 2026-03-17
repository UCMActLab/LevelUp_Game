using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class EndGameStep : MonoBehaviour
{
    float _startAnimationPosX = 0.0f;
    float _endAnimationPosX = 0.0f;
    float _centerAnimationPosX = 0.0f;

    int _currentStep = 1;

    [SerializeField] float _secondsPerAnim = 1.5f;
    [SerializeField] string _localizationTable = "Translation";
    [SerializeField] string _localizationTableEntryTitle = "ENDGAME/STEP/_/TITLE";
    [SerializeField] string _localizationTableEntryBody = "ENDGAME/STEP/_/BODY";
    [SerializeField] int _maxSteps = 5;

    string[] _localizedTitle;
    string[] _localizedBody;

    [SerializeField] LocalizeStringEvent _titleText = null;
    [SerializeField] GameObject _body = null;
    [SerializeField] LocalizeStringEvent _bodyText = null;
    [SerializeField] Button _nextButton;
    [SerializeField] string _invokedEvent = "WelcomeNext";

    private UnityEvent _onAnimationEnd;

    [SerializeField]
    private UnityEvent _onEnd;

    private void Awake()
    {
        _onAnimationEnd = new UnityEvent();

        _centerAnimationPosX = transform.position.x;
        _startAnimationPosX = transform.position.x + Screen.width;
        _endAnimationPosX = transform.position.x - Screen.width;

        _localizedTitle = _localizationTableEntryTitle.Split('_');
        _localizedBody = _localizationTableEntryBody.Split('_');
    }

    private void Start()
    {
        _nextButton.interactable = false;
        _nextButton.onClick.AddListener(PressedNextButton);
        _nextButton.onClick.AddListener(() => {
            AnalyticsManager.Instance.SubmitEvent(new Unity.Services.Analytics.CustomEvent(_invokedEvent));
        });
        
        if(ChangeText())
        {
            EnterOnFrame();
        }
    }

    public void PressedNextButton()
    {
        _nextButton.interactable = false;
        _onAnimationEnd.AddListener(() =>
        {
            _onAnimationEnd.RemoveAllListeners();
            if (ChangeText())
            {
                EnterOnFrame();
            }
        });
        LeaveFrame();
    }

    private bool ChangeText()
    {
        if(_currentStep > _maxSteps)
        {
            End();
            return false;
        }
        _titleText.StringReference.SetReference(_localizationTable,
            _localizedTitle[0] + _currentStep.ToString() + _localizedTitle[1]);
        _bodyText.StringReference.SetReference(_localizationTable,
            _localizedBody[0] + _currentStep.ToString() + _localizedBody[1]);

        _currentStep++;

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);

        return true;
    }

    private void End() 
    {
        _onEnd?.Invoke();
    }

    private void EnterOnFrame()
    {
        Transform[] tr = { _body.transform.parent };
        StartCoroutine(ReachXPointAnimation(tr, _startAnimationPosX, _centerAnimationPosX));
    }

    private void LeaveFrame()
    {
        Transform[] tr = { _body.transform.parent };
        StartCoroutine(ReachXPointAnimation(tr, _centerAnimationPosX, _endAnimationPosX));
    }

    private IEnumerator ReachXPointAnimation(Transform[] obj, float startingX, float goalX)
    {
        _nextButton.interactable = false;
        
        foreach (Transform tr in obj) 
        {
            tr.position = new Vector2(startingX, tr.position.y);
        }
        Vector2 currentPos = obj[0].position;

        float time = 0.0f;
        while(Mathf.Abs(currentPos.x - goalX) > 0.2f)
        {
            time += Time.deltaTime;
            currentPos = new Vector2(
                Mathf.Lerp(currentPos.x, goalX, time / _secondsPerAnim),
                currentPos.y);
            
            foreach (Transform tr in obj)
            {
                tr.position = new Vector2(currentPos.x, tr.position.y);
            }

            yield return new WaitForEndOfFrame();
        }

        foreach (Transform tr in obj)
        {
            tr.position = new Vector2(goalX, tr.position.y);
        }

        _nextButton.interactable = true;

        _onAnimationEnd.Invoke();
    }
}
