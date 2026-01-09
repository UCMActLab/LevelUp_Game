using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

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

    /// RESUMEN: Avisar al jugador de la siguiente interaccióntras X + Y tiempo, siendo X un valor serialiazdo e 
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
    [SerializeField] private UnityEngine.UI.Image _assistanteImage = null;

    ScoreManager _scoreManager = null;

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

        _scoreManager = ScoreManager.Instance;

        ChangeState(INITIAL_STATE);
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