using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationInitializer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LocalizationStart());
    }

    IEnumerator LocalizationStart()
    {
        yield return LocalizationSettings.InitializationOperation;
        Destroy(gameObject);
    }

}
