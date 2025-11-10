using System;
using UnityEngine;
using UnityEngine.UI;

public class TestLogic : MonoBehaviour
{
	[SerializeField]
	private bool _setUpOnAwake = false;

	[SerializeField]
    private Test _test;

	[SerializeField] private GameObject _parentUI = null;

	[SerializeField]
	private GameObject _testContainer;

	[SerializeField]
	private GameObject _likertQuestionPrefab;

	[SerializeField]
	private GameObject _MCQuestionPrefab;

	[SerializeField]
    private GameObject _openQuestionPrefab;

	[SerializeField]
	private GameObject _feedbackQuestion;

	[SerializeField]
	private GameObject _closingQuestion;

	[SerializeField]
	private GameObject _testButtons;

	private int _currentQuestionIndex = 0;

	private GameObject[] _questions;

	private IQuestionLogic[] _questionLogics;

	[SerializeField]
	private QuestionFeedbackLogic _feedbackLogic;

	private EvaluationResult[] _results;

	private bool _showingFeedback = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_setUpOnAwake) SetUp();
    }

	public void SetTest(Test newTest, bool startQuiz)
	{
		_test = newTest;

		if (startQuiz) SetUp();
	}

    public void SetUp()
    {
        if(_test == null)
        {
			Debug.LogError("TestLogic: No test assigned!");
			return;
		}

		if (!_parentUI.activeSelf) _parentUI.SetActive(true);

		_questions = new GameObject[_test.questions.Length];
		_questionLogics = new IQuestionLogic[_test.questions.Length];
		_results = new EvaluationResult[_test.questions.Length];

		// TODO encontrar alternativa para evitar GetComponent
        for (int i = 0; i < _test.questions.Length; i++)
        {
			GameObject question = null;
			switch (_test.questions[i].questionType)
            {
				case Question.QuestionType.LIKERT:
					question = Instantiate(_likertQuestionPrefab, _testContainer.transform);
					_questionLogics[i] = question.GetComponent<LikertLogic>();
					break;
				case Question.QuestionType.MULTIPLE_CHOICE:
					question = Instantiate(_MCQuestionPrefab, _testContainer.transform);
					_questionLogics[i] = question.GetComponent<MCLogic>();

					break;
				case Question.QuestionType.OPEN_ENDED:
					question = Instantiate(_openQuestionPrefab, _testContainer.transform);
					_questionLogics[i] = question.GetComponent<OpenQuestionLogic>();
					break;
				default:
					Debug.LogWarning("Question - Type: Unknown");
					break;
			}
			_questionLogics[i].SetUp(_test.questions[i]);
			_questions[i] = question;
		}

		if(_questions[_currentQuestionIndex] != null)
			_questions[_currentQuestionIndex].SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_parentUI.transform as RectTransform);
        foreach (Transform tr in _parentUI.GetComponentsInChildren<Transform>(true))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tr as RectTransform);
        }
    }

	public void SubmitAnswers()
	{
		for (int i = 0; i < _test.questions.Length; i++)
		{
			_results[i] = _questionLogics[i].GetResults();
			switch (_results[i].resultType)
			{
				case EvaluationResult.ResultType.INT:
					Debug.Log("Answer submitted (int): " + _results[i].resultScore);
					break;
				case EvaluationResult.ResultType.BIT_MASK:
					Debug.Log("Answer submitted (bitmask): " + Convert.ToString(_results[i].bitmaskScore, 2).PadLeft(8, '0'));
					break;
				case EvaluationResult.ResultType.STRING:
					Debug.Log("Answer submitted (string): " + _results[i].resultText);
					break;
				default:
					Debug.Log("Answer submitted: Unknown type");
					break;
			}
		}
		AnalyticsManager.Instance.SubmitTestResults(_test, _results);
	}

	public void DisplayEnd()
	{
		_questions[_currentQuestionIndex].SetActive(false);
		_testButtons.SetActive(false);
		_closingQuestion.SetActive(true);
	}

	public void ReturnToTest()
	{
		if (_currentQuestionIndex < _questions.Length && _currentQuestionIndex > 0)
		{
			_closingQuestion.SetActive(false);
			_testButtons.SetActive(true);
			_questions[_currentQuestionIndex].SetActive(true);
		}
		else
		{
			Debug.LogWarning("Questions to display out of range.");
		}
	}

	public void DisplayFeedback()
	{
		_showingFeedback = true;
		_questions[_currentQuestionIndex].SetActive(false);
		_questionLogics[_currentQuestionIndex].LockQuestion();

		IQuestionLogic currentLogic = _questionLogics[_currentQuestionIndex];

		_feedbackLogic.SetUp(currentLogic.IsCorrect(), currentLogic.GetCorrectResponse(), _test.questions[_currentQuestionIndex].explanation);

		_feedbackQuestion.SetActive(true);
	}

	public void NextQuestion()
	{
		if (_test.questions[_currentQuestionIndex].showFeedback && !_showingFeedback)
		{
			DisplayFeedback();
			return;
		}

		if (_showingFeedback)
		{
			_showingFeedback = false;
			_feedbackQuestion.SetActive(false);
		}

		if (_currentQuestionIndex < _questions.Length - 1)
		{
			_questions[_currentQuestionIndex].SetActive(false);
			_currentQuestionIndex++;
			_questions[_currentQuestionIndex].SetActive(true);
		}
		else
		{
			DisplayEnd();
		}
	}

	public void PreviousQuestion()
	{
		if (_showingFeedback)
		{
			_showingFeedback = false;
			_feedbackQuestion.SetActive(false);
			_currentQuestionIndex++;
		}

		if (_currentQuestionIndex > 0)
		{
			if(_currentQuestionIndex < _questions.Length)
				_questions[_currentQuestionIndex].SetActive(false);
			_currentQuestionIndex--;
			_questions[_currentQuestionIndex].SetActive(true);
		}
	}
}
