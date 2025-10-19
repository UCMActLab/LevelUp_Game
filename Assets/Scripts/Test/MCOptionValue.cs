using TMPro;
using UnityEngine;

public class MCOptionValue : MonoBehaviour
{
    [SerializeField]
    private int _optionID;

    [SerializeField]
    private MCLogic _mcLogic;

	[SerializeField]
	private TMP_Text _optionText;

	public void SetValue(int id, string text)
    {
        _optionID = id;
        _optionText.text = text;
    }

	public void SetMCLogic(MCLogic logic)
	{
		_mcLogic = logic;
	}

	public void SendValue()
    {
        if(_mcLogic != null)
            _mcLogic.SetSelectedOption(_optionID);
    }
}
