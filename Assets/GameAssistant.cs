using AYellowpaper.SerializedCollections;
using B83.Win32;
using DA_Assets.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private Image _assistantImage = null;
    [SerializeField] private ScrollRect _scroll = null;
    [SerializeField] private GameObject _message = null;
    private TextMeshProUGUI _messageText = null;
    private Animator _messageAnimator = null;
    
    
    // TODO: ADD SOUND TO MESSAGES


    [SerializeField] private Button _okAssitantButton = null;
    // this button is for handling events when showing Messages
    [SerializeField] private Button _okAssitantButtonOneShots = null;
    [SerializeField] private Button _assistantHeadButton = null;

    [Header("Parameters")]
    [SerializeField] float noActionTimeUntilAdvice = 1.0f;

    // this depends on the current article title's length or body if the user has already started reading it.
    [SerializeField]
    float _hasDoneActionAdditionalTime = 10.0f;
    [SerializeField, Range(0.01f, 0.3f), Tooltip("Time to read each word in article's body")]
    float _readTimePerWord = 0.075f;

    [Header("How many to get assistant worried")]
    [SerializeField]
    private int _skipBeforeReadArticles = 2;
    private int _sharingNotReadingArticles = 2;
    private int _sharedFakeArticles = 2;

    bool _readingArticle = false;
    bool _hasScrolled = false;
    bool _keepTrackOfTime = false;

    int _skipBeforeReadCounter = 0;
    int _sharedNotRead = 0;
    int _fakeArticlesShared = 0;

    ScoreManager _scoreManager = null;
    ArticleGameObject _articleData = null;

    float _timer = 0.0f;

    List<string> _messagesOnClick = new List<string>();

    private void Initialize()
    {
        if (_assistantImage == null)
        {
            Debug.LogError("Game Assistant Image not found...");
        }
        else
        {
            _assistantImage.enabled = true;

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
            _articleData.OnRead.RemoveListener(OnArticleRead);
            _articleData.OnShare.RemoveListener(OnArticleShare);
        }

        _articleData = article;
        _articleData.OnSkip.AddListener(OnArticleSkip);
        _articleData.OnRead.AddListener(OnArticleRead);
        _articleData.OnShare.AddListener(OnArticleShare);


        _timer = 0.0f;
        _keepTrackOfTime = true;
    }

    private void OnArticleSkip()
    {
        if (_readingArticle)
        {
            SkipedBeforeReading();
        }
        _timer = 0.0f;
    }
    
    private void OnArticleRead()
    {
        StartCoroutine(IsReading());
    }

    IEnumerator IsReading()
    {
        _readingArticle = true;
        _keepTrackOfTime = false;
        int words = _articleData.GetBodyString().Split(" ").Length;
        yield return new WaitForSeconds(words * _readTimePerWord);
        _keepTrackOfTime = true;
        _readingArticle = false;

        _timer = 0.0f;
    }

    private void OnArticleShare()
    {
        _keepTrackOfTime = false;

        if (!_articleData.IsTrue)
        {
            _fakeArticlesShared++;
        }

        // no leído
        if ((!_articleData.HasReadArticle || _readingArticle) && _articleData.GetBodyString() != string.Empty)
        {
            SharedDidntRead();
        }
        // sí leído pero falso
        else if (_articleData.HasReadArticle && !_readingArticle && !_articleData.IsTrue && _fakeArticlesShared > _sharedFakeArticles)
        {
            // WORRY MAN acerca de esparcir desinformación

            // WORRIED/SHARED_FALSE/READED_ARTICLE/0/0
            List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SHARED_FALSE/READED_ARTICLE/0/", 5);

            //messages.Add("Creo que no te estás fijando bien en lo que lees");
            //messages.Add("Has compartido algunos bulos, a pesar de haber leído los artículos");
            //messages.Add("Intenta fijarte más en si apelan a tus emociones");
            //messages.Add("O si la redacción parece profesional");
            //messages.Add("¡Y no te olvides de revisar la fuente!");
            WorryAssitant(messages);


            _fakeArticlesShared = 0;
        }
    }

    private void WorryAssitant(List<string> messages)
    {
        if (GameAssistantState.BAD != _currentState)
        {
            _messagesOnClick.Clear(); 

            ChangeState(GameAssistantState.BAD);

            _assistantHeadButton.onClick.AddListener(ShowMessageOnClick);
        }

        _messagesOnClick.AddRange(messages);
    }

    int _messageIndex = 0;
    private void ShowMessageOnClick()
    {
        _messageIndex = 0;

        _okAssitantButton.onClick.RemoveListener(HideMessage);
        _okAssitantButton.onClick.AddListener(ShowNextMessage);

        _okAssitantButton.gameObject.SetActive(true);

        _message.SetActive(true);
        ShowNextMessage();
        _keepTrackOfTime = false;

        _assistantHeadButton.onClick.RemoveListener(ShowMessageOnClick);
    }

    private void ShowNextMessage()
    {
        if (_messageIndex >= _messagesOnClick.Count)
        {
            HideMessage();
            ChangeState(GameAssistantState.NORMAL);
            _okAssitantButton.onClick.RemoveListener(ShowNextMessage);
            _okAssitantButton.onClick.AddListener(HideMessage);
        }
        else
        {
            _messageText.SetText(_messagesOnClick[_messageIndex++]);
            _message.GetComponent<RebuildLayoutOnStart>().RebuildAllLayouts();
            _messageAnimator.SetTrigger("NewMessage");
        }
    }

    private void HasScrolled(Vector2 _) 
    {
        if(!_hasScrolled)
        {
            _hasScrolled = true; 
            _timer = 0.0f; 
        }
    }

    private void SharedDidntRead()
    {
        _sharedNotRead++;

        if (_sharedNotRead >= _sharingNotReadingArticles)
        {
            if (_fakeArticlesShared >= _sharedFakeArticles)
            {
                // WORRY MAN pero con distinto texto diciendo que se está esparciendo desinformación por no leer
                List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SHARED_FALSE/DOESNT_READ/0/", 3);

                //List<string> messages = new List<string>();
                //messages.Add("No estás leyendo las cosas antes de compartirlas...");
                //messages.Add("Eso te hace difundir más bulos");
                //messages.Add("Tienes que leer más los artículos");
                WorryAssitant(messages);

                _fakeArticlesShared = 0;
            }
            else
            {
                // WORRY MAN así en general diciendo que hay que fijarse en las fuentes y cómo están escritas las cosas
                List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SHARED_TRUE/DOESNT_READ/0/", 4);

                //messages.Add("No estás leyendo las cosas antes de compartirlas...");
                //messages.Add("Tienes suerte de que algunos son ciertos");
                //messages.Add("Pero esto no siempre es así");
                //messages.Add("Tienes que leer más los artículos");
                WorryAssitant(messages);

                _sharedNotRead = 0;
            }


            _sharedNotRead = 0;
        }
    }

    private void SkipedBeforeReading()
    {
        _skipBeforeReadCounter++;

        if (_skipBeforeReadCounter >= _skipBeforeReadArticles)
        {
            // WORRY MAN
            List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SKIPPED/DOESNT_READ/0/", 3);

            //messages.Add("No estás leyendo las cosas antes de saltártelas...");
            //messages.Add("A mí no me engañas jej");
            WorryAssitant(messages);

            _skipBeforeReadCounter = 0;
        }
    }   
    
    public void ChangeState(GameAssistantState newState)
    {
        _currentState = newState;
        _assistantImage.sprite = _spriteOnState[newState];
    }

