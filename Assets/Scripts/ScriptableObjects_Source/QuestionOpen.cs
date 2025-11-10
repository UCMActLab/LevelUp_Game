using UnityEngine;

[CreateAssetMenu(fileName = "Open", menuName = "ScriptableObjects/Test/Questions/Open")]
public class QuestionOpen : Question
{
	[Header("Optional")]
	public string correctAnswer = "";

	public QuestionOpen()
	{
		_questionType = QuestionType.OPEN_ENDED;
	}
}
