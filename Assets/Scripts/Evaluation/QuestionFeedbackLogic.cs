using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionFeedbackLogic : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _headerText;

	[SerializeField]
	private TMP_Text _correctResponseText;

	[SerializeField]
    private TMP_Text _explanationText;

    [SerializeField]
    private GameObject _explanationBody;

    [SerializeField]
    private TestLogic _testLogic;

    [SerializeField]
    private GameAssistant _assistant;

    public void SetUp(bool correct, string correctOptions, string explanation)
    {
        string title = string.Empty;
        List<string> messages = new List<string>();

        try
        {
            string expl = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", explanation);
            messages.Add(expl);
        }
        catch
        {
            Debug.LogWarning("No feedback found for this question");

            if (correct)
            {
                messages.Add(TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "CORRECT"));

                ScoreManager.Instance.AnsweredQuestionRight();
            }
            else
            {
                messages.Add(TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "INCORRECT"));
                messages.Add(TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", "INCORRECT_OPTIONS"));
                messages.Add(correctOptions);

            }
        }
       
        _assistant.ShowMessagesOneShot(messages.ToArray(), _testLogic.NextQuestion);

    }
}
