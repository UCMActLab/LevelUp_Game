using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] SliderFillAnimation _slider;
    public void UpdateValue()
    {
        _slider.SetNewValue(ScoreManager.Instance.Score);
    }
}
