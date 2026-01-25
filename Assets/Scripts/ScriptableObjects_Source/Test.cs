using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "ScriptableObjects/Test/Test")]
public class Test : ScriptableObject
{
    public string testName;
    public int numMultipleChoiceQuestions;
    public int numTrueFalseQuestions;

    public int TotalQuestions
    {
        get { return numTrueFalseQuestions + numMultipleChoiceQuestions; }
    }

    [HideInInspector]
    public Question[] questions;
}
