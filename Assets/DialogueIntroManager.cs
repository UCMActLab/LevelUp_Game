using UnityEngine;
using UnityEngine.Events;

public class DialogueIntroManager : MonoBehaviour
{
    [Header("ShowDialogue(s)")]
    [SerializeField] ShowDialogue _titleDialogue;
    [SerializeField] ShowDialogue _bodyDialogue;

    [Header("DialogueSettings")]
    [SerializeField] DialogSettings[] _titleSettings;
    [SerializeField] DialogSettings[] _bodySettings;

    [Header("Eventos")]
    [SerializeField]
    private UnityEvent onDialoguesEnd = new UnityEvent();

    int _titleDialogueIndex = 0;
    int _bodyDialogueIndex = 0;

    private void SetUp()
    {
        _titleDialogueIndex = 0;
        _bodyDialogueIndex = 0;

        _titleDialogue.SetSettings(_titleSettings[_titleDialogueIndex]);
        _bodyDialogue.SetSettings(_bodySettings[_bodyDialogueIndex]);

        _titleDialogue.onDialogueEnd.AddListener(() => { _bodyDialogue.ShowText(); _bodyDialogue.waitForInteraction = false; });
        _bodyDialogue.onDialogueEnd.AddListener(AdvanceDialogues);

        _titleDialogue.waitForInteraction = false;
        _titleDialogue.waitTimeForNext = 0.0f;
        _bodyDialogue.waitForInteraction = false;
    }

    private void AdvanceDialogues()
    {
        if(_titleDialogueIndex + 1 >= _titleSettings.Length)
        {
            EndDialogues();
            return;
        }

        _titleDialogue.SetSettings(_titleSettings[++_titleDialogueIndex]);
        _bodyDialogue.SetSettings(_bodySettings[++_bodyDialogueIndex]);

        _titleDialogue.ShowText();
        _bodyDialogue.waitForInteraction = true;
        _bodyDialogue.QuitSkip();
    }

    private void EndDialogues()
    {
        onDialoguesEnd.Invoke();
    }

    public void StartDialogues()
    {
        SetUp();

        _titleDialogue.ShowText();
    }
}
