using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    [SerializeField, Range(0,1)] float _startingValue;

    public UnityEvent OnFadeEnd;

    Image _image;

    void Start()
    {
        _image = GetComponent<Image>();

        ChangeAlpha(_startingValue);
    }

    private void ChangeAlpha(float newAlpha)
    {
        _image.color = GetGoalColor(newAlpha);
    }

    private Color GetGoalColor(float alpha)
    {
        return new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
    } 

    public void StartFade(float time, float initialValue, float goalValue)
    {
        StartCoroutine(Fade(time, initialValue, goalValue));
    }

    IEnumerator Fade(float time, float initialValue, float goalValue)
    {
        float timer = 0.0f;

        float currentAlpha = initialValue;

        while(timer < time)
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
