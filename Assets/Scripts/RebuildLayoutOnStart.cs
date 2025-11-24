using UnityEngine;
using UnityEngine.UI;

public class RebuildLayoutOnStart : MonoBehaviour
{
    [SerializeField] private float timeToRebuild = 0.5f;
    [SerializeField] private bool repeat = false;

    void Start()
    {
        Invoke("RebuildAllLayouts", timeToRebuild);
        if(repeat) InvokeRepeating("RebuildAllLayouts", 0.0f, timeToRebuild);
        RebuildAllLayouts();
    }

    public void RebuildAllLayouts()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        foreach (Transform tr in transform.GetComponentsInChildren<Transform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tr as RectTransform);
        }
    }
}
