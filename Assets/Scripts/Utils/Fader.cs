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

    Image _myImage = null;

    Image _image { get { 
            if (_myImage == null)
            {
                _myImage = GetComponent<Image>();
            }
            return _myImage; } }

    public float Value { get { return _image.color.a; } }

    void Start()
    {
        _myImage = GetComponent<Image>();

        ChangeAlpha(_startingValue);
    }

    private void ChangeAlpha(float newAlpha)
    {
        if (newAlpha == 0.0f) 
            _image.raycastTarget = false;
        _image.color = GetGoalColor(newAlpha);
    }

    private Color GetGoalColor(float alpha)
    {
        return new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
    } 

    public void StartFade(float time, float initialValue, float goalValue, bool blockInteraction = false)
    {
        _image.raycastTarget = blockInteraction;
        StartCoroutine(Fade(time, initialValue, goalValue));
    }

    IEnumerator Fade(float time, float initialValue, float goalValue)
    {
        float timer = 0.0f;

        float currentAlpha = initialValue;

        while(timer < time)
        {
            timer += Time.deltaTime;
            currentAlpha = Mathf.Lerp(initialValue, goalValue, timer / time);

            ChangeAlpha(currentAlpha);

            yield return new WaitForNextFrameUnit();
        }

        ChangeAlpha(goalValue);

        OnFadeEnd?.Invoke();
    }
}
