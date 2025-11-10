using UnityEngine;

[System.Serializable]
public struct OptionMC
{
	public string optionText;

	public bool isCorrect;
}

[CreateAssetMenu(fileName = "Multiple Choice", menuName = "ScriptableObjects/Test/Questions/MultipleChoice")]
public class QuestionMC : Question
{
	public bool allowMultipleSelections = false;

	public OptionMC[] answerOptions;

	public QuestionMC()
	{
		_questionType = QuestionType.MULTIPLE_CHOICE;
	}
}
