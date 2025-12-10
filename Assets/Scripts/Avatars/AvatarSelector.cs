using UnityEngine;
using UnityEngine.UI;

public class AvatarSelector : MonoBehaviour
{
    public enum SelectorCategory
    {
        Face,
        FaceColor,
        Hair,
        HairColor,
        Eyes,
        EyesColor,
        Noses,
        Clothes,
        ClothesColor,
        Accesories,
        _Length
    }

    private SelectorCategory _category;

    [SerializeField]
    private Color _deSelectedColor = new Color(0.7960784f, 0.8352941f, 0.8823529f);

    [SerializeField]
    private Color _selectedColor = new Color(0f, 0.2078432f, 0.2941177f);

    [SerializeField]
    private Outline _outline;

    [SerializeField]
    private Image _selectionImage;

    [SerializeField]
    private Sprite _selectionSprite;

    [SerializeField]
    private GameObject _badge;

    private bool _selected = false;

    private AvatarLogic _logic;

    private int _index;

    public void SetUp(AvatarLogic logic, int index, SelectorCategory category, Sprite image, bool selected = false, Color? color = null)
    {
        _logic = logic;
        _index = index;
        _category = category;
        _selectionSprite = image;
        _selectionImage.sprite = _selectionSprite;
        if(color != null)
        {
            _selectionImage.color = color.Value;
        }
        else
        {
            _selectionImage.color = Color.white;
        }
        _selected = selected;
		if (_outline != null)
		{
			_outline.effectColor = _selected ? _selectedColor : _deSelectedColor;
		}
		if (_badge != null)
		{
			_badge.gameObject.SetActive(_selected);
		}
	}

    public void ChangeColor(Color color)
    {
        _selectionImage.color = color;
    }

    public void TurnOff()
    {
        _selected = false;
        UpdateVisuals(false);
    }

    public void OnSelection()
    {
        if(_logic != null)
        {
            _logic.UpdateSelection(_index, _category);
        }

        _selected = !_selected;
        UpdateVisuals(_selected);
    }

    private void UpdateVisuals(bool active)
    {
		if (_outline != null)
		{
			_outline.effectColor = active ? _selectedColor : _deSelectedColor;
		}
		if (_badge != null)
		{
			_badge.gameObject.SetActive(active);
		}
	}
}
