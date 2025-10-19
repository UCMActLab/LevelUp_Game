using UnityEngine;

[CreateAssetMenu(fileName = "OpenQuestion", menuName = "ScriptableObjects/Test/OpenQuestion")]
public class OpenQuestion : ScriptableObject
{
	private QuestionType questionType = QuestionType.OPEN_ENDED;

	public string questionText;
}
