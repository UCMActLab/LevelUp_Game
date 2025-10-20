using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/Test/Question")]
public class Question : ScriptableObject
{
	public QuestionType questionType = QuestionType.NONE;

	public string questionText;

	// TODO inheritance would be better here but enums have problems with overriding (main focus for using inheritance), find alternatives

	// For Multiple Choice questions

	public string[] answerOptions;

	// For Likert Scale questions

	public string leftLablel = "Muy en desacuerdo";
	public string rightLabel = "Muy de acuerdo";

	public int leftValue = 1;
	public int rightValue = 5;

	public int defaultValue = 3;
}
