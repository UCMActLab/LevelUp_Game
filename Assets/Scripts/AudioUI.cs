using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _playIcon;

    [SerializeField]
    private Sprite _pauseIcon;

    // Ajustamos el slider segun la longitud del audio
    void Start()
    {
        _slider.minValue = 0;
        _slider.maxValue = _audioSource.clip.length;
    }

    // Metodo que se llama cuando se pulsa el boton de play/pausa y detiene o reanuda la reproduccion del audio
    public void OnClick()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }

    // Metodo que se llama cuando el user mueve el slider y ajusta el momento de reproduccion del audio correspondientemente
    public void OnValueChanged()
    {
        if(_slider.value < _audioSource.clip.length)
        {
            _audioSource.time = _slider.value;
        }
    }

    // Comprobamos que el slider avance con el audio y en caso de acabar o ponerlo en pausa ajustamos la longitud y el sprite
    void Update()
    {
        if (_audioSource.isPlaying)
        {
            _image.sprite = _pauseIcon;
            _slider.value = _audioSource.time;
        }
        else if (_audioSource.time == _audioSource.clip.length)
        {
            _slider.value = 0;
            _image.sprite = _playIcon;
        }
        else if (!_audioSource.isPlaying)
        {
            _image.sprite = _playIcon;
        }
    }
}
