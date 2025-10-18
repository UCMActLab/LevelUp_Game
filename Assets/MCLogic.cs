using UnityEngine;
using UnityEngine.UI;

public class MCLogic : MonoBehaviour
{
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

	private bool changing = false;

	private void Start()
	{
		SetUp();
	}

	public void SetUp()
	{
		_optionToggles = new Toggle[_numberOfOptions];
		for (int i = 0; i < _numberOfOptions; i++)
		{
			// TODO Instantiate Prefab with question (scriptable object or csv)
			GameObject option = Instantiate(_optionPrefab, _optionsParent.transform);

			option.transform.localPosition = new Vector3(0, -i * _optionSpacing, 0);

			MCOptionValue mcVal = option.GetComponent<MCOptionValue>();
			mcVal.SetMCLogic(this);
			mcVal.SetValue(i);

			_optionToggles[i] = option.GetComponent<Toggle>();
		}
	}

	public void SetSelectedOption(int optionID)
	{
		if (changing)
			return;

		_selectedOptionID = optionID;

		changing = true;

		ClearOptions();

		changing = false;
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
