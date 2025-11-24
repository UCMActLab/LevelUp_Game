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
        _feedbackGO.SetFalseShared(ScoreManager.Instance.TotalFalseShared);
        _feedbackGO.SetTrueShared(ScoreManager.Instance.TotalTrueShared);
        _feedbackGO.SetArticlesRead(ScoreManager.Instance.TotalRead, ScoreManager.Instance.TotalArticles);
    }
}
