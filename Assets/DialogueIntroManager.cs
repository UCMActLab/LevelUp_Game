using UnityEngine;
using UnityEngine.Events;

public class DialogueIntroManager : MonoBehaviour
{
    [Header("ShowDialogue(s)")]
    [SerializeField] ShowDialogue _bodyDialogue;

    [Header("DialogueSettings")]
    [SerializeField] DialogSettings[] _bodySettings;

    [Header("Eventos")]
    [SerializeField]
    private bool _startOnAwake = false;

    [SerializeField]
    private UnityEvent onDialoguesEnd = new UnityEvent();

    [SerializeField]
    private Animator _okButton = null;

    int _bodyDialogueIndex = 0;

    private void Start()
    {
        if (_startOnAwake)
        {
            StartDialogues();
            _bodyDialogue.ShowText();
        }
    }

    private void SetUp()
    {
        _bodyDialogueIndex = 0;

        _bodyDialogue.SetSettings(_bodySettings[_bodyDialogueIndex]);

        _bodyDialogue.onDialogueEnd.AddListener(AdvanceDialogues);
    }

    public void SetFeedbackOkButton(bool active)
    {
        _okButton.SetBool("Highlighted", active);
    }

    private void AdvanceDialogues()
    {
        if(++_bodyDialogueIndex >= _bodySettings.Length)
        {
            EndDialogues();
            return;
        }

        _bodyDialogue.SetSettings(_bodySettings[_bodyDialogueIndex]);

        _bodyDialogue.waitForInteraction = true;
    }

    private void EndDialogues()
    {
        onDialoguesEnd.Invoke();
    }

    public void StartDialogues()
    {
        SetUp();
    }
}
