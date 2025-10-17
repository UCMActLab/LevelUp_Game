using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI)), ExecuteInEditMode]
public class TranslateLabel : MonoBehaviour
{
    TextMeshProUGUI _text = null;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void ChangeKey(Object obj)
    {
        if(obj == _text)
        {
            string newText = 
                TranslationManager.Instance.GetValueForKey(_text.text);
            _text.text = newText;
        }
    }
}
