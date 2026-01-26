using DA_Assets.Extensions;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class TestLogic : MonoBehaviour
{
	[SerializeField]
	private bool _setUpOnAwake = false;

	[SerializeField]
    private Test _test;

	[SerializeField] 
	private GameObject _parentUI = null;

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

	private GameObject[] _questions = null;

	private IQuestionLogic[] _questionLogics;

	[SerializeField]
	private QuestionFeedbackLogic _feedbackLogic;

	private EvaluationResult[] _results;

	private bool _showingFeedback = false;

	public UnityEvent OnTestEnd = new UnityEvent();

	Stack<QuestionMC> _trueFalseQuestions;
	Stack<QuestionMC> _multipleChoiceQuestions;

	[SerializeField]
    private GameObject _scoreTracker = null;
	[SerializeField]
	private GameObject _footer = null;

    void Start()
    {
		_questions = null;

		CreateQuestions();

        if (_setUpOnAwake) SetUp();

    }

    private void OnEnable()
    {
		OnTestEnd.AddListener(() => { _footer.SetActive(true); _scoreTracker.SetActive(true); });
    }

    private void OnDisable()
    {
		OnTestEnd.RemoveAllListeners();
    }

    public void SetTest(Test newTest, bool startQuiz)
	{
		_test = newTest;

		if (startQuiz) SetUp();
	}

	private void CreateQuestions()
	{
		var allQuestions = TranslationManager.Instance.GetAllTableEntries("EVALUATION");

        Dictionary<int, QuestionMC> questions = new Dictionary<int, QuestionMC>();

        foreach (StringTableEntry v in allQuestions)
        {
            foreach (var metadata in v.MetadataEntries)
            {
                Debug.Log(metadata);
            }

            string key = v.Key;
            string[] keySplitUnderscore = key.Split('_');
            if (keySplitUnderscore[0] != "QUESTION") { continue; }
            else
            {
                int id = int.Parse(keySplitUnderscore[1].Split('/')[0]);
                string[] keySplitBar = key.Split('/');

                QuestionMC question = null;
                if (keySplitBar.Length == 1)
                {
                    question = ScriptableObject.CreateInstance("QuestionMC") as QuestionMC;
					question.name = key;
                    question.questionText = key;

                    if (question.answerOptions == null) question.answerOptions = new List<OptionMC>();

                    if (v.SharedEntry.Metadata.MetadataEntries.Count > 0)
                    {
                        OptionMC optionFalse = new OptionMC();
                        optionFalse.optionText = "FALSE";
                        optionFalse.isCorrect = v.SharedEntry.Metadata.MetadataEntries[0].ToString() == "FALSE";

                        OptionMC optionTrue = new OptionMC();
                        optionTrue.optionText = "TRUE";
                        optionTrue.isCorrect = !optionFalse.isCorrect;

                        question.answerOptions.Add(optionTrue);
                        question.answerOptions.Add(optionFalse);
                    }

                    questions.Add(id, question);
                }
                else
                {
                    question = questions[id];
                    string nature = keySplitBar[1];
                    if (nature == "POSITIVE_FEEDBACK")
                    {
                        question.positiveExplanation = key;
                    }
                    else if (nature == "NEGATIVE_FEEDBACK")
                    {
                        question.negativeExplanation = key;
                    }
                    else
                    {
                        string answer = nature.Split('_')[0];
                        if (answer == "ANSWER")
                        {
                            OptionMC option = new OptionMC();
                            option.optionText = key;
                            option.isCorrect = v.SharedEntry.Metadata.MetadataEntries[0].ToString() == "TRUE";

                            question.answerOptions.Add(option);
                        }
                    }
                }
            }
        }

		List<QuestionMC> trueFalse = new List<QuestionMC>();
		List<QuestionMC> multipleChoice = new List<QuestionMC>();

        foreach (QuestionMC question in questions.Values)
        {
            question.showFeedback = true;
            question.optionsAsButtons = true;
            question.allowMultipleSelections = false;

            int numCorrect = 0;
            foreach (OptionMC option in question.answerOptions)
            {
                if (option.isCorrect) numCorrect++;

                if (numCorrect > 1)
                {
                    question.optionsAsButtons = false;
                    question.allowMultipleSelections = true;
                    break;
                }
            }

			if (numCorrect == 1 && question.answerOptions.Count == 2) trueFalse.Add(question);
			else {
				Debug.Log(question.questionText);
				multipleChoice.Add(question);
			}
        }

		trueFalse.Shuffle();
		multipleChoice.Shuffle();

        _trueFalseQuestions = new Stack<QuestionMC>(trueFalse);
        _multipleChoiceQuestions = new Stack<QuestionMC>(multipleChoice);

		Debug.Log("Number of questions created: True/False: " + _trueFalseQuestions.Count.ToString() + " Multiple Choice: " + _multipleChoiceQuestions.Count.ToString());
    }

    public void SetUp()
    {
        if(_test == null)
        {
			Debug.LogError("TestLogic: No test assigned!");
			return;
		}

		if (!_parentUI.activeSelf) _parentUI.SetActive(true);

		if(_questions != null) foreach (GameObject go in _questions) { Destroy(go); }

		int totalQuestions = Mathf.Min(_test.TotalQuestions, _trueFalseQuestions.Count + _multipleChoiceQuestions.Count);

		_questions = new GameObject[totalQuestions];
		_questionLogics = new IQuestionLogic[totalQuestions];
		_results = new EvaluationResult[totalQuestions];

		_test.questions = new Question[totalQuestions];
        for (int i = 0; i < totalQuestions; i++)
        {
			GameObject question = Instantiate(_MCQuestionPrefab, _testContainer.transform);

			_questionLogics[i] = question.GetComponent<MCLogic>();
			(_questionLogics[i] as MCLogic).SetTestLogic(this);

			Question q = null;


			if (_trueFalseQuestions.Count > 0 && (i < _test.numTrueFalseQuestions || _multipleChoiceQuestions.Count == 0)) q = _trueFalseQuestions.Pop();
			else if (_multipleChoiceQuestions.Count > 0) q = _multipleChoiceQuestions.Pop();

			_questionLogics[i].SetUp(q);
			_test.questions[i] = q;

			_questions[i] = question;
		}

		if(_questions[_currentQuestionIndex] != null)
		{
			_questions[_currentQuestionIndex].SetActive(true);
			ActivateTestButtonsIfOptionsAreNotButtons();
		}

        LayoutRebuilder.ForceRebuildLayoutImmediate(_parentUI.transform as RectTransform);
        foreach (Transform tr in _parentUI.GetComponentsInChildren<Transform>(true))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(tr as RectTransform);
		}

		_footer.SetActive(false);
		_scoreTracker.SetActive(false);
	}

	private void ActivateTestButtonsIfOptionsAreNotButtons()
	{
        if (_questionLogics[_currentQuestionIndex] is MCLogic)
        {
            _testButtons.SetActive(!(_questionLogics[_currentQuestionIndex] as MCLogic).OptionsAsButtons);
        }
    }

	public void SubmitAnswers()
	{
		for (int i = 0; i < _test.TotalQuestions; i++)
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

			CustomEvent customEvent = new CustomEvent("Old_Question_Answer")
			{
				{ "QuestionType", (int)_results[i].resultType },
				{ "Result_MC", (int)_results[i].bitmaskScore },
				{ "QuestionID", _test.questions[i].name }
			};

			AnalyticsManager.Instance.SubmitEvent(customEvent);
		}
	}

	public void DisplayEnd()
	{
		_questions[_currentQuestionIndex].SetActive(false);
        _parentUI.SetActive(false);
		_currentQuestionIndex = 0;

		OnTestEnd.Invoke();
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

		_footer.SetActive(true);
		_scoreTracker.SetActive(true);

		_testButtons.SetActive(false);

		IQuestionLogic currentLogic = _questionLogics[_currentQuestionIndex];

		bool correct = currentLogic.IsCorrect();
		string feedback = string.Empty;

		if (correct) feedback = _test.questions[_currentQuestionIndex].positiveExplanation;
		else feedback = _test.questions[_currentQuestionIndex].negativeExplanation;

		_feedbackLogic.SetUp(correct, currentLogic.GetCorrectResponse(), feedback);

		// _feedbackQuestion.SetActive(true);
	}

	public void NextQuestion()
	{
        _footer.SetActive(false);
        _scoreTracker.SetActive(false);

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

			ActivateTestButtonsIfOptionsAreNotButtons();
			_questions[_currentQuestionIndex].SetActive(true);
			Debug.Log(_test.questions[_currentQuestionIndex].name);
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
