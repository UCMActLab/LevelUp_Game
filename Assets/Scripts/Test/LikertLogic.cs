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

    public void SetUp(Question question)
    {
        QuestionLikert questionLikert = question as QuestionLikert;
		if (questionLikert == null)
		{
			Debug.LogError("MCLogic: Question is not of type QuestionLikert!");
			return;
		}
		_questionText.text = question.questionText;

        _leftValue = questionLikert.leftValue;
        _rightValue = questionLikert.rightValue;

        _leftText.text = questionLikert.leftLablel;
        _rightText.text = questionLikert.rightLabel;

		_likertSlider.value = questionLikert.defaultValue;

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
}
