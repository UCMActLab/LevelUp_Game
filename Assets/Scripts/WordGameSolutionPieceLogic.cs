using TMPro;
using UnityEngine;

public class WordGameSolutionPieceLogic : MonoBehaviour
{
	[SerializeField]
	private TMP_Text optionText;

	private string value = "";

	private int indexInSolution;

	private WordGameLogic wordGameLogic;

	public void SetUp(string defaultText, int index, WordGameLogic logic)
	{
		optionText.text = defaultText;
		indexInSolution = index;
		wordGameLogic = logic;
	}

	public void updateValue(string text)
	{
		value = text;
		optionText.text = text;
	}

	public string GetValue()
	{
		return value;
	}

	public void OnSelection()
	{
		if(wordGameLogic == null)
		{
			Debug.LogError("WordGameLogic reference is missing!");
			return;
		}

		wordGameLogic.RemoveOptionFromSolution(indexInSolution);
	}
}
