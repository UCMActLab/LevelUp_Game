using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct QueuedMessage
{
    public string Text;
    public UnityAction OnComplete;

    public QueuedMessage(string text, UnityAction onComplete = null)
    {
        Text = text;
        OnComplete = onComplete;
    }
}

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

    private Queue<QueuedMessage> _messageQueue = new Queue<QueuedMessage>();
    private bool _isDisplayingQueue = false;
    private UnityAction _currentMessageAction = null;

    const GameAssistantState INITIAL_STATE = GameAssistantState.NORMAL;
    GameAssistantState _currentState = GameAssistantState.NORMAL;

    public GameAssistantState State { get {  return _currentState; } }

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
    [SerializeField] private GameObject _footer = null;
    [SerializeField] private Button _okAssitantButton = null;
    // this button is for handling events when showing Messages
    [SerializeField] private Button _okAssitantButtonOneShots = null;
    // [SerializeField] private Button _assistantHeadButton = null;
    [SerializeField] private GameObject _buttonBackground = null;

    [Header("Parameters")]
    [SerializeField] float noActionTimeUntilAdvice = 1.0f;

    // this depends on the current article title's length or body if the user has already started reading it.
    [SerializeField]
    float _hasDoneActionAdditionalTime = 10.0f;
    [SerializeField, Range(0.2f, 1.5f), Tooltip("Time to read each word in article's body")]
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

    ArticleGameObject _articleData = null;

    float _timer = 0.0f;


    public UnityEvent<GameAssistantState> onStateChanged = new UnityEvent<GameAssistantState>();

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

        LevelManager.Instance.onLevelStart.AddListener(OnLevelStart);
        LevelManager.Instance.onLevelEnd.AddListener(OnLevelEnd);
        LevelManager.Instance.onNewArticleSpawned.AddListener(GetNewArticle);

        ChangeState(INITIAL_STATE);

        // Asegurarnos de que el botón OK principal procese la cola
        _okAssitantButton.onClick.RemoveAllListeners();
        _okAssitantButton.onClick.AddListener(OnOkButtonClicked);

        // El botón OneShot puede seguir existiendo, pero lo unificaremos
        _okAssitantButtonOneShots.onClick.RemoveAllListeners();
        _okAssitantButtonOneShots.onClick.AddListener(OnOkButtonClicked);
    }

    public void ShowMessage(string msg)
    {
        EnqueueMessage(msg, ProcessNextMessage);
    }

    public void ShowEnqueuedMessages(UnityAction onComplete = null)
    {
        AddActionToQueueEnd(onComplete);
        ProcessNextMessage();
    }
    
    private void AddActionToQueueEnd(UnityAction onComplete)
    {
        if (onComplete == null) return;

        var list = _messageQueue.ToList();
        QueuedMessage message = list[_messageQueue.Count -1];
        list.RemoveAt(_messageQueue.Count - 1);
        message.OnComplete += onComplete;

        _messageQueue = new Queue<QueuedMessage>(list);
        _messageQueue.Enqueue(message);
    }

    private void EnqueueMessage(string msg, UnityAction onComplete = null, bool activateMessage = true)
    {
        _messageQueue.Enqueue(new QueuedMessage(msg, onComplete));
        
        if (!_isDisplayingQueue && activateMessage)
        {
            ProcessNextMessage();
        }
    }

    private void ProcessNextMessage()
    {
        if (_messageQueue.Count > 0)
        {
            // ver cuándo es el primer mensaje

            _isDisplayingQueue = true;
            _keepTrackOfTime = false;
            
            QueuedMessage next = _messageQueue.Dequeue();
            _currentMessageAction = next.OnComplete;

            _messageText.SetText(next.Text);
            _message.SetActive(true);
            _okAssitantButton.gameObject.SetActive(true); // Usamos el botón estándar
            _buttonBackground.SetActive(true);

            // reseteamos antess de animar la cajita
            RectTransform rect = _message.GetComponent<RectTransform>();
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;

            _messageAnimator.Play("NewMessageTutorial", 0, 0f);
            if (_message.TryGetComponent(out RebuildLayoutOnStart rebuild)) rebuild.RebuildAllLayouts();
        }
        else
        {
            // se acaban los mensajes

            _isDisplayingQueue = false;
            _message.SetActive(false);
            _okAssitantButton.gameObject.SetActive(false); // Usamos el botón estándar
            _buttonBackground.SetActive(false);
            _keepTrackOfTime = true;
            _timer = 0.0f;
        }
    }

    private void OnOkButtonClicked()
    {
        // Ejecutar acción asociada al mensaje actual si existe
        _currentMessageAction?.Invoke();
        // _currentMessageAction = null;

        // Intentar mostrar el siguiente
        // ProcessNextMessage();
    }

    // --- ADAPTACIÓN DE TUS FUNCIONES EXISTENTES ---

    public void ShowMessages(string[] msgs, UnityAction oneShot = null)
    {
        for (int i = 0; i < msgs.Length; ++i) {
            UnityAction action = i < msgs.Length -1 ? ProcessNextMessage : () => { ProcessNextMessage(); oneShot(); };
            EnqueueMessage(msgs[i], action);
        }
    }

    public void ShowMessageOneShot(string msg, UnityAction oneShot = null)
    {
        EnqueueMessage(msg, () => { oneShot(); ProcessNextMessage(); });
    }

    public void WorryAssistant(List<string> messages)
    {
        if (_articleData.Data.feedback != null) return;

        if (GameAssistantState.BAD != _currentState)
        {
            ChangeState(GameAssistantState.BAD);
            for (int i = 0; i < messages.Count; ++i)
            {
                if (i == messages.Count - 1) EnqueueMessage(messages[i], ProcessNextMessage, false);
                else EnqueueMessage(messages[i], () => { ChangeState(GameAssistantState.NORMAL); ProcessNextMessage(); }, false);
            }
            
            // _assistantHeadButton.onClick.AddListener(() => { ProcessNextMessage(); _assistantHeadButton.onClick.RemoveAllListeners(); });
        }
    }

    //// El sistema de ShowMessageOnClick ahora es más simple
    //private void ShowMessageOnClick()
    //{
    //    // Si ya hay mensajes en cola, no hacemos nada o añadimos estos
    //    foreach(var msg in _messagesOnClick)
    //    {
    //         EnqueueMessage(msg);
    //    }
    //    _messagesOnClick.Clear();
    //    _assistantHeadButton.onClick.RemoveListener(ShowMessageOnClick);
    //}

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

        // _articleData = null;
        _timer = 0.0f;

        _scroll.onValueChanged.RemoveListener(HasScrolled);
    }

    private void GetNewArticle(ArticleGameObject article)
    {
        if (_articleData != null)
        {
            _articleData.OnSkip.RemoveListener(LevelManager.Instance.ShowNextArticle);
            _articleData.OnSkip.RemoveListener(OnArticleSkip);
            _articleData.OnRead.RemoveListener(OnArticleRead);
            _articleData.OnShare.RemoveListener(OnArticleShare);
        }

        _articleData = article;
        _articleData.OnSkip.AddListener(OnArticleSkip);
        _articleData.OnSkip.AddListener(LevelManager.Instance.ShowNextArticle);
        _articleData.OnRead.AddListener(OnArticleRead);
        _articleData.OnShare.AddListener(OnArticleShare);

        _timer = 0.0f;
        _keepTrackOfTime = true;
    }

    private void OnArticleSkip()
    {
        if ((_readingArticle || !_articleData.HasReadArticle) && _articleData.CanBeRead)
        {
            SkippedBeforeReading();
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
            List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SHARED_FALSE/READED_ARTICLE/0/", 2);

            WorryAssistant(messages);


            _fakeArticlesShared = 0;
        }
    }

    //public void WorryAssistant(List<string> messages)
    //{
    //    if (GameAssistantState.BAD != _currentState)
    //    {
    //        _messagesOnClick.Clear(); 

    //        ChangeState(GameAssistantState.BAD);

    //        _assistantHeadButton.onClick.AddListener(ShowMessageOnClick);
    //    }

    //    _messagesOnClick.AddRange(messages);
    //}

    //private void ShowMessageOnClick()
    //{
    //    _messageIndex = 0;

    //    _okAssitantButton.onClick.RemoveListener(HideMessage);
    //    _okAssitantButton.onClick.AddListener(ShowNextMessage);

    //    _okAssitantButton.gameObject.SetActive(true);

    //    _message.SetActive(true);
    //    ShowNextMessage();
    //    _keepTrackOfTime = false;

    //    _assistantHeadButton.onClick.RemoveListener(ShowMessageOnClick);
    //}

    //private void ShowNextMessage()
    //{
    //    if (_messageIndex >= _messagesOnClick.Count)
    //    {
    //        HideMessage();
    //        ChangeState(GameAssistantState.NORMAL);
    //        _okAssitantButton.onClick.RemoveListener(ShowNextMessage);
    //        _okAssitantButton.onClick.AddListener(HideMessage);
    //    }
    //    else
    //    {
    //        _messageText.SetText(_messagesOnClick[_messageIndex++]);
    //        _message.GetComponent<RebuildLayoutOnStart>().RebuildAllLayouts();
    //        _messageAnimator.SetTrigger("NewMessage");
    //    }
    //}

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
                List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SHARED_FALSE/DOESNT_READ/0/", 2);

                //List<string> messages = new List<string>();
                //messages.Add("No estás leyendo las cosas antes de compartirlas...");
                //messages.Add("Eso te hace difundir más bulos");
                //messages.Add("Tienes que leer más los artículos");
                WorryAssistant(messages);

                _fakeArticlesShared = 0;
            }
            else
            {
                // WORRY MAN así en general diciendo que hay que fijarse en las fuentes y cómo están escritas las cosas
                List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SHARED_TRUE/DOESNT_READ/0/", 2);

                //messages.Add("No estás leyendo las cosas antes de compartirlas...");
                //messages.Add("Tienes suerte de que algunos son ciertos");
                //messages.Add("Pero esto no siempre es así");
                //messages.Add("Tienes que leer más los artículos");
                WorryAssistant(messages);

                _sharedNotRead = 0;
            }


            _sharedNotRead = 0;
        }
    }

    private void SkippedBeforeReading()
    {
        _skipBeforeReadCounter++;

        if (_skipBeforeReadCounter >= _skipBeforeReadArticles)
        {
            // WORRY MAN
            List<string> messages = TranslationManager.Instance.GetLocalizedStringsList("ASSISTANT_ADVICES", "WORRIED/SKIPPED/DOESNT_READ/0/", 2);

            //messages.Add("No estás leyendo las cosas antes de saltártelas...");
            //messages.Add("A mí no me engañas jej");
            WorryAssistant(messages);

            _skipBeforeReadCounter = 0;
        }
    }   
    
    public void ChangeState(GameAssistantState newState)
    {
        _currentState = newState;
        _assistantImage.sprite = _spriteOnState[newState];

        onStateChanged.Invoke(_currentState);

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
        if(_isDisplayingQueue)
        {
            _footer.SetActive(true);
        }

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
        else
        {
            return;
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
        ShowMessage(TranslationManager.Instance.GetLocalizedStringValue("ASSISTANT_ADVICES", "SUGGEST_SHARING"));
    }

    private void SuggestReadingArticle()
    {
        ShowMessage(TranslationManager.Instance.GetLocalizedStringValue("ASSISTANT_ADVICES", "SUGGEST_READING"));
    }

    private void SuggestScroll()
    {
        ShowMessage(TranslationManager.Instance.GetLocalizedStringValue("ASSISTANT_ADVICES", "SUGGEST_SCROLLING"));
    }

    //public void ShowMessage(string msg)
    //{
    //    _ShowMessage(msg);
    //    _okAssitantButton.gameObject.SetActive(true);
    //    _keepTrackOfTime = false;
    //}

    //public void ShowMessages(string[] msg)
    //{
    //    ShowMessagesOneShot(msg, null);
    //}
    //public void ShowMessageOneShot(string msg, UnityAction oneShot = null)
    //{
    //    _ShowMessage(msg);
    //    _okAssitantButtonOneShots.gameObject.SetActive(true);
    //    _keepTrackOfTime = false;
    //    if (oneShot != null) AssistantEndMessageOneShot(oneShot);
    //}

    //public void ShowMessagesOneShot(string[] msg, UnityAction oneShot = null)
    //{
    //    StartCoroutine(_ShowMessages(msg, oneShot));
    //    //_okAssitantButtonOneShots.gameObject.SetActive(true);
    //    //_keepTrackOfTime = false;
    //    //if (oneShot != null) AssistantEndMessageOneShot(oneShot);
    //}

    //IEnumerator _ShowMessages(string[] msg, UnityAction oneShot = null)
    //{
    //    _okAssitantButtonOneShots.gameObject.SetActive(true);
    //    _okAssitantButtonOneShots.onClick.RemoveAllListeners();
    //    _keepTrackOfTime = false;

    //    int current = 0;
    //    bool nextMessage = false;
    //    _okAssitantButtonOneShots.onClick.AddListener(() => nextMessage = true);
    //    while (current < msg.Length)
    //    {
    //        nextMessage = false;
    //        _ShowMessage(msg[current++]);
    //        if (current >= msg.Length)
    //        {
    //            _okAssitantButtonOneShots.onClick.RemoveAllListeners();
    //            if (oneShot != null) AssistantEndMessageOneShot(oneShot);
    //            else AssistantEndMessageOneShot(HideMessage);
    //        }
    //        else
    //        {
    //            yield return new WaitUntil(() => nextMessage == true);
    //        }
    //    }
    //}

    //private void _ShowMessage(string msg)
    //{
    //    _messageText.SetText(msg);
    //    _message.SetActive(true);
    //    _messageAnimator.SetTrigger("NewMessage");
    //    _message.GetComponent<RebuildLayoutOnStart>().RebuildAllLayouts();
    //}

    //public void AssistantEndMessageOneShot(UnityAction oneShot)
    //{
    //    _okAssitantButtonOneShots.onClick.AddListener(() => { HideMessageOneShot(); oneShot(); _okAssitantButtonOneShots.onClick.RemoveAllListeners(); });
    //}

    public void HideMessage()
    {
        _message.SetActive(false);
        _messageText.SetText(string.Empty);
        _okAssitantButton.gameObject.SetActive(false);
        _keepTrackOfTime = true;
        _timer = 0.0f;
    }

    //private void HideMessageOneShot()
    //{
    //    _message.SetActive(false);
    //    _messageText.SetText(string.Empty);
    //    _okAssitantButtonOneShots.gameObject.SetActive(false);
    //}
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