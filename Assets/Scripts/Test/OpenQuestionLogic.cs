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
		_questionText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", questionOpen.questionText);

		_responseInputField.text = "";
	}

	public EvaluationResult GetResults()
	{
		EvaluationResult result = new EvaluationResult();
		result.resultType = EvaluationResult.ResultType.STRING;
		result.resultText = _responseInputField.text;
		return result;
	}
}
