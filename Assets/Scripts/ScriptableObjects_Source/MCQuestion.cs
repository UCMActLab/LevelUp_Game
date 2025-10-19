using UnityEngine;

[CreateAssetMenu(fileName = "MCQuestion", menuName = "ScriptableObjects/Test/MCQuestion")]
public class MCQuestion : ScriptableObject
{
	private QuestionType questionType = QuestionType.MULTIPLE_CHOICE;

	public string questionText;

	public string[] answerOptions;
}
