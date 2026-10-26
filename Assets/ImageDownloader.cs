using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using AYellowpaper.SerializedCollections;

public class ImageDownloader : MonoBehaviour
{
    // Importamos la función de nuestro plugin .jslib
    [DllImport("__Internal")]
    private static extern void DownloadFileRaw(string filename, byte[] array, int size);

    [Header("Configuration")]
    [SerializedDictionary]
    public SerializedDictionary<Language, string> imageURLs;

    public void OnDownloadButtonClicked()
    {
        string imageID = imageURLs[LanguageSelection.chosenLanguage].Replace("https://drive.google.com/file/d/", "");
        imageID = imageID.Replace("/view?usp=sharing", "");
        imageID = imageID.Replace("/view?usp=drive_link", "");
        imageID = imageID.Replace("image:", "");
        string finalURL = "https://levelup-game.fundacionmaldita.es/api/proxy/gdrive?fileId=" + imageID;
        StartCoroutine(DownloadAndSaveImage(finalURL));
    }

    private IEnumerator DownloadAndSaveImage(string url)
    {
        // 1. Descargar la imagen desde la web
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al descargar la imagen: " + uwr.error);
            }
            else
            {
                // 2. Obtener la textura descargada
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                // 3. Convertir la textura a bytes (PNG) y luego a texto Base64
                byte[] imageBytes = texture.EncodeToPNG();
                string base64String = Convert.ToBase64String(imageBytes);

#if UNITY_WEBGL && !UNITY_EDITOR
                // 4. Llamar a JavaScript (Solo funciona en la build final de WebGL)
                DownloadFileRaw("Certificate_" + LanguageSelection.chosenLanguage, imageBytes, imageBytes.Length);
#else
                Debug.LogWarning("La descarga directa a través del navegador solo funciona en la Build de WebGL. En el editor no se ejecutará.");
#endif

                // Limpiar memoria
                Destroy(texture);
            }
        }
    }
}