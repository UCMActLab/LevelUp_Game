using UnityEngine;

[CreateAssetMenu(fileName = "LikertQuestion", menuName = "ScriptableObjects/Test/LikertQuestion")]
public class LikertQuestion : ScriptableObject
{
    private QuestionType questionType = QuestionType.LIKERT;

    public string questionText;

    public string[] rangeLabels = new string[2] { "Strongly Disagree", "Strongly Agree" };

    public int[] rangeVal = new int[2] { 1, 5 };

    public int defaultValue = 3;
}