#if UNITY_EDITOR
    TextMeshProUGUI d_hasScrolled = null;
    TextMeshProUGUI d_currentTime = null;
    TextMeshProUGUI d_targetTime = null;
    TextMeshProUGUI d_hasRead = null;
    TextMeshProUGUI d_hasShared = null;
    TextMeshProUGUI d_isReading = null;
    TextMeshProUGUI d_fakeReadArticles = null;
    TextMeshProUGUI d_falseShared = null;
    TextMeshProUGUI d_sharedNotRead = null;
#endif

    public void Start()
    {
        Initialize();

#if UNITY_EDITOR
        d_hasScrolled = DebugMenu.Instance.AddDebugText("Has Scrolled");
        d_currentTime = DebugMenu.Instance.AddDebugText("Current Time");
        d_targetTime = DebugMenu.Instance.AddDebugText("Target Time");
        d_isReading = DebugMenu.Instance.AddDebugText("Is Reading");
        d_hasRead = DebugMenu.Instance.AddDebugText("Has Read");
        d_hasShared = DebugMenu.Instance.AddDebugText("Has Shared");
        d_fakeReadArticles = DebugMenu.Instance.AddDebugText("Skip before read Counter");
        d_falseShared = DebugMenu.Instance.AddDebugText("False Sharing Counter");
        d_sharedNotRead = DebugMenu.Instance.AddDebugText("Didnt read before Sharing Counter");
#endif
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        DebugMenu.Instance.RemoveText(d_hasScrolled);
        DebugMenu.Instance.RemoveText(d_currentTime);
        DebugMenu.Instance.RemoveText(d_targetTime);
        DebugMenu.Instance.RemoveText(d_isReading);
        DebugMenu.Instance.RemoveText(d_hasRead);
        DebugMenu.Instance.RemoveText(d_hasShared);
        DebugMenu.Instance.RemoveText(d_fakeReadArticles);
        DebugMenu.Instance.RemoveText(d_falseShared);
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        d_hasScrolled.SetText(_hasScrolled.ToString());
        d_isReading.SetText(_readingArticle.ToString());
        d_fakeReadArticles.SetText(_skipBeforeReadCounter.ToString());
        d_falseShared.SetText(_fakeArticlesShared.ToString());
        d_sharedNotRead.SetText(_sharedNotRead.ToString());

        if (_articleData != null)
        {
            d_hasRead.SetText(_articleData.HasReadArticle.ToString());
            d_hasShared.SetText(_articleData.HasSharedArticle.ToString());
        }
#endif

        if (_keepTrackOfTime)
        {
            _timer += Time.deltaTime;

#if UNITY_EDITOR
            d_currentTime.SetText(_timer.ToString());
#endif

            if (!_hasScrolled && !_articleData.HasReadArticle && !_articleData.HasSharedArticle)
            {
#if UNITY_EDITOR
                d_targetTime.SetText(noActionTimeUntilAdvice.ToString());
#endif
                if (_timer > noActionTimeUntilAdvice)
                {
                    SuggestScroll();
                    _keepTrackOfTime = false;
                }
            }
            else
            {
#if UNITY_EDITOR
                d_targetTime.SetText((noActionTimeUntilAdvice + _hasDoneActionAdditionalTime).ToString());
#endif
                if (_timer > _hasDoneActionAdditionalTime)
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

    public void ShowMessage(string msg)
    {
        _ShowMessage(msg);
        _okAssitantButton.gameObject.SetActive(true);
        _keepTrackOfTime = false;
    }

    public void ShowMessageOneShot(string msg, UnityAction oneShot = null)
    {
        _ShowMessage(msg);
        _okAssitantButtonOneShots.gameObject.SetActive(true);
        _keepTrackOfTime = false;
        if (oneShot != null) AssistantEndMessageOneShot(oneShot);
    }

    public void ShowMessagesOneShot(string[] msg, UnityAction oneShot = null)
    {
        StartCoroutine(_ShowMessages(msg, oneShot));
        //_okAssitantButtonOneShots.gameObject.SetActive(true);
        //_keepTrackOfTime = false;
        //if (oneShot != null) AssistantEndMessageOneShot(oneShot);
    }

    IEnumerator _ShowMessages(string[] msg, UnityAction oneShot = null)
    {
        _okAssitantButtonOneShots.gameObject.SetActive(true);
        _okAssitantButtonOneShots.onClick.RemoveAllListeners();
        _keepTrackOfTime = false;

        int current = 0;
        bool nextMessage = false;
        _okAssitantButtonOneShots.onClick.AddListener(() => nextMessage = true);
        while (current < msg.Length)
        {
            nextMessage = false;
            _ShowMessage(msg[current++]);
            if (current >= msg.Length)
            {
                _okAssitantButtonOneShots.onClick.RemoveAllListeners();
                if (oneShot != null) AssistantEndMessageOneShot(oneShot);
            }
            else
            {
                yield return new WaitUntil(() => nextMessage == true);
            }
        }
    }

    private void _ShowMessage(string msg)
    {
        _messageText.SetText(msg);
        _message.SetActive(true);
        _messageAnimator.SetTrigger("NewMessage");
        _message.GetComponent<RebuildLayoutOnStart>().RebuildAllLayouts();
    }

    public void AssistantEndMessageOneShot(UnityAction oneShot)
    {
        _okAssitantButtonOneShots.onClick.AddListener(() => { oneShot(); _okAssitantButtonOneShots.onClick.RemoveAllListeners(); HideMessageOneShot(); });
    }

    public void HideMessage()
    {
        _message.SetActive(false);
        _messageText.SetText(string.Empty);
        _okAssitantButton.gameObject.SetActive(false);
        _keepTrackOfTime = true;
        _timer = 0.0f;
    }

    private void HideMessageOneShot()
    {
        _message.SetActive(false);
        _messageText.SetText(string.Empty);
        _okAssitantButtonOneShots.gameObject.SetActive(false);
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