using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsMenu : MonoBehaviour
{
    [SerializeField] private RotateAnimation _rotateAnim = null;
    [SerializeField] private TextMeshProUGUI _scoreText = null;
    [SerializeField] private TextMeshProUGUI _titleText = null;
    [SerializeField] private TextMeshProUGUI _explanationText = null;
    [SerializeField] private Image _avatar = null;

    private Animator _animator;
    private ScoreManager _scoreManager;

    private void OnEnable()
    {
        _animator.SetTrigger("PopUp");
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
        SetText();
    }

    private void OnDisable()
    {
        _scoreText.text = _scoreManager.MaxScore.ToString();
    }

    public void ShowScore()
    {
        float score = _scoreManager.Score / (float)_scoreManager.MaxScore;
        Debug.Log(score);
        _rotateAnim.StartRotation(score, ShowScoreText);
    }

    private void ShowScoreText(float progress)
    {
        _scoreText.text = 
            (_scoreManager.Score + 
            (int)((_scoreManager.MaxScore - _scoreManager.Score) 
            * (1 - progress))).ToString();

        if(progress >= 1.0f)
        {
            ShowText();
        }
    }

    private void ShowText()
    {
        SetText();
        _animator.SetTrigger("ShowInfo");
    }

    private void SetText()
    {
        ScoreMessages messages = ScoreManager.Instance.GetMenuText();

        _titleText.text = messages.Title;
        // _explanationText.text = messages.Explanation;
        _avatar.sprite = messages.Avatar;

        _explanationText.text = ScoreManager.Instance.GetWhatHappened();
    }
}
