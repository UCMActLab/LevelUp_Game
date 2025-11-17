using DA_Assets.Extensions;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointsMenu : MonoBehaviour
{
    [Header("Scores Object")]
    [SerializeField] private GameObject _readArticlesObject = null;
    [SerializeField] private GameObject _trueArticlesObject = null;
    [SerializeField] private GameObject _falseArticlesObject = null;

    [Header("Sliders")]
    [SerializeField] private Slider _generalSlider = null;
    [SerializeField] private Slider _readArticlesSlider = null;
    [SerializeField] private Slider _trueArticlesSlider = null;
    [SerializeField] private Slider _falseArticlesSlider = null;

    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI _readArticlesText = null;
    [SerializeField] private TextMeshProUGUI _trueArticlesText = null;
    [SerializeField] private TextMeshProUGUI _falseArticlesText = null;

    [Header("Image")]
    [SerializeField] private Image _medal = null;
    private Sprite _newMedal = null;

    [Header("Button")]
    [SerializeField] private Button _okButton = null;
    [SerializeField] private float _enableOkButtonAfterSeconds = 3.0f;
    

    private Animator _animator;
    private ScoreManager _scoreManager;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _generalSlider.value = 0;
    }

    private void OnEnable()
    {
        _okButton.gameObject.SetActive(false);
        StartCoroutine(EnableAfterSeconds(_okButton.gameObject, _enableOkButtonAfterSeconds));
    }

    private void OnDisable()
    {
        _okButton.gameObject.SetActive(false);
        if(_newMedal != null) ChangeMedalDuringAnimation();
    }

    IEnumerator EnableAfterSeconds(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
    }

    public void ShowScore(int numArticles, int articlesRead, int articlesTrue, int totalArticlesTrue, int articlesFalseShared, int totalArticlesFalse)
    {
        _readArticlesSlider.value = 0.0f;
        _falseArticlesSlider.value = 0.0f;
        _trueArticlesSlider.value = 0.0f;

        // Aquí una función de SetSlider que haga la animación y todo esto jej
        SetValues(_readArticlesObject, _readArticlesSlider, _readArticlesText, articlesRead, numArticles, 1.25f);

        // Aquí una función de SetSlider que haga la animación y todo esto jej
        SetValues(_trueArticlesObject, _trueArticlesSlider, _trueArticlesText, articlesTrue, totalArticlesTrue, 1.25f);

        // Aquí una función de SetSlider que haga la animación y todo esto jej
        SetValues(_falseArticlesObject, _falseArticlesSlider, _falseArticlesText, articlesFalseShared, totalArticlesFalse, 1.25f);
    }

    public void SetTotalScore(int maxScore)
    {
        _generalSlider.maxValue = maxScore;
    }

    public void ChangeMedal(Sprite newSprite)
    {
        _newMedal = newSprite;
        _animator.SetTrigger("ChangeMedal");
    }

    public void ChangeMedalDuringAnimation()
    {
        _medal.sprite = _newMedal;
        _newMedal = null;
    }

    private void SetValues(GameObject targetObject, Slider target, TextMeshProUGUI text, int value, int maxValue, float time)
    {
        if (maxValue == 0)
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
    }
}
