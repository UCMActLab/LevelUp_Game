using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChangeBetweenTexts : MonoBehaviour
{
    [SerializeField] string[] _texts = null;
    [SerializeField] float _waitTime = 1.2f;

    TextMeshProUGUI _text = null;

    int _currentIndex = 0;

    bool _isPlaying = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _currentIndex = 0;
        _text = GetComponent<TextMeshProUGUI>();
        _text.SetText(_texts[_currentIndex]);
    }

    private void OnEnable() { _isPlaying = true; StopAllCoroutines(); StartCoroutine(ChangeText()); } 

    private void OnDisable() { _isPlaying = false; }

    IEnumerator ChangeText()
    {
        while (_isPlaying)
        {
            yield return new WaitForSeconds(_waitTime);
            _currentIndex++;
            if (_currentIndex >= _texts.Length) _currentIndex = 0;
            _text.SetText(_texts[_currentIndex]);
        }
    }
}
