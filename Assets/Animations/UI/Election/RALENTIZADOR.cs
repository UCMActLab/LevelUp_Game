using UnityEngine;

public class RALENTIZADOR : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)) 
            Time.timeScale = 1.0f;

        if (Input.GetKeyUp(KeyCode.A))
            Time.timeScale = 0.1f;
    }
}
