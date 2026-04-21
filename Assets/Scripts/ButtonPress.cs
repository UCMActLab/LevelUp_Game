using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private float pressedScale = 0.9f;
    
    [SerializeField] 
    private float speed = 12f;

    Vector3 originalScale;
    bool isPressed;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        Vector3 target = isPressed ? originalScale * pressedScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * speed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
