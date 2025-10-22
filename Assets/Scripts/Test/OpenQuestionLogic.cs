using TMPro;
using UnityEngine;

public class OpenQuestionLogic : MonoBehaviour, IQuestionLogic
{
	[SerializeField]
	private TMP_Text _questionText;

	[SerializeField]
	private TMP_InputField _responseInputField;

	public void SetUp(Question question)
	{
		_questionText.text = question.questionText;

		_responseInputField.text = "";
	}

	public string GetResults()
	{
		return _responseInputField.text;
	}
}
