using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderFillAnimation : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField, Range(0.1f, 5.0f)] private float _speed;
    [SerializeField] private GameObject _handle;

    private void Start()
    {
        if(_handle == null)
        {
            _handle = _slider.handleRect.gameObject;
        }
        _handle.SetActive(false);
    }

    public void SetNewValue(float newValue)
    {
        StartCoroutine(Animation(newValue));
    }

    IEnumerator Animation(float goalValue)
    {
        float currentValue = _slider.value;

        _handle.SetActive(true);
        while (Mathf.Abs(currentValue - goalValue) > 0.05f)
        {
            currentValue = Mathf.Lerp(currentValue, goalValue, _speed * Time.deltaTime);
            _slider.value = currentValue;
            yield return new WaitForEndOfFrame();
        }
        _handle.SetActive(false);
        _slider.value = goalValue;
    }
}
