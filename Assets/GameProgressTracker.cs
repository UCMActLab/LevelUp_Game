using UnityEngine;

public class GameProgressTracker : MonoBehaviour
{
    [SerializeField] SliderFillAnimation _slider;
    public void UpdateValue()
    {
        _slider.SetNewValue(LevelManager.Instance.CurrentLevel);
    }
}
