using UnityEngine;

public class ScoreManager_SetGeneralSlider : MonoBehaviour
{
    void Start()
    {
        ScoreManager.Instance.SetGeneralScoreSlider(GetComponent<UnityEngine.UI.Slider>());
        Destroy(this);
    }
}
