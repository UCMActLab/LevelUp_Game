using AYellowpaper.SerializedCollections;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

public enum Score
{
    PERFECT,
    HIGH,
    MEDIUM,
    LOW,
    ZERO
}

[Serializable]
public struct ScoreMessages
{
    public LocalizedString Title;
    public string Explanation;
    public Sprite Avatar;
}

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("How many points does any of this substract from the player's score")]
    [SerializeField] private int _sharedFalseArticle = 0;
    [SerializeField] private int _sharedUnreadArticle = 0;
    // [SerializeField] private int _sharedArticleAfterBadComments = 0;

    [Header("Score States")]
    [SerializeField, Range(0.0f,1.0f)] float _howMuchForHIGH;
    [SerializeField, Range(0.0f,1.0f)] float _howMuchForMEDIUM;

    [Header("Messages to Show")]
    [SerializeField] private SerializedDictionary<Score, ScoreMessages> _messagesOnScore;

    [Header("Other")]
    [SerializeField] private int _initialScore = 100;

    [Header("References")]
    [SerializeField] private GameObject _pointsMenu = null;

    private int _currentScore = 0;
    private Score _currentState = global::Score.PERFECT;

    private string _whatHapppenedMessage = string.Empty;

    public int Score { get { return _currentScore; } }
    public int MaxScore { get { return _initialScore; } }

    private void Start()
    {
        _currentScore = _initialScore;
        SetScoreState();
    }

    private void SubstractScore(int howMuch)
    {
        _currentScore = Mathf.Max(_currentScore - howMuch, 0);

        SetScoreState();
    }

    private void SetScoreState()
    {
        if(_currentScore == MaxScore)
        {
            _currentState = global::Score.PERFECT;
        }
        else if (_currentScore >= _howMuchForHIGH * MaxScore)
        {
            _currentState = global::Score.HIGH;
        }
        else if (_currentScore >= _howMuchForMEDIUM * MaxScore)
        {
            _currentState = global::Score.MEDIUM;
        }
        else if (_currentScore > 0)
        {
            _currentState = global::Score.LOW;
        }
        else
        {
            _currentState = global::Score.ZERO;
        }
    }

    #region Substract Score Actions
    public void SharedFalseArticle(bool hasReadArticle) 
    { 
        SubstractScore(_sharedFalseArticle);

        // feedback
        AddToFeedback("SCORE/SHAREDFAKENEW", true);
        if(hasReadArticle)
        {
            AddToFeedback("SCORE/SHAREDFAKENEW/READ", false);
        }
        else
        {
            AddToFeedback("SCORE/SHAREDFAKENEW/UNREAD", false);
        }
    }

    // public void SharedArticleAfterBadComments() { SubstractScore(_sharedArticleAfterBadComments); }
    public void SharedUnreadArticle(bool isArticleTrue) 
    { 
        SubstractScore(_sharedUnreadArticle);

        if(isArticleTrue)
        {
            AddToFeedback("SCORE/SHAREDTRUENEW", true);
            AddToFeedback("SCORE/SHAREDTRUENEW/UNREAD");
        }
    }
    #endregion

    private void AddToFeedback(string feedback, bool restart = false)
    {
        if (restart) RestartFeedback();

        _whatHapppenedMessage += TranslationManager.Instance.GetLocalizedStringValue("Translation", feedback) + '\n';
    }

    public void RestartFeedback()
    {
        _whatHapppenedMessage = string.Empty;
    }

    public void ShowPoints()
    {
        // poner los puntos y eso

        _pointsMenu.SetActive(true);
    }

    public void DeactivateMenu()
    {
        _pointsMenu.SetActive(false);
    }

    public void ToggleMenu()
    {
        _pointsMenu.SetActive(!_pointsMenu.activeSelf);
    }

    public ScoreMessages GetMenuText()
    {
        return _messagesOnScore[_currentState];
    }

    public string GetWhatHappened()
    {
        return _whatHapppenedMessage;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(ScoreManager))]
public class ScoreManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScoreManager my = (ScoreManager)target;

        // Optional: disable the button when not in Play Mode
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if (GUILayout.Button("Activate Score Menu"))
        {
            my.ToggleMenu();
        }

        if (GUILayout.Button("Shared False Article"))
        {
            my.SharedFalseArticle(false);
        }

        if (GUILayout.Button("Shared Unread Article"))
        {
            my.SharedUnreadArticle(true);
        }
        EditorGUI.EndDisabledGroup();

        // If you want the button to work in edit mode too, remove the BeginDisabledGroup/EndDisabledGroup
    }
}
#endif
