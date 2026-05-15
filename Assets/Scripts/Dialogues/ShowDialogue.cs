using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShowDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public bool waitForInteraction = false;
    public float waitTimeForNext = 1.5f;

    [Header("Parameters")]
    [SerializeField, Range(0.0f, 1.0f)] float _volume = 0.5f;
    [SerializeField] bool _activateOnEnable = true;

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
        // Avanza al siguiente texto si ha terminado de escribirse y se permite avanzar
        if (_textEnded && ((waitForInteraction && _canGoNext) || !waitForInteraction))
        {
            ShowText();
        }
    }

    // He renombrado QuitSkip a CleanAlphaTags ya que describe mejor su función actual
    public void CleanAlphaTags()
    {
        _text.text = _text.text.Replace(HTML_ALPHA_NULL, string.Empty);
        _text.text = _text.text.Replace(HTML_ALPHA_FULL, string.Empty);
    }

    public void GoToNextText()
    {
        _canGoNext = true;
    }

    public void SkipCurrentText()
    {
        // 1. Si la animación NO ha terminado, mostramos todo el texto de golpe
        if (!_textEnded)
        {
            StopAllCoroutines(); // Detiene WriteNewText y AnimText

            // Construimos el texto completo basándonos en el índice actual
            if (_currentText > 0 && _currentText <= _settings.texts.Count)
            {
                string currentMessage = _settings.texts[_currentText - 1].GetLocalizedString();
                bool isLastMessage = _currentText >= _settings.texts.Count;

                _text.text = currentMessage;
            }

            CleanAlphaTags(); // Limpiamos cualquier etiqueta alpha sobrante de la animación

            _textEnded = true;
            _canGoNext = false; // Evitamos que salte de texto automáticamente si requiere interacción

            PlayEndWritingSound();
            onLineEnded.Invoke();
        }
        // 2. Si la animación YA terminó (o lo acabamos de saltar), avanzamos al siguiente texto
        else
        {
            _canGoNext = true;

            // Si el diálogo no estaba configurado para esperar interacción, forzamos el avance visual aquí
            if (!waitForInteraction)
            {
                ShowText();
            }
        }
    }

    private void OnEnable()
    {
        if(_activateOnEnable) ShowText();
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

        if (_currentText >= _settings.texts.Count)
        {
            CleanAlphaTags();
            EndDialog();
        }
        else
        {
            StartCoroutine(WriteNewText(_settings.texts[_currentText++].GetLocalizedString(), _currentText >= _settings.texts.Count));
        }
    }

    IEnumerator WriteNewText(string text, bool isLastText)
    {
        if (_text.text != string.Empty)
        {
            string originalText = _text.text.Replace(HTML_ALPHA_NULL, "");

            for (int i = 0; i < originalText.Length; ++i)
            {
                string displayText = new string(originalText);
                displayText = displayText.Insert(0, HTML_ALPHA_NULL);
                displayText = displayText.Insert(i + HTML_ALPHA_NULL.Length, HTML_ALPHA_FULL);
                _text.text = displayText;

                if (i % _eraseSoundFrequence == 0) { PlayErasingSound(); }

                yield return new WaitForSeconds(_settings.speed / 4);
            }
        }

        StartCoroutine(AnimText(text, isLastText));
    }

    private void PlayWritingSound() { PlaySound(_writeCharacterEvent); }
    private void PlayErasingSound() { PlaySound(_eraseCharacterEvent); }
    private void PlayEndWritingSound() { PlaySound(_endWritingEvent); }

    private void PlaySound(FMODUnity.EventReference sound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sound, _volume);
    }

    IEnumerator AnimText(string messageToShow, bool isLastMessage)
    {
        _textEnded = false;

        int alphaIndex = 0;
        string displayText = "";

        foreach (char c in messageToShow.ToCharArray())
        {
            alphaIndex++;
            _text.text = messageToShow;

            displayText = _text.text.Insert(alphaIndex, HTML_ALPHA_NULL);
            _text.text = displayText;

            if (alphaIndex % _writeSoundFrequence == 0) { PlayWritingSound(); }

            yield return new WaitForSeconds(_settings.speed);
            if (!isLastMessage && alphaIndex >= messageToShow.Length) break;
        }

        PlayEndWritingSound();
        onLineEnded.Invoke();

        if (!waitForInteraction)
        {
            if (!isLastMessage)
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