using DA_Assets.Extensions;
using System;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
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
	private GameObject _closingQuestion;

	[SerializeField]
	private GameObject _testButtons;

	private int _currentQuestionIndex = 0;

	private GameObject[] questions;

	private IQuestionLogic[] questionLogics;

	private EvaluationResult[] results;

	public UnityEvent OnTestEnd = new UnityEvent();

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
		
		questions = new GameObject[_test.questions.Length];
		questionLogics = new IQuestionLogic[_test.questions.Length];
		results = new EvaluationResult[_test.questions.Length];

		_testContainer.DestroyChilds();
		// TODO encontrar alternativa para evitar GetComponent
        for (int i = 0; i < _test.questions.Length; i++)
        {
			GameObject question = null;
			switch (_test.questions[i].questionType)
            {
				case Question.QuestionType.LIKERT:
					question = Instantiate(_likertQuestionPrefab, _testContainer.transform);
					questionLogics[i] = question.GetComponent<LikertLogic>();
					break;
				case Question.QuestionType.MULTIPLE_CHOICE:
					question = Instantiate(_MCQuestionPrefab, _testContainer.transform);
					questionLogics[i] = question.GetComponent<MCLogic>();

					break;
				case Question.QuestionType.OPEN_ENDED:
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
			results[i] = questionLogics[i].GetResults();
			switch (results[i].resultType)
			{
				case EvaluationResult.ResultType.INT:
					Debug.Log("Answer submitted (int): " + results[i].resultScore);
					break;
				case EvaluationResult.ResultType.BIT_MASK:
					Debug.Log("Answer submitted (bitmask): " + Convert.ToString(results[i].bitmaskScore, 2).PadLeft(8, '0'));
					break;
				case EvaluationResult.ResultType.STRING:
					Debug.Log("Answer submitted (string): " + results[i].resultText);
					break;
				default:
					Debug.Log("Answer submitted: Unknown type");
					break;
			}

			CustomEvent customEvent = new CustomEvent("Old_Question_Answer")
			{
				{ "QuestionType", (int)results[i].resultType },
				{ "Result_MC", (int)results[i].bitmaskScore },
				{ "QuestionID", _test.questions[i].name }
			};

			AnalyticsManager.Instance.SubmitEvent(customEvent);
		}


		// AnalyticsManager.Instance.SubmitTestResults(_test, results);
	}

	public void DisplayEnd()
	{
		questions[_currentQuestionIndex].SetActive(false);
		//  _testButtons.SetActive(false);
        // _closingQuestion.SetActive(true);
        _parentUI.SetActive(false);
		_currentQuestionIndex = 0;

		OnTestEnd.Invoke();
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
