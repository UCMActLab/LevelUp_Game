using UnityEngine;
using UnityEngine.UI;

public class ImageZoomPanel : MonoBehaviour
{
    [SerializeField] private Image _image;

    [Header("Configuración de Sensibilidad")]
    [SerializeField] private float _mouseZoomSpeed = 50f;
    [SerializeField] private float _touchZoomSpeed = 0.5f;
    [SerializeField] private float _buttonZoomStep = 100f;
    [SerializeField] private float _minSizeThreshold = 100f; // Evita que la imagen se encoja a valores negativos

    private Vector3 _lastMousePosition;
    private RectTransform _parentRectTransform;

    private void Start()
    {
        // Guardamos la referencia al RectTransform del padre para saber las dimensiones de la pantalla/panel
        _parentRectTransform = _image.rectTransform.parent as RectTransform;

        // Forzamos el reset inicial
        ResetImage();
    }

    private void Update()
    {
        // Prioridad 1: Gestos en Pantalla Táctil (Móvil)
        if (Input.touchCount > 0)
        {
            HandleTouchInputs();
        }
        // Prioridad 2: Inputs de Ratón (PC)
        else
        {
            HandleMouseInputs();
        }
    }

    private void HandleTouchInputs()
    {
        // 1 Dedo: Desplazar / Mover imagen
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                _image.rectTransform.localPosition += (Vector3)touch.deltaPosition;
            }
        }
        // 2 Dedos: Pinch to Zoom (Ampliar / Encoger)
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Posiciones de los dedos en el frame anterior
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            // Distancias entre dedos (actual vs anterior)
            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ChangeImageSize(difference * _touchZoomSpeed);
        }
    }

    private void HandleMouseInputs()
    {
        // Rueda del ratón: Ampliar / Encoger
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            ChangeImageSize(scroll * _mouseZoomSpeed * 10f);
        }

        // Click izquierdo + arrastrar: Mover imagen
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _image.rectTransform.localPosition += delta;
            _lastMousePosition = Input.mousePosition;
        }
    }

    private void ChangeImageSize(float amount)
    {
        Vector2 newSize = _image.rectTransform.sizeDelta + new Vector2(amount, amount);

        // Controlamos que no se reduzca más allá del límite mínimo para evitar bugs visuales
        if (newSize.x > _minSizeThreshold && newSize.y > _minSizeThreshold)
        {
            _image.rectTransform.sizeDelta = newSize;
        }
    }

    public void ZoomIn() => ChangeImageSize(_buttonZoomStep);
    public void ZoomOut() => ChangeImageSize(-_buttonZoomStep);

    public void ResetImage()
    {
        // 1. Forzamos los anclajes y el pivote rigurosamente al CENTRO
        _image.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _image.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _image.rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // 2. Llevamos la imagen al centro exacto del panel (0, 0, 0)
        _image.rectTransform.localPosition = Vector3.zero;
        _image.rectTransform.localScale = Vector3.one;

        // 3. Simulamos el "Stretch" aplicando el tamaño actual del padre a nuestro sizeDelta
        if (_parentRectTransform != null)
        {
            _image.rectTransform.sizeDelta = _parentRectTransform.rect.size;
        }
    }

    public void ShowImageZoomPanel(Image imageToShow)
    {
        ResetImage();

        _image.sprite = imageToShow.sprite;

        gameObject.SetActive(true);
    }
}
