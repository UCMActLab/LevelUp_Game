using DA_Assets.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectMask2D))]
public class MaskDisplacementAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _fillTarget;
    [SerializeField] private float _speed = 5.0f;

    private System.Collections.Generic.List<RectTransform> _childs;

    Vector2 _initialPos;
    Vector2 _endPos;

    IEnumerator WaitTimeToSetup(float time)
    {
        yield return new WaitForSeconds(time);

        _childs = new System.Collections.Generic.List<RectTransform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            _childs.Add(transform.GetChild(i).transform as RectTransform);
        }

        _initialPos = _fillTarget.position - Vector3.right * _fillTarget.GetWidth() / 2;
        _endPos = _fillTarget.position + Vector3.right * _fillTarget.GetWidth() / 2;
        float width = Mathf.Abs((_initialPos - _endPos).x);
        float widthOffset = width / _childs.Count;

        Debug.Log(widthOffset);
        Debug.Log(_childs.Count);
        Debug.Log(width);

        for (int i = 0; i < _childs.Count; ++i)
        {
            _childs[i].position = _initialPos + Vector2.right * widthOffset * i;
            _childs[i].sizeDelta = new Vector2(_childs[i].sizeDelta.x, 400);
        }
    }

    private void _clean()
    {
        if (!(_childs != null && _childs.Count > 0)) return;

        _childs.Clear();
        _childs = null;

        StopAllCoroutines();
    }

    private void OnEnable()
    {
        _clean();
        StartCoroutine(WaitTimeToSetup(0.5f));
    }

    private void OnDisable()
    {
        _clean();
    }

    private void Animation()
    {
        if (!(_childs != null && _childs.Count > 0)) return;

        foreach (RectTransform child in _childs)
        {
            child.Translate(Time.deltaTime * Vector2.right * _speed, Space.World);
            if (child.position.x >= _endPos.x)
            {
                child.position = Vector2.right * (child.position.x - _endPos.x + _initialPos.x) + Vector2.up * child.position.y;
            }
        }
    }

    void Update()
    {
        Animation();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_fillTarget.position, 10);
        Gizmos.DrawSphere(_initialPos, 10);
        Gizmos.DrawSphere(_endPos, 10);
    }
}
