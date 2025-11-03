using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "ScriptableObjects/Test/Test")]
public class Test : ScriptableObject
{
    public string testName;
    public Question[] questions;
}
