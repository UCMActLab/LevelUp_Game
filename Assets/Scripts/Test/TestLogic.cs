using UnityEngine;

public enum QuestionType
{
	NONE = 0,
	LIKERT = 1,
	MULTIPLE_CHOICE = 2,
	OPEN_ENDED = 3
}

public class TestLogic : MonoBehaviour
{
	[SerializeField]
    private Test _test;

	[SerializeField]
	private GameObject _testContainer;

	[SerializeField]
	private GameObject _likertQuestionPrefab;

	[SerializeField]
	private GameObject _MCQuestionPrefab;

	[SerializeField]
    private GameObject _openQuestionPrefab;

	private int _currentQuestionIndex = 0;

	private GameObject[] questions;

	private string[] results;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        if(_test == null)
        {
			Debug.LogError("TestLogic: No test assigned!");
			return;
		}

		questions = new GameObject[_test.questions.Length];
		results = new string[_test.questions.Length];

		// TODO encontrar alternativa para evitar GetComponent
        for (int i = 0; i < _test.questions.Length; i++)
        {
			Debug.Log("Question " + (i + 1) + ": " + _test.questions[i].questionText);
			GameObject question = null;
			switch (_test.questions[i].questionType)
            {
				case QuestionType.LIKERT:
					question = Instantiate(_likertQuestionPrefab, _testContainer.transform);
					question.GetComponent<LikertLogic>().SetUp(_test.questions[i]);
					break;
				case QuestionType.MULTIPLE_CHOICE:
					question = Instantiate(_MCQuestionPrefab, _testContainer.transform);
					question.GetComponent<MCLogic>().SetUp(_test.questions[i]);
					break;
				case QuestionType.OPEN_ENDED:
					question = Instantiate(_openQuestionPrefab, _testContainer.transform);
					question.GetComponent<OpenQuestionLogic>().SetUp(_test.questions[i]);
					break;
				default:
					Debug.LogWarning("Question - Type: Unknown");
					break;
			}
			questions[i] = question;
		}

		if(questions[_currentQuestionIndex] != null)
			questions[_currentQuestionIndex].SetActive(true);
    }

	public void NextQuestion()
	{
		if (_currentQuestionIndex < questions.Length - 1)
		{
			questions[_currentQuestionIndex].SetActive(false);
			_currentQuestionIndex++;
			questions[_currentQuestionIndex].SetActive(true);
		}
		// FINISH TEST
	}

	public void PreviousQuestion()
	{
		if (_currentQuestionIndex > 0)
		{
			questions[_currentQuestionIndex].SetActive(false);
			_currentQuestionIndex--;
			questions[_currentQuestionIndex].SetActive(true);
		}
	}
}
