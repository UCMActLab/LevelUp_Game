using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarLogic : MonoBehaviour
{
	[Header("Containers")]

    [SerializeField]
    private GameObject _avatarBaseContainer;

	[SerializeField]
    private GameObject _avatarHairContainer;

	[SerializeField]
	private GameObject _avatarFaceContainer;

	[SerializeField]
	private GameObject _avatarClothesContainer;

	private GameObject _currentGameObject;

	[Header("General")]

	[SerializeField]
	private GameObject _avatarSelectorPrefab;

	[SerializeField]
	private Sprite _colorImage;

	[SerializeField]
	private GameObject _resultParent;

	[Header("Avatar")]

	[SerializeField]
	private GameObject _avatar;

	[SerializeField]
	private Image _avatarFace;

	[SerializeField]
	private Image _avatarHair;

	[SerializeField]
	private Image _avatarEyes;

	[SerializeField]
	private Image _avatarNose;

	[SerializeField]
	private Image _avatarClothes;

	[SerializeField]
	private Image _avatarAcessory;

	[Header("Base")]

	[SerializeField]
	private GameObject _baseFaceContainer;

	[SerializeField]
	private GameObject _baseColorContainer;

	[SerializeField]
	private Sprite[] _faceShapes;

	private AvatarSelector[] _faces;

	private int _selectedFace = 0;

	[SerializeField]
	private Color[] _faceColors;

	private AvatarSelector[] _selectorFaceColor;

	private int _selectedFaceColor = 0;
	
	[SerializeField]
	private int _defaultFaceIndex = 0;

	[SerializeField]
	private int _defaultSkinColorIndex = 0;

	[Header("Hair")]

	[SerializeField]
	private GameObject _hairTypesContainer;

	[SerializeField]
	private GameObject _hairColorContainer;

	[SerializeField]
	private Sprite[] _hairShapes;

	private AvatarSelector[] _hairs;

	private int _selectedHair = 0;

	[SerializeField]
	private Color[] _hairColors;

	private AvatarSelector[] _selectorHairColor;

	private int _selectedHairColor = 0;

	[SerializeField]
	private int _defaultHairIndex = 0;
	
	[SerializeField]
	private int _defaultHairColorIndex = 0;

	[Header("Face")]

	[SerializeField]
	private GameObject _faceEyesContainer;

	[SerializeField]
	private GameObject _faceColorEyesContainer;

	[SerializeField]
	private GameObject _faceNoseContainer;

	[SerializeField]
	private Sprite[] _eyesShapes;

	private AvatarSelector[] _eyes;

	private int _selectedEyes = 0;

	[SerializeField]
	private Color[] _eyesColors;

	private AvatarSelector[] _selectorEyesColor;

	private int _selectedEyesColor = 0;

	[SerializeField]
	private Sprite[] _nosesShapes;

	private AvatarSelector[] _noses;

	private int _selectedNose = 0;

	[SerializeField]
	private int _defaultEyesIndex = 0;

	[SerializeField]
	private int _defaultEyesColorIndex = 0;

	[SerializeField]
	private int _defaultNoseIndex = 0;

	[Header("Clothes")]

	[SerializeField]
	private GameObject _clothesContainer;

	[SerializeField]
	private GameObject _clothesColourContainer;

	[SerializeField]
	private GameObject _clothesAccesoriesContainer;

	[SerializeField]
	private Sprite[] _clothesShapes;

	private AvatarSelector[] _clothes;

	private int _selectedClothes = 0;

	[SerializeField]
	private Color[] _clothesColors;

	private AvatarSelector[] _selectorClothesColor;

	private int _selectedClothesColor = 0;

	[SerializeField]
	private Sprite[] _accesoriesShapes;

	private AvatarSelector[] _accesories;

	private int _selectedAccesory = 0;

	[SerializeField]
	private int _defaultClothesIndex = 0;

	[SerializeField]
	private int _defaultClothesColorIndex = 0;

	[SerializeField]
	private int _defaultAccesoriesIndex = 0;

	public void Start()
	{
		SetUp();
	}

	public void SetUp()
	{
		_currentGameObject = _avatarBaseContainer;

		_faces = new AvatarSelector[_faceShapes.Length];
		_selectorFaceColor = new AvatarSelector[_faceColors.Length];

		_hairs = new AvatarSelector[_hairShapes.Length];
		_selectorHairColor = new AvatarSelector[_hairColors.Length];

		_eyes = new AvatarSelector[_eyesShapes.Length];
		_selectorEyesColor = new AvatarSelector[_eyesColors.Length];
		_noses = new AvatarSelector[_nosesShapes.Length];

		_clothes = new AvatarSelector[_clothesShapes.Length];
		_selectorClothesColor = new AvatarSelector[_clothesColors.Length];
		_accesories = new AvatarSelector[_accesoriesShapes.Length];
		
		// Base
		for (int i = 0; i < _faceColors.Length; i++)
		{
			GameObject color = Instantiate(_avatarSelectorPrefab, _baseColorContainer.transform);
			_selectorFaceColor[i] = color.GetComponent<AvatarSelector>();

			_selectorFaceColor[i].SetUp(this, i, AvatarSelector.SelectorCategory.FaceColor, _colorImage, i == 0, _faceColors[i]);
		}

		for (int i = 0; i < _faceShapes.Length; i++)
		{
			GameObject face = Instantiate(_avatarSelectorPrefab, _baseFaceContainer.transform);
			_faces[i] = face.GetComponent<AvatarSelector>();

			_faces[i].SetUp(this, i, AvatarSelector.SelectorCategory.Face, _faceShapes[i], i==0, _faceColors[0]);
		}

		_avatarFace.sprite = _faceShapes[0];
		_avatarFace.SetNativeSize();
		_avatarFace.color = _faceColors[0];

		// Hair
		for (int i = 0; i < _hairColors.Length; i++)
		{
			GameObject color = Instantiate(_avatarSelectorPrefab, _hairColorContainer.transform);
			_selectorHairColor[i] = color.GetComponent<AvatarSelector>();

			_selectorHairColor[i].SetUp(this, i, AvatarSelector.SelectorCategory.HairColor, _colorImage, i == 0, _hairColors[i]);
		}

		for (int i = 0; i < _hairShapes.Length; i++)
		{
			GameObject hair = Instantiate(_avatarSelectorPrefab, _hairTypesContainer.transform);
			_hairs[i] = hair.GetComponent<AvatarSelector>();

			_hairs[i].SetUp(this, i, AvatarSelector.SelectorCategory.Hair, _hairShapes[i], i == 0, _hairColors[0]);
		}

		_avatarHair.sprite = _hairShapes[0];
		_avatarHair.SetNativeSize();
		_avatarHair.color = _hairColors[0];

		// Face
		for (int i = 0; i < _eyesColors.Length; i++)
		{
			GameObject color = Instantiate(_avatarSelectorPrefab, _faceColorEyesContainer.transform);
			_selectorEyesColor[i] = color.GetComponent<AvatarSelector>();

			_selectorEyesColor[i].SetUp(this, i, AvatarSelector.SelectorCategory.EyesColor, _colorImage, i == 0, _eyesColors[i]);
		}

		for (int i = 0; i < _eyesShapes.Length; i++)
		{
			GameObject eyes = Instantiate(_avatarSelectorPrefab, _faceEyesContainer.transform);
			_eyes[i] = eyes.GetComponent<AvatarSelector>();

			_eyes[i].SetUp(this, i, AvatarSelector.SelectorCategory.Eyes, _eyesShapes[i], i == 0, _eyesColors[0]);
		}

		for (int i = 0; i < _nosesShapes.Length; i++)
		{
			GameObject nose = Instantiate(_avatarSelectorPrefab, _faceNoseContainer.transform);
			_noses[i] = nose.GetComponent<AvatarSelector>();

			_noses[i].SetUp(this, i, AvatarSelector.SelectorCategory.Noses, _nosesShapes[i], i == 0);
		}

		_avatarEyes.sprite = _eyesShapes[0];
		_avatarEyes.SetNativeSize();
		_avatarEyes.color = _eyesColors[0];
		_avatarNose.sprite = _nosesShapes[0];
		_avatarNose.SetNativeSize();

		// Clothes

		for (int i = 0; i < _clothesColors.Length; i++)
		{
			GameObject color = Instantiate(_avatarSelectorPrefab, _clothesColourContainer.transform);
			_selectorClothesColor[i] = color.GetComponent<AvatarSelector>();

			_selectorClothesColor[i].SetUp(this, i, AvatarSelector.SelectorCategory.ClothesColor, _colorImage, i == 0, _clothesColors[i]);
		}

		for (int i = 0; i < _clothesShapes.Length; i++)
		{
			GameObject clothes = Instantiate(_avatarSelectorPrefab, _clothesContainer.transform);
			_clothes[i] = clothes.GetComponent<AvatarSelector>();

			_clothes[i].SetUp(this, i, AvatarSelector.SelectorCategory.Clothes, _clothesShapes[i], i == 0, _clothesColors[0]);
		}

		for (int i = 0; i < _accesoriesShapes.Length; i++)
		{
			GameObject accesory = Instantiate(_avatarSelectorPrefab, _clothesAccesoriesContainer.transform);
			_accesories[i] = accesory.GetComponent<AvatarSelector>();

			_accesories[i].SetUp(this, i, AvatarSelector.SelectorCategory.Accesories, _accesoriesShapes[i], i == 0);
		}

		_avatarClothes.sprite = _clothesShapes[0];
		_avatarClothes.SetNativeSize();
		_avatarClothes.color = _clothesColors[0];
		_avatarAcessory.sprite = _accesoriesShapes[0];
		_avatarAcessory.SetNativeSize();

		ResetToDefault();
	}

	public void ResetToDefault()
	{
		UpdateSelection(_defaultFaceIndex, AvatarSelector.SelectorCategory.Face);
		UpdateSelection(_defaultSkinColorIndex, AvatarSelector.SelectorCategory.FaceColor);
		UpdateSelection(_defaultHairIndex, AvatarSelector.SelectorCategory.Hair);
		UpdateSelection(_defaultHairColorIndex, AvatarSelector.SelectorCategory.HairColor);
		UpdateSelection(_defaultEyesIndex, AvatarSelector.SelectorCategory.Eyes);
		UpdateSelection(_defaultEyesColorIndex, AvatarSelector.SelectorCategory.EyesColor);
		UpdateSelection(_defaultNoseIndex, AvatarSelector.SelectorCategory.Noses);
		UpdateSelection(_defaultClothesIndex, AvatarSelector.SelectorCategory.Clothes);
		UpdateSelection(_defaultClothesColorIndex, AvatarSelector.SelectorCategory.ClothesColor);
		UpdateSelection(_defaultAccesoriesIndex, AvatarSelector.SelectorCategory.Accesories);
	}

	public void Randomize()
	{
		UpdateSelection(Random.Range(0, _faces.Length), AvatarSelector.SelectorCategory.Face);
		UpdateSelection(Random.Range(0, _faceColors.Length), AvatarSelector.SelectorCategory.FaceColor);
		UpdateSelection(Random.Range(0, _hairs.Length), AvatarSelector.SelectorCategory.Hair);
		UpdateSelection(Random.Range(0, _hairColors.Length), AvatarSelector.SelectorCategory.HairColor);
		UpdateSelection(Random.Range(0, _eyes.Length), AvatarSelector.SelectorCategory.Eyes);
		UpdateSelection(Random.Range(0, _eyesColors.Length), AvatarSelector.SelectorCategory.EyesColor);
		UpdateSelection(Random.Range(0, _noses.Length), AvatarSelector.SelectorCategory.Noses);
		UpdateSelection(Random.Range(0, _clothes.Length), AvatarSelector.SelectorCategory.Clothes);
		UpdateSelection(Random.Range(0, _clothesColors.Length), AvatarSelector.SelectorCategory.ClothesColor);
		UpdateSelection(Random.Range(0, _accesories.Length), AvatarSelector.SelectorCategory.Accesories);
	}

	public void UpdateSelection(int index, AvatarSelector.SelectorCategory category)
	{
		switch(category)
		{
			case AvatarSelector.SelectorCategory.Face:
				_faces[_selectedFace].TurnOff();
				_selectedFace = index;
				_avatarFace.sprite = _faceShapes[_selectedFace];
				_avatarFace.SetNativeSize();
				_faces[_selectedFace].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.FaceColor:
				_selectorFaceColor[_selectedFaceColor].TurnOff();
				_selectedFaceColor = index;
				for(int i = 0; i < _faces.Length; i++)
				{
					_faces[i].ChangeColor(_faceColors[_selectedFaceColor]);
				}
				_avatarFace.color = _faceColors[_selectedFaceColor];
				_selectorFaceColor[_selectedFaceColor].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.Hair:
				_hairs[_selectedHair].TurnOff();
				_selectedHair = index;
				_avatarHair.sprite = _hairShapes[_selectedHair];
				_avatarHair.SetNativeSize();
				_hairs[_selectedHair].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.HairColor:
				_selectorHairColor[_selectedHairColor].TurnOff();
				_selectedHairColor = index;
				for (int i = 0; i < _hairs.Length; i++)
				{
					_hairs[i].ChangeColor(_hairColors[_selectedHairColor]);
				}
				_avatarHair.color = _hairColors[_selectedHairColor];
				_selectorHairColor[_selectedHairColor].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.Eyes:
				_eyes[_selectedEyes].TurnOff();
				_selectedEyes = index;
				_avatarEyes.sprite = _eyesShapes[_selectedEyes];
				_avatarEyes.SetNativeSize();
				_eyes[_selectedEyes].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.EyesColor:
				_selectorEyesColor[_selectedEyesColor].TurnOff();
				_selectedEyesColor = index;
				for (int i = 0; i < _eyes.Length; i++)
				{
					_eyes[i].ChangeColor(_eyesColors[_selectedEyesColor]);
				}
				_avatarEyes.color = _eyesColors[_selectedEyesColor];
				_selectorEyesColor[_selectedEyesColor].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.Noses:
				_noses[_selectedNose].TurnOff();
				_selectedNose = index;
				_avatarNose.sprite = _nosesShapes[_selectedNose];
				_avatarNose.SetNativeSize();
				_noses[_selectedNose].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.Clothes:
				_clothes[_selectedClothes].TurnOff();
				_selectedClothes = index;
				_avatarClothes.sprite = _clothesShapes[_selectedClothes];
				_avatarClothes.SetNativeSize();
				_clothes[_selectedClothes].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.ClothesColor:
				_selectorClothesColor[_selectedClothesColor].TurnOff();
				_selectedClothesColor = index;
				for (int i = 0; i < _eyes.Length; i++)
				{
					_clothes[i].ChangeColor(_clothesColors[_selectedClothesColor]);
				}
				_avatarClothes.color = _clothesColors[_selectedClothesColor];
				_selectorClothesColor[_selectedClothesColor].TurnOn();
				break;
			case AvatarSelector.SelectorCategory.Accesories:
				_accesories[_selectedAccesory].TurnOff();
				_selectedAccesory = index;
				_avatarAcessory.sprite = _accesoriesShapes[_selectedAccesory];
				_avatarAcessory.SetNativeSize();
				_accesories[_selectedAccesory].TurnOn();
				break;
			default:
				Debug.LogError("Type of avatar customization invalid: Index " + index);
				break;
		}
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

	public void SubmitAvatar()
	{
		// TODO depends on how we construct or save the avatar during game, this is a temporary solution until the necessity comes forth
		// Copies the GameObject and returns it for it to be manipulated as necessary
		Instantiate(_avatar, _resultParent.transform);
	}
}
