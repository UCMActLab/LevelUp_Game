using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikertLogic : MonoBehaviour, IQuestionLogic
{
    [SerializeField]
    private int _leftValue;

    [SerializeField]
    private int _rightValue;

	[SerializeField]
	private TMP_Text _questionText;

	[SerializeField]
    private TMP_Text _sliderHandleText;

	[SerializeField]
	private TMP_Text _leftText;
    [SerializeField]
	private TMP_Text _rightText;

	[SerializeField]
    private Slider _likertSlider;

	private QuestionLikert _questionLikert;

    public void SetUp(Question question)
    {
        _questionLikert = question as QuestionLikert;
		if (_questionLikert == null)
		{
			Debug.LogError("MCLogic: Question is not of type QuestionLikert!");
			return;
		}
        _questionText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionLikert.questionText);

		_leftValue = _questionLikert.leftValue;
        _rightValue = _questionLikert.rightValue;

		_leftText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionLikert.leftLablel);
		_rightText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionLikert.rightLabel);

		_likertSlider.value = _questionLikert.defaultValue;

		SetUpSlider();
    }

	public EvaluationResult GetResults()
	{
        EvaluationResult result = new EvaluationResult();
        result.resultType = EvaluationResult.ResultType.INT;
        result.resultScore = (int)_likertSlider.value;
		return result;
	}

	private void SetUpSlider()
    {
        _likertSlider.minValue = _leftValue;
        _likertSlider.maxValue = _rightValue;
        UpdateSliderHandle();
    }

    public void UpdateSliderHandle()
    {
        _sliderHandleText.text = _likertSlider.value.ToString();
	}

	public void LockQuestion()
	{
		_likertSlider.interactable = false;
	}

	public bool IsCorrect()
	{
		return _questionLikert.correctValue == (int)_likertSlider.value;
	}

	public string GetCorrectResponse()
	{
		return _questionLikert.correctValue.ToString();
	}
}
