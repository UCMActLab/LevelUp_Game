using UnityEngine;
using UnityEngine.UI;

using static Unity.Collections.AllocatorManager;

public class HandIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject hand;

    Button button = null;

    [SerializeField]
    float timeHand = 3f;

    float timer = 0f;
    void Start()
    {
        // Oculta el indicador al iniciar
        hand.SetActive(false);

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);

        // Reinicia contador
        timer = 0f;
    }

    public void Update()
    {
        // Va contando el tiempo
        timer += Time.deltaTime;

        // Si pasan 3 segundos sin pulsar el botón
        if (timer >= timeHand)
        {
            hand.SetActive(true);
        }
    }

    private void OnButtonPressed()
    {
        // Oculta el indicador
        hand.SetActive(false);

        // Reinicia contador
        timer = 0f;
    }

}
