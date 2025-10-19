using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikertLogic : MonoBehaviour
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
        _questionText.text = question.questionText;

        _leftValue = question.leftValue;
        _rightValue = question.rightValue;

        _leftText.text = question.leftLablel;
        _rightText.text = question.rightLabel;

		_likertSlider.value = question.defaultValue;

		SetUpSlider();
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
