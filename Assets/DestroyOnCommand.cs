using UnityEngine;

public class DestroyOnCommand : MonoBehaviour
{
    public void Destroy(GameObject target)
    {
        Object.Destroy(target);
    }
}
