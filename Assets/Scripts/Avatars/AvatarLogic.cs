using UnityEngine;

public class AvatarLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _avatarBaseContainer;

    [SerializeField]
    private GameObject _avatarHairContainer;

	[SerializeField]
	private GameObject _avatarFaceContainer;

	[SerializeField]
	private GameObject _avatarClothesContainer;

	private GameObject _currentGameObject;

	public void Start()
	{
		SetUp();
	}

	public void SetUp()
	{
		_currentGameObject = _avatarBaseContainer;
	}

	public void showBaseFeatures()
	{
		if(_currentGameObject != null)
			_currentGameObject.SetActive(false);
		_avatarBaseContainer.SetActive(true);
		_currentGameObject = _avatarBaseContainer;
	}

	public void showHairFeatures()
	{
		if (_currentGameObject != null)
			_currentGameObject.SetActive(false);
		_avatarHairContainer.SetActive(true);
		_currentGameObject = _avatarHairContainer;
	}

	public void showFaceFeatures()
	{
		if (_currentGameObject != null)
			_currentGameObject.SetActive(false);
		_avatarFaceContainer.SetActive(true);
		_currentGameObject = _avatarFaceContainer;
	}

	public void showClothesFeatures()
	{
		if (_currentGameObject != null)
			_currentGameObject.SetActive(false);
		_avatarClothesContainer.SetActive(true);
		_currentGameObject = _avatarClothesContainer;
	}
}
