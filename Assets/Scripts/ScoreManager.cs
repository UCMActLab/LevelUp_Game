using AYellowpaper.SerializedCollections;
using System;
using UnityEditor;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum Score
{
    GOLD = 3,
    SILVER = 2,
    BRONZE = 1,
    NONE = 0
}

[Serializable]
public struct ScoreInfo
{
    public Sprite Sprite;
    public float NeededScore;
}

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Score")]
    private int _maxScore;
    private int _initialScore = 0;

    [SerializeField] private int _pointsForIdentifyingTrueArticle = 1;
    [SerializeField] private int _pointsForReadingArticle = 1;

    [Header("References")]
    private ScoreMenu _scoreMenu = null;
    private Slider _generalScoreSlider = null;

    [SerializeField]
    SerializedDictionary<global::Score, ScoreInfo> _pointsForEachCategory = null;

    private int _currentScore = 0;
    private Score _currentState = global::Score.NONE;

    public Score State { get { return _currentState; } }

    private int _numArticlesForCurrentLevel = 0;
    private int _numArticlesCanReadForCurrentLevel = 0;
    private int _numArticlesReadForCurrentLevel = 0;
    private int _numTrueArticlesSharedForCurrentLevel = 0;
    private int _numTrueArticles = 0;
    private int _numFalseArticlesSharedForCurrentLevel = 0;

    private int _numArticlesReadTotal = 0;
    private int _numFalseSharedTotal = 0;
    private int _numTrueSharedTotal = 0;
    private int _totalArticles = 0;

    public int TotalArticles { get { return _totalArticles; } }

    public int TotalRead { get { return _numArticlesReadTotal; } }
    public int TotalTrueShared { get { return _numTrueSharedTotal; } }
    public int TotalFalseShared { get { return _numFalseSharedTotal; } }

    public int Score { get { return _currentScore; } }
    public int MaxScore { get { return _maxScore; } }

    private void Start()
    {
        _currentScore = _initialScore;
        _currentState = global::Score.NONE;
    }
    
    private void FindPointsMenu()
    {
        _scoreMenu = GameObject.FindAnyObjectByType<ScoreMenu>(FindObjectsInactive.Include);
    }

    public void SetGeneralScoreSlider(Slider general)
    {
        _generalScoreSlider = general;
    }

    public void CalculateScoreState()
    {
        int nextState = (int)_currentState + 1;
        // reached max score
        if (nextState >= _pointsForEachCategory.Count) return;

        if(_currentScore >= _maxScore * _pointsForEachCategory[(global::Score)nextState].NeededScore)
        {
            _currentState = (global::Score)nextState;

            // change something (?)
            _scoreMenu.ChangeMedal(_pointsForEachCategory[_currentState].Sprite);
        }
    }

    private void AwardPointsReadArticle()
    {
        AddPoints(_pointsForReadingArticle);
        _numArticlesReadForCurrentLevel++;
    }

    private void AwardPointsIdentifyingArticle(bool wasTrue)
    {
        AddPoints(_pointsForIdentifyingTrueArticle);
        if(wasTrue) _numTrueArticlesSharedForCurrentLevel++;
    }

    public void CalculateArticlePoints(ArticleGameObject data)
    {
        if(data.HasReadArticle)
        {
            _numArticlesReadTotal++;
            AwardPointsReadArticle();
        }

        if(data.IsTrue)
        {
            _numTrueArticles++;
            if(data.HasSharedArticle)
            {
                _numTrueSharedTotal++;
                AwardPointsIdentifyingArticle(true);
            }
        }
        else if (!data.HasSharedArticle)
        {
            // AwardPointsIdentifyingArticle(false);
        }
        else
        {
            _numFalseArticlesSharedForCurrentLevel++;
            _numFalseSharedTotal++;
        }
    }

    public void SetMaxScore(int numArticles, int numTrueArticles)
    {
        // Cada artículo tiene 2 variables que cuentan puntitos
        RestartScore();
        _totalArticles = numArticles;
        _maxScore = numArticles + numTrueArticles;
        
        SetMaxScoreToUIElements();
        
        CalculateScoreState();
    }

    private void SetMaxScoreToUIElements()
    {
        FindPointsMenu();
        _scoreMenu.SetTotalScore(_maxScore);
        _generalScoreSlider.maxValue = _maxScore;
        _generalScoreSlider.value = _currentScore;
    }


    public void SetLevelInfo(int numArticles, int numArticlesToRead)
    {
        _numArticlesForCurrentLevel = numArticles;
        _numArticlesCanReadForCurrentLevel = numArticlesToRead;

        _numArticlesReadForCurrentLevel = 0;
        _numTrueArticlesSharedForCurrentLevel = 0;
        _numFalseArticlesSharedForCurrentLevel = 0;
        _numTrueArticles = 0;
    }

    private void AddPoints(int points)
    {
        _currentScore += points;

        SubmitScoreEvent();
    }

    private void SubmitScoreEvent()
    {
        CustomEvent newEvent = new CustomEvent("Score_Check")
        {
            {"Score", _currentScore },
            {"MaxScore", MaxScore }
        };
        AnalyticsManager.Instance.SubmitEvent(newEvent);
    }

    public void ShowPoints()
    {
        // poner los puntos y eso
        _scoreMenu.gameObject.SetActive(true);
        
        _scoreMenu.ShowScore(_numArticlesForCurrentLevel, _numArticlesCanReadForCurrentLevel, _numArticlesReadForCurrentLevel, _numTrueArticlesSharedForCurrentLevel, _numTrueArticles, _numFalseArticlesSharedForCurrentLevel, _numArticlesForCurrentLevel - _numTrueArticles);

        SubmitScoreEvent();
    }

    public void RestartScore()
    {
        _currentScore = _initialScore;

        _totalArticles = 0;
        _numArticlesReadTotal = 0;
        _numFalseSharedTotal = 0;
        _numTrueSharedTotal = 0;

        _currentState = global::Score.NONE;
    }

    public void DeactivateMenu()
    {
        _scoreMenu.gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        _scoreMenu.gameObject.SetActive(!_scoreMenu.gameObject.activeSelf);
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
        // If you want the button to work in edit mode too, remove the BeginDisabledGroup/EndDisabledGroup
    }
}
#endif
