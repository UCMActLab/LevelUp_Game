using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton usado para cambiar de escenas, que incorpora funcionalidad
/// para cambio asíncrono y fade-in/fade-outs
/// 
/// TODO: Añadir fader genérico
/// </summary>
public class SceneChanger : Singleton<SceneChanger>
{
    /// <summary>
    /// Componente que nos ayuda a realizar FadeIns y FadeOuts
    /// </summary>
    private Fader _fader;

    /// <summary>
    /// Evento llamado al terminar de cargar una escena.
    /// Recibe una string, que es el nombre de la anterior escena
    /// y una Scene, que es la escena cargada
    /// </summary>
    private UnityEvent<string, Scene> _onSceneChanged;

    public UnityEvent<string, Scene> OnSceneChanged { get { return _onSceneChanged; } }

    /// <summary>
    /// Variable utilizada para guardar el valor de la nueva escena a la que se 
    /// debe cambiar.
    /// Una vez se cambie de escena, se limpiará dicho valor y se pondrá a string.Empty
    /// </summary>
    string newSceneName = string.Empty;

    protected override void Awake()
    {
        base.Awake();
        _onSceneChanged = new UnityEvent<string, Scene>();
        _fader = GetComponentInChildren<Fader>();
    }

    /// <summary>
    /// Cambia la escena a la dada por el parámetro en el nombre
    /// tras hacer un fade-out
    /// </summary>
    /// <param name="newSceneName"> Escena a cargar </param>
    public void ChangeScene(string newSceneName)
    {
        this.newSceneName = newSceneName;

        if(_fader != null)
        {
            _fader.StartFade(1.5f, 0.0f, 1.0f);
            _fader.OnFadeEnd.AddListener(LoadScene);
        }
        else { LoadScene(); }
    }

    private async void LoadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        await SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Single);
        newSceneName = string.Empty;
        _onSceneChanged.Invoke(currentSceneName, SceneManager.GetActiveScene());

        if(_fader != null)
        {
            _fader.OnFadeEnd.RemoveListener(LoadScene);
            _fader.StartFade(1.5f, 1.0f, 0.0f);
        }
    }
}
