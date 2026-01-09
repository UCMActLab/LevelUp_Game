using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[Serializable]
struct FeedbackInfo
{
    public Sprite sprite;
    public string feedback;
}

public class GiveEndFeedback : MonoBehaviour
{
    [SerializeField]
    SerializedDictionary<global::Score, FeedbackInfo> _feedback = null;

    [SerializeField] EndFeedback _feedbackGO = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FeedbackInfo info = _feedback[ScoreManager.Instance.State];
        _feedbackGO.SetTitle(info.feedback);
        _feedbackGO.SetImage(info.sprite);

        LevelScore score = ScoreManager.Instance.ScoreStats;
        _feedbackGO.SetFalseShared(score.falseArticlesShared);
        _feedbackGO.SetTrueShared(score.trueArticlesShared);
        _feedbackGO.SetArticlesRead(score.readArticles, score.readableArticles);
    }
}
