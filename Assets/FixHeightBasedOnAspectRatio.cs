using UnityEngine;
using UnityEngine.UI;

public class FixHeightBasedOnAspectRatio : MonoBehaviour
{
    [SerializeField] Image _image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_image == null) _image = GetComponent<Image>();
        float aspectRatio = (float)_image.mainTexture.width / _image.mainTexture.height;
        RectTransform myTransform = transform as RectTransform;
        myTransform.sizeDelta = new Vector2(myTransform.sizeDelta.x, myTransform.sizeDelta.x / aspectRatio);
    }
}
