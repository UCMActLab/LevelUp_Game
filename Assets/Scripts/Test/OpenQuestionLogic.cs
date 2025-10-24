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
		QuestionOpen questionOpen = question as QuestionOpen;
		if (questionOpen == null)
		{
			Debug.LogError("OpenQuestionLogic: Question is not of type QuestionOpen!");
			return;
		}
		_questionText.text = questionOpen.questionText;

		_responseInputField.text = "";
	}

	public string GetResults()
	{
		return _responseInputField.text;
	}
}
