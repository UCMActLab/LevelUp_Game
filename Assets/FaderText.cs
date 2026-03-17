using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FaderText : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _startingValue;

    public UnityEvent OnFadeEnd;

    TextMeshProUGUI _text;

    public float Value { get { return _text.color.a; } }

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        ChangeAlpha(_startingValue);
    }

    private void ChangeAlpha(float newAlpha)
    {
        _text.color = GetGoalColor(newAlpha);
    }

    private Color GetGoalColor(float alpha)
    {
        return new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
    }

    public void StartFade(float time, float initialValue, float goalValue)
    {
        StartCoroutine(Fade(time, initialValue, goalValue));
    }

    IEnumerator Fade(float time, float initialValue, float goalValue)
    {
        float timer = 0.0f;

        float currentAlpha = initialValue;

        while (timer < time)
        {
            currentAlpha = Mathf.Lerp(initialValue, goalValue, timer / time);

            ChangeAlpha(currentAlpha);

            timer += Time.deltaTime;
            yield return new WaitForNextFrameUnit();
        }

        ChangeAlpha(goalValue);

        OnFadeEnd?.Invoke();
    }
}
