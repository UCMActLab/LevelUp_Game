using UnityEngine;

[CreateAssetMenu(fileName = "WordMiniGame", menuName = "ScriptableObjects/MiniGames/WordMiniGame")]
public class WordMiniGame : ScriptableObject
{
    public string targetWordTableKey;

    public string hintTableKey;

    public string optionsTableKey;
}
