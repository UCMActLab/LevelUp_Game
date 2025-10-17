using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] private float _rotateFactor = 1.0f;
    [SerializeField, 
        Tooltip("Todavía no funciona bien el Clockwise. El anti-horario sí")] 
    private bool _clockwise = false;

    private float _initialRotation = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _initialRotation = transform.rotation.eulerAngles.z;
    }


    private void OnDisable()
    {
        Quaternion rot = transform.rotation;
        transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, _initialRotation);
    }

    /// <summary>
    /// Realiza la rotación
    /// </summary>
    /// <param name="finalPoint"> [0, 1] </param>
    public void StartRotation(float finalPoint, UnityAction<float> whileRotating)
    {
        float initial = transform.rotation.eulerAngles.z;

        float final = initial + (_clockwise ? -finalPoint : (1 - finalPoint)) * 360;
        StartCoroutine(Rotate(final, whileRotating));
    }

    private IEnumerator Rotate(float finalRotation, UnityAction<float> whileRotating)
    {
        Quaternion rot = transform.rotation;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += _rotateFactor * Time.deltaTime;

            rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y,
                Mathf.Lerp(_initialRotation, finalRotation, t));

            transform.rotation = rot;

            whileRotating.Invoke(t);

            yield return new WaitForEndOfFrame();
        }

        rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, finalRotation);

        transform.rotation = rot;
    }
}
