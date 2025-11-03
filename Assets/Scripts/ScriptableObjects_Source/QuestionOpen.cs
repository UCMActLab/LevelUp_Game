using UnityEngine;

[CreateAssetMenu(fileName = "Open", menuName = "ScriptableObjects/Test/Questions/Open")]
public class QuestionOpen : Question
{
	// No additional fields needed for open-ended questions

	public QuestionOpen()
	{
		questionType = QuestionType.OPEN_ENDED;
	}
}
