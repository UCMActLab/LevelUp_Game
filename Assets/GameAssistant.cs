using AYellowpaper.SerializedCollections;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum GameAssistantState
{
    NORMAL = 0, // Sonríe feliz viendo que el jugador ha hecho bien 1 nivel (este es el estado base)
    BAD = 1, // Detecta que el jugador no está haciéndolo muy bien y se pone sad. Aquí es cuándo debemos lanzar la interrogación
    GOOD = 2 // Está súper contento porque ve que el jugador lleva haciéndolo bien 2 o más niveles
}

public class GameAssistant : MonoBehaviour
{
    /// Te suelta la chapa: El avatar detecta que el jugador NO está interactuando con la pantalla. 
    ///     Esto quiere decir que el jugador no ha tocado ningún botón y ni siquiera está haciendo scroll.
    ///     En caso de SÍ estar haciendo scroll, podemos poner más tiempo para decirle al juego que el jugador todavía
    ///     no está perdido. Si finalmente pasa X + Y tiempo y solo ha hecho scroll, se le avisa de la siguiente interacción posible

    /// RESUMEN: Avisar al jugador de la siguiente interacción tras X + Y tiempo, siendo X un valor serialiazdo e 
    ///     Y = 0 si no ha hecho scroll o 
    ///     Y = Z si sí ha hecho scroll (siendo Z otro valor configurable)

    /// Te pide interacción: Cuando el avatar detecte que el jugador no lo está haciendo muy bien 
    ///     (que no lo hace perfecto durante X artículos o más) este mostrará una exclamación y se verá preocupado. 
    ///     Cuando se interactúe con él, se le soltará la chapa acerca de la importancia de leer y compartir buenos artículos

    /// este nota tiene que tener acceso a la puntuación del nivel actual, así que necesitará referencias al LevelManager y habrá que hacer públicas varias cosas
    /// también tiene que ver cuántas interaccione estás haciendo con la pantalla
    /// también tiene que ver qué botones ya has pulsado (si has leído el artículo, si lo has compartido...)
    /// va a necesitar diferentes estados:
    ///     - NORMAL: Sonríe feliz viendo que el jugador ha hecho bien 1 nivel (este es el estado base)
    ///     - BAD: Detecta que el jugador no está haciéndolo muy bien y se pone sad. Aquí es cuándo debemos lanzar la interrogación
    ///     - GOOD: Está súper contento porque ve que el jugador lleva haciéndolo bien 2 o más niveles

    const GameAssistantState INITIAL_STATE = GameAssistantState.NORMAL;
    GameAssistantState _currentState = GameAssistantState.NORMAL;

    [Header("Visuals")]
    [SerializeField, SerializedDictionary]
    SerializedDictionary<GameAssistantState, Sprite> _spriteOnState = new SerializedDictionary<GameAssistantState, Sprite>();

    [Header("Scene References")]
    [SerializeField] private Image _assistanteImage = null;
    [SerializeField] private ScrollRect _scroll = null;
    [SerializeField] private GameObject _message = null;
    private TextMeshProUGUI _messageText = null;
    private Animator _messageAnimator = null;
    
    
    // TODO: ADD SOUND TO MESSAGES


    [SerializeField] private Button _okAssitantButton = null;

    [Header("Parameters")]
    [SerializeField] float noActionTimeUntilAdvice = 1.0f;
    
    // this depends on the current article title's length or body if the user has already started reading it.
    float _additionalScrolledTime = 10.0f;

    bool _hasScrolled = false;
    bool _keepTrackOfTime = false;

    ScoreManager _scoreManager = null;
    ArticleGameObject _articleData = null;

    float _timer = 0.0f;

