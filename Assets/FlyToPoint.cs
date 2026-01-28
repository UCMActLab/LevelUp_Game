using System.Collections;
using UnityEngine;

public class FlyToPoint : MonoBehaviour
{
    public void FlyTo(Vector3 position, float time = 1.5f)
    {
        StartCoroutine(FlyToCoroutine(position, time));
    } 

    IEnumerator FlyToCoroutine(Vector3 position, float time)
    {
        Vector3 initialPosition = transform.position;
        Vector3 currentPosition = initialPosition;
        float currentTime = 0.0f;

        while (currentTime < time)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;

            currentPosition = Vector3.Lerp(initialPosition, position, currentTime / time);
            transform.position = currentPosition;
        }

        transform.position = position;

        float scaleTime = 0.3f;
        currentTime = 0.0f;
        Vector3 initialScale = transform.localScale;
        while(currentTime < scaleTime)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            float factor = 1 - currentTime / scaleTime;
            transform.localScale = new Vector3(initialScale.x * factor, initialScale.y * factor, initialScale.z * factor);
        }

        // Destroy(gameObject);
    }
}
