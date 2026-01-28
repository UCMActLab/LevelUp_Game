using UnityEngine;
using UnityEngine.UI;

public class AddPointsOnHit : MonoBehaviour
{
    Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlyToPoint go = collision.GetComponent<FlyToPoint>();
        if (go == null) go = collision.GetComponentInParent<FlyToPoint>();

        if (go != null)
        {
            _slider.value += 1;
            ScoreManager.Instance.CalculateScoreState();
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
