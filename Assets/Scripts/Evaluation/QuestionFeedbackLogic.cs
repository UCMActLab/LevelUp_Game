using TMPro;
using UnityEngine;

public class QuestionFeedbackLogic : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _headerText;

    [SerializeField]
    private Color _correctColor = Color.green;

	[SerializeField]
	private Color _incorrectColor = Color.red;

	[SerializeField]
	private TMP_Text _correctResponseText;

	[SerializeField]
    private TMP_Text _explanationText;

    [SerializeField]
    private GameObject _explanationBody;

    public void SetUp(bool correct, string correctOptions, string explanation)
    {
        if(correct)
        {
            _headerText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "CORRECT");
            _headerText.color = _correctColor;
            _correctResponseText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "CORRECT_OPTIONS");

		}
        else
        {
            _headerText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "INCORRECT");
            _headerText.color = _incorrectColor;
			_correctResponseText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "INCORRECT_OPTIONS") + "\n\"<b>" + correctOptions + "\"<b>";
		}
        try
        {
            _explanationText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", explanation);
        }
        catch
        {
            bool hasExplanation = explanation != string.Empty;
            _explanationBody.SetActive(hasExplanation);
			_explanationText.text = explanation;
		}

    }
}
