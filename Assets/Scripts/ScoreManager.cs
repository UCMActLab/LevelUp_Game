using AYellowpaper.SerializedCollections;
using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
    [SerializeField] private PointsMenu _pointsMenu = null;

    [SerializeField]
    SerializedDictionary<global::Score, ScoreInfo> _pointsForEachCategory = null;

    private int _currentScore = 0;
    private Score _currentState = global::Score.NONE;

    private int _numArticlesForCurrentLevel = 0;
    private int _numArticlesReadForCurrentLevel = 0;
    private int _numTrueArticlesSharedForCurrentLevel = 0;
    private int _numTrueArticles = 0;
    private int _numFalseArticlesSharedForCurrentLevel = 0;

    public int Score { get { return _currentScore; } }
    public int MaxScore { get { return _initialScore; } }

    private void Start()
    {
        _currentScore = _initialScore;
        _currentState = global::Score.NONE;
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
            _pointsMenu.ChangeMedal(_pointsForEachCategory[_currentState].Sprite);
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
            AwardPointsReadArticle();
        }

        if(data.IsTrue)
        {
            _numTrueArticles++;
            if(data.HasSharedArticle)
            {
                AwardPointsIdentifyingArticle(true);
            }
        }
        else if (!data.HasSharedArticle)
        {
            AwardPointsIdentifyingArticle(false);
        }
        else
        {
            _numFalseArticlesSharedForCurrentLevel++;
        }
    }

    public void SetMaxScore(int numArticles)
    {
        // Cada artículo tiene 2 variables que cuentan puntitos
        _maxScore = numArticles * 2;
        _pointsMenu.SetTotalScore(_maxScore);
        CalculateScoreState();
    }

    public void SetLevelInfo(int numArticles)
    {
        _numArticlesForCurrentLevel = numArticles;

        _numArticlesReadForCurrentLevel = 0;
        _numTrueArticlesSharedForCurrentLevel = 0;
        _numTrueArticles = 0;
    }

    private void AddPoints(int points)
    {
        _currentScore += points;
    }

    public void ShowPoints()
    {
        // poner los puntos y eso
        _pointsMenu.gameObject.SetActive(true);
        
        _pointsMenu.ShowScore(_numArticlesForCurrentLevel, _numArticlesReadForCurrentLevel, _numTrueArticlesSharedForCurrentLevel, _numTrueArticles, _numFalseArticlesSharedForCurrentLevel, _numArticlesForCurrentLevel - _numTrueArticles);
    }

    public void RestartScore()
    {
        _currentScore = _initialScore;
    }

    public void DeactivateMenu()
    {
        _pointsMenu.gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        _pointsMenu.gameObject.SetActive(!_pointsMenu.gameObject.activeSelf);
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
