using UnityEngine;

public class SetGameProgressSliderToLevelManager : MonoBehaviour
{
    void Start()
    {
        LevelManager.Instance.SetGameProgressSlider(GetComponent<UnityEngine.UI.Slider>());
        Destroy(this);
    }
}
