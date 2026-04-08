using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    [Header("Scores Object")]
    [SerializeField] private GameObject _readArticlesObject = null;
    [SerializeField] private GameObject _trueArticlesObject = null;
    [SerializeField] private GameObject _questionsObject = null;
    [SerializeField] private GameObject _falseArticlesObject = null;

    [Header("Sliders")]
    [SerializeField] private Slider _generalSlider = null;
    [SerializeField] private Slider _readArticlesSlider = null;
    [SerializeField] private Slider _trueArticlesSlider = null;
    [SerializeField] private Slider _questionsSlider = null;
    [SerializeField] private Slider _falseArticlesSlider = null;

    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI _readArticlesText = null;
    [SerializeField] private TextMeshProUGUI _trueArticlesText = null;
    [SerializeField] private TextMeshProUGUI _questionsText = null;
    [SerializeField] private TextMeshProUGUI _falseArticlesText = null;

    [Header("Image")]
    [SerializeField] private Image _medal = null;
    private Sprite _newMedal = null;

    [Header("Button")]
    [SerializeField] private Button _okButton = null;
    
    [Header("Feedback")]
    [SerializeField] private GameObject _feedback = null;
    [SerializeField] private GameAssistant _assistant = null;
    private TextMeshProUGUI _feedbackText = null;
    private string _feedbackString = string.Empty;

    private Animator _animator;

    private bool _medalAnimationEnded = false;

    bool _animationRunning = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _generalSlider.value = 0;

        _feedbackText = _feedback.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _medalAnimationEnded = true;
        ActivateOKButton(false);
        StartCoroutine(ActivateOKButtonOnEnd());
    }

    private IEnumerator ActivateOKButtonOnEnd()
    {
        yield return new WaitUntil(() => _generalSlider.value == ScoreManager.Instance.Score && _medalAnimationEnded && ScoreManager.Instance.CanContinue);
        ActivateOKButton(true);
    }

    private void OnDisable()
    {
        ActivateOKButton(false);
        if(_newMedal != null) ChangeMedalDuringAnimation();
    }

    //public void ShowScore(LevelScore score)
    //{
    //    _readArticlesSlider.value = 0.0f;
    //    _falseArticlesSlider.value = 0.0f;
    //    _trueArticlesSlider.value = 0.0f;
    //    _questionsSlider.value = 0.0f;

    //    string feedback = string.Empty;

    //    bool badReading = score.readArticles < score.readableArticles;
    //    bool badTrueSharing = score.trueArticlesShared < score.trueArticles;
    //    bool badFalseSharing = score.falseArticlesShared > 0;

    //    if (badReading)
    //    {
    //        feedback += TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/READ");
    //    }
    //    SetValues(_readArticlesObject, _readArticlesSlider, _readArticlesText, score.readArticles, score.readableArticles, 1.25f);

    //    if (badTrueSharing)
    //    {
    //        if (feedback != string.Empty) feedback += '\n';
    //        feedback += TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/SHARE_TRUE");
    //    }
    //    SetValues(_trueArticlesObject, _trueArticlesSlider, _trueArticlesText, score.trueArticlesShared, score.trueArticles, 1.25f);

    //    if (badFalseSharing)
    //    {
    //        if (feedback != string.Empty) feedback += '\n';
    //        feedback += TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/SHARE_FALSE");
    //    }
    //    SetValues(_falseArticlesObject, _falseArticlesSlider, _falseArticlesText, score.falseArticlesShared, score.falseArticles, 1.25f);

    //    SetValues(_questionsObject, _questionsSlider, _questionsText, score.questionsRight, score.totalQuestions, 1.25f);

    //    if(!badFalseSharing  && !badTrueSharing && !badReading)
    //    {
    //        feedback = TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/GOOD_JOB");
    //    }

    //    _feedbackString = feedback;
    //}

    public void ShowScore(Quest quest)
    {
        _readArticlesSlider.value = 0.0f;
        _falseArticlesSlider.value = 0.0f;
        _trueArticlesSlider.value = 0.0f;
        _questionsSlider.value = 0.0f;

        string feedback = string.Empty;

        bool badReading = quest.done.readedArticles < quest.toDo.toRead;
        bool badTrueSharing = quest.done.identifiedArticles < quest.toDo.articlesToIdentify;
        bool badFalseSharing = quest.done.falseArticlesShared > 0;

        if (badReading)
        {
            feedback += TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/READ");
        }
        _readArticlesObject.SetActive(false);
        //SetValues(_readArticlesObject, _readArticlesSlider, _readArticlesText, quest.done.readedArticles, quest.toDo.toRead, 1.25f);

        if (badTrueSharing)
        {
            if (feedback != string.Empty) feedback += '\n';
            feedback += TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/SHARE_TRUE");
        }
        SetValues(_trueArticlesObject, _trueArticlesSlider, _trueArticlesText, quest.done.identifiedArticles, quest.toDo.articlesToIdentify, 1.25f);

        if (badFalseSharing)
        {
            if (feedback != string.Empty) feedback += '\n';
            feedback += TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/SHARE_FALSE");
        }
        SetValues(_falseArticlesObject, _falseArticlesSlider, _falseArticlesText, quest.done.falseArticlesShared, quest.toDo.falseArticlesToSkip, 1.25f);

        // SetValues(_questionsObject, _questionsSlider, _questionsText, score.questionsRight, score.totalQuestions, 1.25f);
        SetValues(_questionsObject, _questionsSlider, _questionsText, -1, -1, 1.25f);

        if (!badFalseSharing && !badTrueSharing && !badReading)
        {
            feedback = TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/GOOD_JOB");
        }

        _feedbackString = feedback;
    }

    public void ShowFeedback()
    {
        string[] feedback = _feedbackString.Split('\n');
        _assistant.ShowMessagesOneShot(feedback, LevelManager.Instance.ShowNextArticle);
    }

    public void RebuildLayouts()
    {
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child as RectTransform);
        }
    }

    public void SetTotalScore(int maxScore)
    {
        _generalSlider.maxValue = maxScore;
    }

    public void ChangeMedal(Sprite newSprite)
    {
        _newMedal = newSprite;
        if (!_medalAnimationEnded) return;

        _medalAnimationEnded = false;
        _animator.SetTrigger("ChangeMedal");
    }

    public void ChangeMedalDuringAnimation()
    {
        _medal.sprite = _newMedal;
        _newMedal = null;
    }

    public void MedalAnimationEnded()
    {
        _medalAnimationEnded = true;
    }

    public void ActivateOKButton(bool activate)
    {
        _okButton.gameObject.SetActive(activate);
    }

    private void SetValues(GameObject targetObject, Slider target, TextMeshProUGUI text, int value, int maxValue, float time)
    {
        if (maxValue <= 0)
        {
            targetObject.SetActive(false);
            return;
        }

        targetObject.SetActive(true);
        StartCoroutine(AnimSliderValue(targetObject, target, value, maxValue, time));
        text.SetText(string.Format("{0}/{1}", value, maxValue));
    }

    IEnumerator AnimSliderValue(GameObject targetObject, Slider target, int targetValue, int maxValue, float time)
    {
        if (_animationRunning) 
            yield return new WaitUntil(() => !_animationRunning);

        _animationRunning = true;


        float initialValue = target.value;
        if (targetValue != initialValue)
        {
            float currentValue = initialValue;
            target.maxValue = maxValue;

            float currentTime = 0.0f;

            while(currentTime < time)
            {
                yield return new WaitForEndOfFrame();

                currentTime += Time.deltaTime;
                currentValue = Mathf.Lerp(initialValue, targetValue, currentTime / time);

                target.value = currentValue;
            }

            target.value = targetValue;

            AddPointsFeedback feedback = targetObject.GetComponent<AddPointsFeedback>();
            if(feedback) feedback.AddPoints((int)target.value);
        }
        _animationRunning = false;
    }
}
