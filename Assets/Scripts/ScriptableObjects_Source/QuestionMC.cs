using UnityEngine;

[CreateAssetMenu(fileName = "Multiple Choice", menuName = "ScriptableObjects/Test/Questions/MultipleChoice")]
public class QuestionMC : Question
{
	public bool allowMultipleSelections = false;

	public string[] answerOptions;

	public QuestionMC()
	{
		questionType = QuestionType.MULTIPLE_CHOICE;
	}
}
