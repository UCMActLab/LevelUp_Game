using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AddPointsOnHit : MonoBehaviour
{
    Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    float timer = 0.0f;
    float maxTime = 1.5f;

    bool startedTimer = false;
    IEnumerator CalculateScoreStateTimer()
    {
        startedTimer = true;
        ScoreManager.Instance.CanContinue = false;
        timer = 0.0f;
        yield return new WaitUntil(() => timer >= maxTime);
        // ScoreManager.Instance.CalculateScoreState();
        ScoreManager.Instance.CanContinue = true;
        startedTimer = false;
    }

    private void Update()
    {
        if (startedTimer) timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointsOnHit go = collision.GetComponent<PointsOnHit>();
        if (go == null) go = collision.GetComponentInParent<PointsOnHit>();

        if (go != null)
        {
            _slider.value += go.scoreOnHit;
            if (!startedTimer) StartCoroutine(CalculateScoreStateTimer());
            else timer = 0.0f;
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
