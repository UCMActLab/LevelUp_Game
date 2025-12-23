using System.Collections;
using TMPro;
using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private bool _waitForInteraction = false;
    [SerializeField] private float _waitTimeForNext = 1.5f;

    private bool _textEnded;
    private bool _canGoNext = false;

    DialogSettings _settings;

    int _currentText;

    private const string HTML_ALPHA_NULL = "<alpha=#00>";
    private const string HTML_ALPHA_FULL = "<alpha=#FF>";

    public void SetSettings(DialogSettings settings) { this._settings = settings; }

    private void Start()
    {
        _text.text = string.Empty;
        _textEnded = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Update()
    {
        if (_textEnded && ((_waitForInteraction && _canGoNext) || !_waitForInteraction))
        {
            ShowText();
        }
    }

    public void GoToNextText()
    {
        _canGoNext = true;
    }

    private void EndDialog()
    {

    }

    public void ShowText()
    {
        _textEnded = false;
        _canGoNext = false;
        if(_currentText >= _settings.texts.Count)
        {
            EndDialog();
        }
        else
        {
            // añadir algún tipo de animación ??
            StartCoroutine(WriteNewText(_settings.texts[_currentText++].GetLocalizedString(), _currentText >= _settings.texts.Count));
        }
    }

    IEnumerator WriteNewText(string text, bool isLastText)
    {
        if(_text.text != string.Empty)
        {
            //int[] rangeArray = Enumerable.Range(0, _text.text.Length - 1).ToArray();
            //rangeArray.Shuffle();

            string originalText = _text.text.Replace(HTML_ALPHA_NULL, "");

            for(int i = 0; i < originalText.Length; ++i)
            {
                string displayText = new string(originalText);
                displayText = displayText.Insert(0, HTML_ALPHA_NULL);
                displayText = displayText.Insert(i + HTML_ALPHA_NULL.Length, HTML_ALPHA_FULL);
                _text.text = displayText;

                yield return new WaitForSeconds(_settings.speed / 4);
            }
        }

        StartCoroutine(AnimText(text, isLastText));

    }

    IEnumerator AnimText(string messageToShow, bool isLastMessage)
    {
        _textEnded = false;

        int alphaIndex = 0;
        string displayText = "";

        if (!isLastMessage) messageToShow += "...";

        foreach (char c in messageToShow.ToCharArray())
        {
            alphaIndex++;
            _text.text = messageToShow;

            displayText = _text.text.Insert(alphaIndex, HTML_ALPHA_NULL);
            _text.text = displayText;
            
            yield return new WaitForSeconds(_settings.speed);
            if (!isLastMessage && alphaIndex >= messageToShow.Length - 3) break;
        }
        
        if (!_waitForInteraction)
        {
            if(!isLastMessage)
            {
                yield return new WaitForSeconds(_waitTimeForNext / 4);

                for (int i = 0; i < 3; ++i)
                {
                    alphaIndex++;
                    _text.text = messageToShow;

                    displayText = _text.text.Insert(alphaIndex, HTML_ALPHA_NULL);
                    _text.text = displayText;

                    yield return new WaitForSeconds(_waitTimeForNext / 4);
                }
            }
            else
            {
                yield return new WaitForSeconds(_waitTimeForNext);
            }
        }

        _textEnded = true;
    }
}