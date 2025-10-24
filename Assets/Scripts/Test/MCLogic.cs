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
	private GameObject _optionPrefab;

	[SerializeField]
	private int _selectedOptionID = -1;

	[SerializeField]
	private float _optionSpacing = 10f;

	private Toggle[] _optionToggles;

	private bool _changing = false;

	public void SetUp(Question question)
	{
		QuestionMC questionMC = question as QuestionMC;
		if(questionMC == null) 
		{
			Debug.LogError("MCLogic: Question is not of type QuestionMC!");
			return;
		}
		_questionText.text = question.questionText;

		_numberOfOptions = questionMC.answerOptions.Length;
		_optionToggles = new Toggle[_numberOfOptions];

		for (int i = 0; i < _numberOfOptions; i++)
		{
			GameObject option = Instantiate(_optionPrefab, _optionsParent.transform);

			option.transform.localPosition = new Vector3(0, -i * _optionSpacing, 0);

			// TODO encontrar alternativa para evitar GetComponent
			MCOptionValue mcVal = option.GetComponent<MCOptionValue>();
			mcVal.SetMCLogic(this);
			mcVal.SetValue(i, questionMC.answerOptions[i]);

			_optionToggles[i] = option.GetComponent<Toggle>();
		}
	}

	public string GetResults()
	{
		return _selectedOptionID.ToString();
	}

	public void SetSelectedOption(int optionID)
	{
		// added stop condition because of unity events -> the value ping pongs back and forth
		if (_changing)
			return;

		_selectedOptionID = optionID;

		_changing = true;

		ClearOptions();

		_changing = false;
	}

	private void ClearOptions()
	{
		for(int i = 0; i < _numberOfOptions; i++)
		{
			_optionToggles[i].isOn = i == _selectedOptionID;
		}
	}

	public int GetSelectedOption()
	{
		return _selectedOptionID;
	}
}
