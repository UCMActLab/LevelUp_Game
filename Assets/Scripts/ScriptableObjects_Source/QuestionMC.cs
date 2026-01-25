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

	public bool optionsAsButtons = false;

	[Space(10, order = 0)]
	[Header("[Option As Buttons] overwrites [Allow Multiple Selection] and will only allow one response", order = 1)]
	[Space(30, order = 2)]

	public System.Collections.Generic.List<OptionMC> answerOptions;

	public QuestionMC()
	{
		_questionType = QuestionType.MULTIPLE_CHOICE;
	}
}
