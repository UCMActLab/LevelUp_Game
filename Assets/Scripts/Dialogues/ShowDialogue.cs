using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShowDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public bool waitForInteraction = false;
    public float waitTimeForNext = 1.5f;

    [Header("Paremeters")]
    [SerializeField, Range(0.0f, 1.0f)] float _volume = 0.5f;

    [Header("Writing Sounds")]
    [SerializeField] FMODUnity.EventReference _writeCharacterEvent;
    [SerializeField] int _writeSoundFrequence = 2;

    [SerializeField] FMODUnity.EventReference _endWritingEvent;

    [Header("Erase Sounds")]
    [SerializeField] FMODUnity.EventReference _eraseCharacterEvent;
    [SerializeField] int _eraseSoundFrequence = 4;

    [Header("Unity Events")]
    public UnityEvent onDialogueEnd = new UnityEvent();
    public UnityEvent onLineEnded = new UnityEvent();

    private bool _textEnded;
    private bool _canGoNext = false;
    private bool _skipCurrent = false;

    DialogSettings _settings;

    int _currentText;

    private const string HTML_ALPHA_NULL = "<alpha=#00>";
    private const string HTML_ALPHA_FULL = "<alpha=#FF>";

    public void SetSettings(DialogSettings settings) { this._settings = settings; _currentText = 0; }

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
        if ((_textEnded && ((waitForInteraction && _canGoNext) || !waitForInteraction)) || _skipCurrent)
        {
            StopAllCoroutines();
            ShowText();
        }
    }

    public void QuitSkip()
    {
        _skipCurrent = false;
        _text.text = _text.text.Replace(HTML_ALPHA_NULL, string.Empty);
        _text.text = _text.text.Replace(HTML_ALPHA_FULL, string.Empty);
    }

    public void GoToNextText()
    {
        _canGoNext = true;
    }

    public void SkipCurrentText()
    {
        _skipCurrent = true;
    }

    private void EndDialog()
    {
        Debug.Log("Dialogue End");
        onDialogueEnd.Invoke();
    }

    public void ShowText()
    {
        StopAllCoroutines();
        _textEnded = false;
        _canGoNext = false;
        if(_currentText >= _settings.texts.Count)
        {
            QuitSkip();
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
        if(_text.text != string.Empty && !_skipCurrent)
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

                if (i % _eraseSoundFrequence == 0) { PlayErasingSound(); }

                yield return new WaitForSeconds(_settings.speed / 4);
            }
        }

        _skipCurrent = false;
        StartCoroutine(AnimText(text, isLastText));

    }

    private void PlayWritingSound()
    {
        PlaySound(_writeCharacterEvent);
    }

    private void PlayErasingSound()
    {
        PlaySound(_eraseCharacterEvent);
    }

    private void PlayEndWritingSound()
    {
        PlaySound(_endWritingEvent);
    }

    private void PlaySound(FMODUnity.EventReference sound)
    {
        
        FMODUnity.RuntimeManager.PlayOneShot(sound, 0.5f);
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

            if (alphaIndex % _writeSoundFrequence == 0) { PlayWritingSound(); }

            yield return new WaitForSeconds(_settings.speed);
            if (!isLastMessage && alphaIndex >= messageToShow.Length - 3) break;
        }

        PlayEndWritingSound();
        onLineEnded.Invoke();

        if (!waitForInteraction)
        {
            if(!isLastMessage)
            {
                yield return new WaitForSeconds(waitTimeForNext / 4);

                for (int i = 0; i < 3; ++i)
                {
                    alphaIndex++;
                    _text.text = messageToShow;

                    displayText = _text.text.Insert(alphaIndex, HTML_ALPHA_NULL);
                    _text.text = displayText;

                    yield return new WaitForSeconds(waitTimeForNext / 4);
                }
            }
            else
            {
                yield return new WaitForSeconds(waitTimeForNext);
            }
        }

        _textEnded = true;
    }
}