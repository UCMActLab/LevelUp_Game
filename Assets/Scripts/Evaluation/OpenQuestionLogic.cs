using TMPro;
using UnityEngine;

public class OpenQuestionLogic : MonoBehaviour, IQuestionLogic
{
	[SerializeField]
	private TMP_Text _questionText;

	[SerializeField]
	private TMP_InputField _responseInputField;

	[SerializeField]
	private TMP_Text _placeholderText;

	private QuestionOpen _questionOpen;

	public void SetUp(Question question)
	{
		_questionOpen = question as QuestionOpen;
		if (_questionOpen == null)
		{
			Debug.LogError("OpenQuestionLogic: Question is not of type QuestionOpen!");
			return;
		}
		_questionText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionOpen.questionText);

		_placeholderText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "INPUT_TEXT_PH");

		_responseInputField.text = "";
	}

	public EvaluationResult GetResults()
	{
		EvaluationResult result = new EvaluationResult();
		result.resultType = EvaluationResult.ResultType.STRING;
		result.resultText = _responseInputField.text;
		return result;
	}

	public void LockQuestion()
	{
		_responseInputField.interactable = false;
	}

	public bool IsCorrect()
	{
		// TODO: Localization?
		return _questionOpen.correctAnswer == _responseInputField.text;
	}

	public string GetCorrectResponse()
	{
		return _questionOpen.correctAnswer;
	}
}
