using UnityEngine;

public class FloatUIStable : MonoBehaviour
{
    [SerializeField]
    public float amplitude = 100f;
    [SerializeField]
    public float speed = 3f;

    RectTransform rt;
    Vector2 basePos;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        basePos = rt.anchoredPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.unscaledTime * speed) * amplitude;
        rt.anchoredPosition = basePos + new Vector2(0, y);
    }
}