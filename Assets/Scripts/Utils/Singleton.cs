using UnityEngine;

/// <summary>
/// Patrón Singleton. Objetos persistentes entre escenas y de los que solo
/// puede haber una instancia. Todas las demás instancias son automáticamente destruídas junto
/// con sus objetos.
/// 
/// WARNING: No tener objetos hijos de Singleton que sean relevantes para otros objeto
/// diferentes al propio Singleton
/// 
/// </summary>
/// <typeparam name="T"> El tipo que heredará de Singleton </typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance { get { return _instance; } }

    protected bool _destroyOnLoad = false;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            // Si tenemos un Singleton hijo de algo,
            // hacemos que ese algo persista, para no perder el
            // singleton
            //Transform current = transform;
            //while (current.parent != null)
            //{
            //    current = current.parent;
            //}
            if (!_destroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if(_instance == this as T)
        {
            _instance = null;
        } 
    }
}
