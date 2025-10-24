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

	[SerializeField]
	private GameObject _closingQuestion;

	[SerializeField]
	private GameObject _testButtons;

	private int _currentQuestionIndex = 0;

	private GameObject[] questions;

	private IQuestionLogic[] questionLogics;

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
		questionLogics = new IQuestionLogic[_test.questions.Length];
		results = new string[_test.questions.Length];

		// TODO encontrar alternativa para evitar GetComponent
        for (int i = 0; i < _test.questions.Length; i++)
        {
			GameObject question = null;
			switch (_test.questions[i].QuestionType)
            {
				case QuestionType.LIKERT:
					question = Instantiate(_likertQuestionPrefab, _testContainer.transform);
					questionLogics[i] = question.GetComponent<LikertLogic>();
					break;
				case QuestionType.MULTIPLE_CHOICE:
					question = Instantiate(_MCQuestionPrefab, _testContainer.transform);
					questionLogics[i] = question.GetComponent<MCLogic>();

					break;
				case QuestionType.OPEN_ENDED:
					question = Instantiate(_openQuestionPrefab, _testContainer.transform);
					questionLogics[i] = question.GetComponent<OpenQuestionLogic>();
					break;
				default:
					Debug.LogWarning("Question - Type: Unknown");
					break;
			}
			questionLogics[i].SetUp(_test.questions[i]);
			questions[i] = question;
		}

		if(questions[_currentQuestionIndex] != null)
			questions[_currentQuestionIndex].SetActive(true);
    }

	public void SubmitAnswers()
	{
		for (int i = 0; i < _test.questions.Length; i++)
		{
			results[i] = questionLogics[i].GetResults();
			Debug.Log("Answer submitted: " + results[i]);
		}
	}

	public void DisplayEnd()
	{
		questions[_currentQuestionIndex].SetActive(false);
		_testButtons.SetActive(false);
		_closingQuestion.SetActive(true);
	}

	public void ReturnToTest()
	{
		if (_currentQuestionIndex < questions.Length && _currentQuestionIndex > 0)
		{
			_closingQuestion.SetActive(false);
			_testButtons.SetActive(true);
			questions[_currentQuestionIndex].SetActive(true);
		}
		else
		{
			Debug.LogWarning("Questions to display out of range.");
		}
	}

	public void NextQuestion()
	{
		if (_currentQuestionIndex < questions.Length - 1)
		{
			questions[_currentQuestionIndex].SetActive(false);
			_currentQuestionIndex++;
			questions[_currentQuestionIndex].SetActive(true);
		}
		else
		{
			DisplayEnd();
		}
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
