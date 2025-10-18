using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikertLogic : MonoBehaviour
{
    [SerializeField]
    private int _minValue;

    [SerializeField]
    private int _maxValue;

    [SerializeField]
    private TMP_Text _sliderHandleText;

    [SerializeField]
    private Slider _likertSlider;

	private void Start()
	{
		SetUp();
	}

	public void SetUp()
    {
        _likertSlider.minValue = _minValue;
        _likertSlider.maxValue = _maxValue;
        UpdateSliderHandle();
    }

    public void UpdateSliderHandle()
    {
        _sliderHandleText.text = _likertSlider.value.ToString();
	}
}
