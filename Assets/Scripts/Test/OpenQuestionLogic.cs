using TMPro;
using UnityEngine;

public class OpenQuestionLogic : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _questionText;

	[SerializeField]
	private TMP_InputField _responseInputField;

	private string _responseText = "";

	public void SetUp(Question question)
	{
		_questionText.text = question.questionText;

		_responseInputField.text = "";
		_responseText = "";
	}

	public void SetResponseText()
	{
		_responseText = _responseInputField.text;
	}

	public string GetResponseText()
	{
		return _responseText;
	}
}
