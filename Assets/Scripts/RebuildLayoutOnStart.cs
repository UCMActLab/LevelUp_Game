using UnityEngine;
using UnityEngine.UI;

public class RebuildLayoutOnStart : MonoBehaviour
{
    void Start()
    {
        RebuildAllLayouts();
    }
    private void RebuildAllLayouts()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        foreach (Transform tr in transform.GetComponentsInChildren<Transform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tr as RectTransform);
        }
    }
}
