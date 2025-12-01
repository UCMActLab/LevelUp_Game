using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordGameOptionLogic : MonoBehaviour
{
    [SerializeField]
    private TMP_Text optionText;

    [SerializeField]
    private Button optionButton;

    private WordGameLogic wordGameLogic;

    private string value;

    public void SetUp(string letter, WordGameLogic logic)
    {
        value = letter;
		optionText.text = letter;
		wordGameLogic = logic;
	}

    public void OnOptionSelected()
    {
        if(wordGameLogic == null)
        {
			Debug.LogError("WordGameLogic reference is missing!");
			return;
		}
		optionButton.interactable = !wordGameLogic.OnOptionSelected(value, this);
	}

    public void Reactivate()
    {
        optionButton.interactable = true;
    }
}
