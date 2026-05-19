using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class ImageDownloader : MonoBehaviour
{
    // Importamos la función de nuestro plugin .jslib
    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string base64);

    [Header("Configuración")]
    [Tooltip("La URL de la imagen que quieres descargar")]
    public string imageUrl = "https://www.w3.org/People/mimasa/test/imgformat/img/w3c_home.png";

    // Este es el método que debes asignar al evento OnClick() de tu botón en el Canvas
    public void OnDownloadButtonClicked()
    {
        string imageID = imageUrl.Replace("https://drive.google.com/file/d/", "");
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

                // 4. Llamar a JavaScript (Solo funciona en la build final de WebGL)
#if UNITY_WEBGL && !UNITY_EDITOR
                DownloadFile("imagen_descargada.png", base64String);
#else
                Debug.LogWarning("La descarga directa a través del navegador solo funciona en la Build de WebGL. En el editor no se ejecutará.");
#endif

                // Limpiar memoria
                Destroy(texture);
            }
        }
    }
}