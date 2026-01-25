using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DebugMenu : Singleton<DebugMenu>
{
    protected override void Awake()
    {
        base.Awake();
        _debugTexts = new Dictionary<TextMeshProUGUI, GameObject>();
#if !UNITY_EDITOR
        Destroy(gameObject);  
#endif
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.D)) {
            ToggleVisibility();
        }
    }

    private Image _image;
    bool _isVisible = false;
    Dictionary<TextMeshProUGUI, GameObject> _debugTexts = null;

    private void Start()
    {
        _image = GetComponent<Image>();
        _isVisible = false;
        SetVisibility(_isVisible);
    }

    public void SetVisibility(bool isVisible)
    {
        _isVisible = isVisible;
        _image.enabled = _isVisible;

        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(_isVisible);
        }
    }

    public void ToggleVisibility()
    {
        SetVisibility(!_isVisible);
    }

    [SerializeField]
    private GameObject _baseTextObject = null;

    public TextMeshProUGUI AddDebugText(string name)
    {
        GameObject newChild = Instantiate(_baseTextObject, transform);
        TextMeshProUGUI[] newText = newChild.GetComponentsInChildren<TextMeshProUGUI>();
        newText[0].SetText(name+": "); newText[1].SetText(string.Empty);

        _debugTexts.Add(newText[1], newChild);

        newChild.gameObject.SetActive(_isVisible);

        return newText[1];
    }
    
    public void RemoveText(TextMeshProUGUI text)
    {
        GameObject value = null;
        _debugTexts.TryGetValue(text, out value);
        if (value != null) Destroy(value);
    }
}
