using System.Collections;
using UnityEngine;

public class AddPointsFeedback : MonoBehaviour
{
    [SerializeField] private GameObject _pointFeedbackPrefab = null;
    [SerializeField] private Transform _targetPosition = null;
    [SerializeField] private Transform _originPoint = null;

    public void AddPoints(int points)
    {
        StartCoroutine(AddPointsCoroutine(points));
    }

    IEnumerator AddPointsCoroutine(int points)
    {
        for (int i = 0; i < points; i++)
        {
            GameObject go = Instantiate(_pointFeedbackPrefab, _originPoint);
            go.GetComponent<FlyToPoint>().FlyTo(_targetPosition.position + Vector3.up * 0.5f, 0.6f);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(AddPointsFeedback))]
public class AddPointsFeedbackEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AddPointsFeedback my = (AddPointsFeedback)target;

        // Optional: disable the button when not in Play Mode
        // UnityEditor.EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if (GUILayout.Button("Add 1 Point"))
        {
            my.AddPoints(1);
        }
        // If you want the button to work in edit mode too, remove the BeginDisabledGroup/EndDisabledGroup
    }
}
#endif
