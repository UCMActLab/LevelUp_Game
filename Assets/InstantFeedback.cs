using TMPro;
using UnityEngine;

public class InstantFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = null;

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void Setup(string text)
    {
        // string realText = TranslationManager.Instance.GetLocalizedStringValue("InstantFeedback", text);

        _text.SetText(text);
        gameObject.SetActive(true);

        _animator.SetTrigger("PopUp");
    }
}
