using UnityEngine;

public class MCOptionValue : MonoBehaviour
{
    [SerializeField]
    private int _optionID;

    [SerializeField]
    private MCLogic _mcLogic;

    public void SetValue(int id)
    {
        _optionID = id;
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
