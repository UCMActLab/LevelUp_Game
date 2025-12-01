using UnityEngine;

[CreateAssetMenu(fileName = "WordMiniGame", menuName = "ScriptableObjects/MiniGames/WordMiniGame")]
public class WordMiniGame : ScriptableObject
{
    public string targetWord;

    public string hint;

    public string[] options;
}
