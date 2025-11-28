using UnityEngine;

public class Question : ScriptableObject
{
	public enum QuestionType
	{
		NONE = 0,
		LIKERT = 1,
		MULTIPLE_CHOICE = 2,
		OPEN_ENDED = 3
	}

	protected QuestionType _questionType = QuestionType.NONE;

	public QuestionType questionType
	{
		get { return _questionType; }
	}

	public string questionText;

	public bool showFeedback = false;
	public string explanation = "";
}