    private void Initialize()
    {
        if (_assistanteImage == null)
        {
            Debug.LogError("Game Assistant Image not found...");
        }
        else
        {
            _assistanteImage.enabled = true;

        }
        Debug.Assert(_scroll != null, "Scroll was not set.");
        Debug.Assert(_message != null, "Message was not set.");

        _message.SetActive(false);
        _messageText = _message.GetComponentInChildren<TextMeshProUGUI>();
        _messageAnimator = _message.GetComponent<Animator>();

        Debug.Assert(_messageText != null, "Message has no Text child object.");

        Debug.Assert(_okAssitantButton != null, "OK Assistant button was not set.");
        _okAssitantButton.onClick.AddListener(HideMessage);

        _scoreManager = ScoreManager.Instance;

        LevelManager.Instance.onLevelStart.AddListener(OnLevelStart);
        LevelManager.Instance.onLevelEnd.AddListener(OnLevelEnd);
        LevelManager.Instance.onNewArticleSpawned.AddListener(GetNewArticle);

        ChangeState(INITIAL_STATE);
    }

    IEnumerator WaitToActivateScrollTracking()
    {
        yield return new WaitForSeconds(0.2f);
        _scroll.onValueChanged.AddListener(HasScrolled);
    }

    private void OnLevelStart(int _)
    {
        _keepTrackOfTime = true; 
        _hasScrolled = false;
        _timer = 0.0f;

        StartCoroutine(WaitToActivateScrollTracking());
    }

    private void OnLevelEnd(int _)
    {
        _keepTrackOfTime = false; 
        _articleData = null;
        _timer = 0.0f;

        _scroll.onValueChanged.RemoveListener(HasScrolled);
    }

    private void GetNewArticle(ArticleGameObject article)
    {
        if (_articleData != null)
        {
            _articleData.OnSkip.RemoveListener(OnArticleSkip);
        }

        _articleData = article;
        _articleData.OnSkip.AddListener(OnArticleSkip);

        _timer = 0.0f;
        _keepTrackOfTime = true;
    }

    private void OnArticleSkip()
    {
        _keepTrackOfTime = false;
    }

    private void HasScrolled(Vector2 _) 
    {
        if(!_hasScrolled)
        {
            Debug.Log("User scrolled!");
            _hasScrolled = true; 
            _timer = 0.0f; 
        }
    }
    
    public void ChangeState(GameAssistantState newState)
    {
        _currentState = newState;
        _assistanteImage.sprite = _spriteOnState[newState];
    }

    public void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (_keepTrackOfTime)
        {
            _timer += Time.deltaTime;
            if (!_hasScrolled && !_articleData.HasReadArticle && !_articleData.HasSharedArticle)
            {
                if (_timer > noActionTimeUntilAdvice)
                {
                    SuggestScroll();
                    _keepTrackOfTime = false;
                }
            }
            else
            {
                if (_timer > noActionTimeUntilAdvice + _additionalScrolledTime)
                {
                    if(!_articleData.HasReadArticle)
                    {
                        SuggestReadingArticle();
                    }
                    else if (!_articleData.HasSharedArticle)
                    {
                        SuggestSharingArticle();
                    }
                }
            }
        }
    }

    private void SuggestSharingArticle()
    {
        ShowMessage("Ahora puedes compartir el artículo o puedes ignorarlo pulsando el botón 'Saltar'");
    }

    private void SuggestReadingArticle()
    {
        ShowMessage("Puedes Leer el artículo pulsando el botón 'Leer'. O puedes ignorarlo pulsando el botón 'Saltar'");
    }

    private void SuggestScroll()
    {
        ShowMessage("¡Recuerda! Puedes utilizar el dedo para deslizar la pantalla");
    }

    private void ShowMessage(string msg)
    {
        _messageText.SetText(msg);
        _message.SetActive(true);
        _messageAnimator.SetTrigger("NewMessage");
        _okAssitantButton.gameObject.SetActive(true);
        _keepTrackOfTime = false;
    }

    public void HideMessage()
    {
        _message.SetActive(false);
        _messageText.SetText(string.Empty);
        _keepTrackOfTime = true;
        _timer = 0.0f;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(GameAssistant))]
public class GameAssistantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameAssistant my = (GameAssistant)target;

        // Optional: disable the button when not in Play Mode
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if (GUILayout.Button("Change State: BAD"))
        {
            my.ChangeState(GameAssistantState.BAD);
        }
        if (GUILayout.Button("Change State: NORMAL"))
        {
            my.ChangeState(GameAssistantState.NORMAL);
        }
        if (GUILayout.Button("Change State: GOOD"))
        {
            my.ChangeState(GameAssistantState.GOOD);
        }
    }
}
#endif