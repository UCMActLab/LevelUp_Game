using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    [Header("Scores Object")]
    [SerializeField] private GameObject _readArticlesObject = null;
    [SerializeField] private GameObject _trueArticlesObject = null;
    [SerializeField] private GameObject _rightThemesObject = null;
    [SerializeField] private GameObject _questionsObject = null;
    [SerializeField] private GameObject _falseArticlesObject = null;

    [Header("Sliders")]
    [SerializeField] private Slider _readArticlesSlider = null;
    [SerializeField] private Slider _trueArticlesSlider = null;
    [SerializeField] private Slider _rightThemesSlider = null;
    [SerializeField] private Slider _questionsSlider = null;
    [SerializeField] private Slider _falseArticlesSlider = null;

    [Header("Toggles")]
    [SerializeField] private TextMeshProUGUI _shareWithText = null;
    [SerializeField] private GameObject _familyToggleGameObject = null;
    [SerializeField] private GameObject _friendsToggleGameObject = null;
    [SerializeField] private GameObject _neighboursToggleGameObject = null;

    Toggle _familyToggle = null;
    Toggle _friendsToggle = null;
    Toggle _neighboursToggle = null;

    TextMeshProUGUI _familySharedText = null;
    TextMeshProUGUI _friendsSharedText = null;
    TextMeshProUGUI _neighboursSharedText = null;

    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI _readArticlesText = null;
    [SerializeField] private TextMeshProUGUI _trueArticlesText = null;
    [SerializeField] private TextMeshProUGUI _rightThemesText = null;
    [SerializeField] private TextMeshProUGUI _questionsText = null;
    [SerializeField] private TextMeshProUGUI _falseArticlesText = null;

    [Header("Button")]
    [SerializeField] private Button _okButton = null;
    
    [Header("Feedback")]
    [SerializeField] private GameObject _feedback = null;
    [SerializeField] private GameAssistant _assistant = null;
    private string _feedbackString = string.Empty;

    private void OnEnable()
    {
        _familyToggle = _familyToggleGameObject.GetComponentInChildren<Toggle>();
        _friendsToggle = _friendsToggleGameObject.GetComponentInChildren<Toggle>();
        _neighboursToggle = _neighboursToggleGameObject.GetComponentInChildren<Toggle>();

        _familySharedText = _familyToggleGameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        _friendsSharedText = _friendsToggleGameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        _neighboursSharedText = _neighboursToggleGameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];

        ActivateOKButton(true);
    }

    private void OnDisable()
    {
        ActivateOKButton(false);
    }

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
        SetValues(_falseArticlesObject, _falseArticlesSlider, _falseArticlesText, quest.done.falseArticlesSkipped, quest.toDo.falseArticlesToSkip, 1.25f);

        bool groups = quest.thereAreGroups && !quest.groupsHaveTopics;
        bool topics = quest.thereAreGroups && quest.groupsHaveTopics;

        if (groups)
        {
            _rightThemesObject.SetActive(false);
            if (quest.toDo.toShareWithFamily > 0)
            {
                SetToggle(_familyToggleGameObject, _familyToggle, _familySharedText
                    , quest.done.sharedWithFamily, 
                    quest.toDo.toShareWithFamily);
            }
            else
            {
                _familyToggleGameObject.SetActive(false);
            }
            if (quest.toDo.toShareWithFriends > 0)
            {
                SetToggle(_friendsToggleGameObject, _friendsToggle, _friendsSharedText,
                    quest.done.sharedWithFriends, quest.toDo.toShareWithFriends);
            }
            else
            {
                _friendsToggleGameObject.SetActive(false);
            }
            if (quest.toDo.toShareWithNeighbours > 0)
            {
                SetToggle(_neighboursToggleGameObject, _neighboursToggle, _neighboursSharedText,
                    quest.done.sharedWithNeighbours, quest.toDo.toShareWithNeighbours);
            }
            else
            {
                
                _neighboursToggleGameObject.SetActive(false);
            }
        }
        else if (topics)
        {
            SetValues(_rightThemesObject, _rightThemesSlider, _rightThemesText, 
                quest.done.themesCorrectlyAddressed, quest.toDo.articlesToIdentify, 1.25f);
        }
        else
        {
            _familyToggleGameObject.SetActive(false);
            _friendsToggleGameObject.SetActive(false);
            _neighboursToggleGameObject.SetActive(false);
        }


        _shareWithText.gameObject.SetActive(groups);
        _rightThemesObject.SetActive(topics);

        SetValues(_questionsObject, _questionsSlider, _questionsText, -1, -1, 1.25f);

        if (!badFalseSharing && !badTrueSharing && !badReading)
        {
            feedback = TranslationManager.Instance.GetLocalizedStringValue("Translation", "SCORE/FEEDBACK/GOOD_JOB");
        }

        _feedbackString = feedback;
    }

    private void SetToggle(GameObject toggleHolder, Toggle toggle, TextMeshProUGUI text, int value, int maxValue)
    {
        toggleHolder.SetActive(true);
        toggle.isOn = maxValue <= value;

        text.text = string.Format("{0}/{1}", value, maxValue);
    }

    public void ShowFeedback()
    {
        string[] feedback = _feedbackString.Split('\n');
        _assistant.ShowMessages(feedback, LevelManager.Instance.ShowMessagesEndLevel);
    }

    public void RebuildLayouts()
    {
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(child as RectTransform);
        }
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
        float initialValue = 0;
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
        }
    }
}
