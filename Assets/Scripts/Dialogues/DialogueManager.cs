using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private ShowDialog _showDialogues;

    public void StartDialog(DialogSettings settings)
    {
        ShowDialog dialog = Instance._showDialogues.GetComponent<ShowDialog>();
        dialog.SetSettings(settings);
        dialog.ShowText();
    }
}
