using UnityEngine;

[CreateAssetMenu(fileName = "Likert", menuName = "ScriptableObjects/Test/Questions/Likert")]
public class QuestionLikert : Question
{
	public string leftLablel = "Muy en desacuerdo";
	public string rightLabel = "Muy de acuerdo";

	public int leftValue = 1;
	public int rightValue = 5;

	public int defaultValue = 3;

	public QuestionLikert()
	{
		_questionType = QuestionType.LIKERT;
	}
}
