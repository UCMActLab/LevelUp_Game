using TMPro;
using UnityEngine;

public class PopUpTextGameObject : MonoBehaviour
{
    Animator _animator = null;

    [SerializeField] TextMeshProUGUI _text = null;

    [SerializeField] bool _popUpOnEnable = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PopUp();
    }

    private void OnDisable()
    {
        Hide();
    }

    private void Hide()
    {
        if (!_animator) _animator = GetComponent<Animator>();
        _animator.SetTrigger("Hide");
    }

    public void PopUp(string newText = "")
    {
        if(!string.IsNullOrEmpty(newText))
        {
            SetText(newText);
        }

        if (!_animator) _animator = GetComponent<Animator>();
        _animator.SetTrigger("PopUp");
    }

    public void SetText(string newText)
    {
        _text.SetText(newText);
    }
}
