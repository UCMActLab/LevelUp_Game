using AYellowpaper.SerializedCollections;
using System;
using UnityEditor;
using Unity.Services.Analytics;
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

public struct LevelScore
{
    // type of articles
    public int totalArticles;
    public int trueArticles;
    public int falseArticles;

    // how many can the user actually read
    public int readableArticles;

    // how many has the user read
    public int readArticles;

    // how many has the user shared
    public int trueArticlesShared;
    public int falseArticlesShared;

    public int questionsRight;
    public int totalQuestions;

    public int maxScore;

    // public int MaxScore { get { return readableArticles + trueArticles + totalQuestions / 2; } }

}

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Score")]
    private int _initialScore = 0;

    [SerializeField] private int _pointsForIdentifyingTrueArticle = 1;
    [SerializeField] private int _pointsForReadingArticle = 1;
    [SerializeField] private int _pointsForQuestionAnsweredCorrectly = 1;
    [SerializeField] private int _pointsForSharingFalseArticle = -1;

    [Header("References")]
    private ScoreMenu _scoreMenu = null;

    [SerializeField]
    SerializedDictionary<global::Score, ScoreInfo> _pointsForEachCategory = null;

    private int _currentScore = 0;
    private Score _currentState = global::Score.NONE;

    public Score State { get { return _currentState; } }

    LevelScore _score;
    public LevelScore ScoreStats { get { return _score; } }

    public bool CanContinue = true;

    //LevelScore[] _levelsScore = null;
    //int _levelScoreIndex = 0;

    public int Score { get { return _currentScore; } }
    public int MaxScore { get { return _score.maxScore; } }

    protected override void Awake()
    {
        base.Awake();

        // _levelsScore = null;
        // _levelScoreIndex = 0;
    }

    private void Start()
    {
        _currentScore = _initialScore;
        _currentState = global::Score.NONE;
    }
    
    private void FindPointsMenu()
    {
        _scoreMenu = GameObject.FindAnyObjectByType<ScoreMenu>(FindObjectsInactive.Include);
    }

    public void CalculateScoreState()
    {
        int nextState = (int)_currentState + 1;
        // reached max score
        if (nextState >= _pointsForEachCategory.Count) return;

        if(_currentScore >= MaxScore * _pointsForEachCategory[(global::Score)nextState].NeededScore)
        {
            _currentState = (global::Score)nextState;

            // change something (?)
            _scoreMenu.ChangeMedal(_pointsForEachCategory[_currentState].Sprite);
        }
    }

    // this is 0 points actually
    private void AwardPointsReadArticle()
    {
        AddPoints(_pointsForReadingArticle);
        // _levelsScore[_levelScoreIndex].readArticles++;
        _score.readArticles++;
    }

    private void AwardPointsForSharingTrueArticle()
    {
        AddPoints(_pointsForIdentifyingTrueArticle);
        // _levelsScore[_levelScoreIndex].trueArticlesShared++;
        _score.trueArticlesShared++;
    }

    private void SubstractPointsForSharingFalseArticle()
    {
        AddPoints(_pointsForSharingFalseArticle);
        _score.falseArticlesShared++;
    }

    public void EvaluateQuest(Quest quest)
    {
        for (int i = 0; i < quest.done.identifiedArticles; ++i)
        {
            AwardPointsForSharingTrueArticle();
        }

        for (int i = 0; i < quest.done.falseArticlesShared; ++i)
        {
            SubstractPointsForSharingFalseArticle();
        }

        for (int i = 0; i < quest.done.readedArticles; ++i)
        {
            AwardPointsReadArticle();
        }

        Debug.Log("Evaluating Quest");

        _score.totalArticles += quest.totalArticles;
        _score.trueArticles += quest.toDo.articlesToIdentify;
        _score.falseArticles += quest.toDo.falseArticlesToSkip;
        _score.readableArticles += quest.toDo.toRead;

        // for (int i = 0; i < quest.)
    }

    public void AnsweredQuestionRight()
    {
        // TODO: CAMBIAR ESTO??
        // _levelsScore[_levelScoreIndex].questionsRight++;
        // AddPoints(_pointsForQuestionAnsweredCorrectly);
    }

    public void SetMaxScore(int maxScore)
    {
        _score.maxScore = maxScore;

        SetMaxScoreToUIElements();
        
        CalculateScoreState();
    }

    private void SetMaxScoreToUIElements()
    {
        FindPointsMenu();
        _scoreMenu.SetTotalScore(MaxScore);
    }

    public void SetNumLevels(int numLevels)
    {
        RestartScore();

        // _levelsScore = new LevelScore[numLevels];
    }

    //public void SetLevelInfo(int levelIndex, int numArticles, int numArticlesToRead, int numArticlesTrue, int numQuestions)
    //{
    //    //LevelScore current = _levelsScore[levelIndex];

    //    //current.totalArticles = numArticles;
    //    //_score.totalArticles += current.totalArticles;

    //    //current.trueArticles = numArticlesTrue;
    //    //_score.trueArticles += current.trueArticles;

    //    //current.falseArticles = current.totalArticles - current.trueArticles;
    //    //_score.falseArticles += current.falseArticles;

    //    //current.readableArticles = numArticlesToRead;
    //    //_score.readableArticles += current.readableArticles;

    //    //current.readArticles = 0;
    //    //current.trueArticlesShared = 0;
    //    //current.falseArticlesShared = 0;

    //    //current.totalQuestions = numQuestions;
    //    //_score.totalQuestions += numQuestions;
    //    //current.questionsRight = 0;

    //    //_levelsScore[levelIndex] = current;
    //}

    //public void ReachedNewLevel(int level)
    //{
    //    _levelScoreIndex = level;
    //}

    private void AddPoints(int points)
    {
        _currentScore = Mathf.Min(MaxScore, _currentScore + points);

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

    public void ShowPoints(Quest quest)
    {
        // poner los puntos y eso
        _scoreMenu.gameObject.SetActive(true);

        // scoreMenu.ShowScore(_levelsScore[_levelScoreIndex]);
        _scoreMenu.ShowScore(quest);

        SubmitScoreEvent();
    }

    public void RestartScore()
    {
        _currentScore = _initialScore;

        _score = new LevelScore();

        _score.totalArticles = 0;
        _score.trueArticles = 0;
        _score.trueArticlesShared = 0;
        _score.falseArticles = 0;
        _score.falseArticlesShared = 0;
        _score.readableArticles = 0;
        _score.readArticles = 0;

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