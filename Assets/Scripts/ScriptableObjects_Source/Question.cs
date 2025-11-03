using UnityEngine;

public class Question : ScriptableObject
{
	protected QuestionType questionType = QuestionType.NONE;

	public QuestionType QuestionType
	{
		get { return questionType; }
	}

	public string questionText;
}
