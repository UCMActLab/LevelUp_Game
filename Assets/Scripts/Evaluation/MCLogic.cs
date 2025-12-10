using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MCLogic : MonoBehaviour, IQuestionLogic
{
	[SerializeField]
	private TMP_Text _questionText;

	[SerializeField]
	private int _numberOfOptions;

	[SerializeField]
	private GameObject _optionsParent;

	[SerializeField]
	private GameObject _multipleOptionPrefab;

	[SerializeField]
	private GameObject _singleOptionPrefab;

	[SerializeField]
	private int _selectedOptionID = -1;

	private uint _optionBitmask = 0;

	[SerializeField]
	private float _optionSpacing = 10f;

	private Toggle[] _optionToggles;

	private Button[] _optionButtons;

	private bool _changing = false;

	private bool _multipleSelection = false;

	private bool _optionsAsButtons = false;

	private QuestionMC _questionMC;

	private TestLogic _testLogic = null;

	public bool OptionsAsButtons { get { return _optionsAsButtons; } }

	public void SetUp(Question question)
	{
		_questionMC = question as QuestionMC;
		if(_questionMC == null) 
		{
			Debug.LogError("MCLogic: Question is not of type QuestionMC!");
			return;
		}
		Debug.Log(_questionMC.questionText);
		_questionText.text = TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionMC.questionText);

		_numberOfOptions = _questionMC.answerOptions.Length;

		_multipleSelection = _questionMC.allowMultipleSelections;

		_optionsAsButtons = _questionMC.optionsAsButtons;

		GameObject optionPrefab = _multipleOptionPrefab;

		if(_optionsAsButtons)
		{
			_optionButtons = new Button[_numberOfOptions];
			optionPrefab = _singleOptionPrefab;
		}
		else
		{
			_optionToggles = new Toggle[_numberOfOptions];
		}

		for (int i = 0; i < _numberOfOptions; i++)
		{
			GameObject option = Instantiate(optionPrefab, _optionsParent.transform);

			// option.transform.localPosition = new Vector3(0, -i * _optionSpacing, 0);

			// TODO encontrar alternativa para evitar GetComponent
			MCOptionValue mcVal = option.GetComponent<MCOptionValue>();
			mcVal.SetMCLogic(this);

            Debug.Log(_questionMC.answerOptions[i].optionText);
            mcVal.SetValue(i, TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionMC.answerOptions[i].optionText));

			if (_optionsAsButtons)
			{
				_optionButtons[i] = option.GetComponent<Button>();
			}
			else
			{
				_optionToggles[i] = option.GetComponent<Toggle>();
			}
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
	}

	public EvaluationResult GetResults()
	{
		EvaluationResult result = new EvaluationResult();
		result.resultType = EvaluationResult.ResultType.BIT_MASK;
		result.bitmaskScore = _optionBitmask;
		return result;
	}

	public void SetSelectedOption(int optionID)
	{
		// added stop condition because of unity events -> the value ping pongs back and forth
		if (_changing)
			return;

		if(_multipleSelection && !_optionsAsButtons)
		{
			UpdateSelectionMultiple(optionID);
		}
		else
		{
			UpdateSelectionSingle(optionID);
		}
	}

	private void UpdateSelectionSingle(int optionID)
	{
		if (_selectedOptionID == optionID)
		{
			_optionBitmask = 0;
			_selectedOptionID = -1;
		}
		else
		{
			_selectedOptionID = optionID;
			_optionBitmask = (uint)(1 << _selectedOptionID);
		}

		if(_optionsAsButtons && _testLogic != null)
		{
			_testLogic.NextQuestion();
		}
		else
		{
			_changing = true;
			ClearOptions();
			_changing = false;
		}

	}

	private void UpdateSelectionMultiple(int optionID)
	{
		int maskCheck = (int)Mathf.Pow(2, optionID);
		if ((_optionBitmask & (uint)(1 << optionID)) == maskCheck)
		{
			_optionBitmask -= (uint)(1 << optionID);
			_changing = true;
			_optionToggles[optionID].isOn = false;
			_changing = false;
		}
		else
		{
			_optionBitmask = _optionBitmask | (uint)(1 << optionID);
			_changing = true;
			_optionToggles[optionID].isOn = true;
			_changing = false;
		}
	}

	private void ClearOptions()
	{
		for(int i = 0; i < _numberOfOptions; i++)
		{
			_optionToggles[i].isOn = i == _selectedOptionID;
		}
	}

	public uint GetOptionBitmask()
	{
		return _optionBitmask;
	}

	public int GetSelectedOption()
	{
		return _selectedOptionID;
	}

	public void LockQuestion()
	{
		if(_optionsAsButtons)
		{
			for (int i = 0; i < _numberOfOptions; i++)
			{
				_optionButtons[i].interactable = false;
			}
		}
		else
		{
			for (int i = 0; i < _numberOfOptions; i++)
			{
				_optionToggles[i].interactable = false;
			}
		}
	}

	public bool IsCorrect()
	{
		uint result = 0;
		for(int i = 0; i < _numberOfOptions; i++)
		{
			if (_questionMC.answerOptions[i].isCorrect)
			{
				result = result | (uint)(1 << i);
			}
		}
		return result == _optionBitmask;
	}

	public string GetCorrectResponse()
	{
		string correctResponses = "";
		bool first = true;
		for (int i = 0; i < _numberOfOptions; i++)
		{
			if (_questionMC.answerOptions[i].isCorrect)
			{
				if(!first)
				{
					correctResponses += ", ";
				}
				correctResponses += TranslationManager.Instance.GetLocalizedStringValue("EVALUATION", _questionMC.answerOptions[i].optionText);
				first = false;
			}
		}	
		return correctResponses;
	}

	public void SetTestLogic(TestLogic testLogic)
	{
		_testLogic = testLogic;
	}
}
