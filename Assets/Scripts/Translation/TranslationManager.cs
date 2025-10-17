using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum LanguageTranslation
{
    english = 0,
    spanish = 1,
    czech = 2,
    bulgarian = 3
}

[ExecuteInEditMode]
public class TranslationManager : Singleton<TranslationManager>
{
    [SerializeField] private string _fileName = "translation.csv";

    Dictionary<string, List<string>> _translations;

    private LanguageTranslation _currentLanguage = LanguageTranslation.english;
    public LanguageTranslation SelectedLanguage { get { return _currentLanguage; } }

    private List<TranslateLabel> _translatedStrings;

    protected override void Awake()
    {
        base.Awake();

        _translatedStrings = new List<TranslateLabel>();
        InitializeData();
    }

    public void SelectLanguage(LanguageTranslation language)
    {
        _currentLanguage = language;
    }

    public string GetValueForKey(string key)
    {
        return _translations[key][(int)_currentLanguage];
    }

    private void InitializeData()
    {
        string[] lines = File.ReadAllLines(Application.persistentDataPath + "/" + _fileName);

        _translations = new Dictionary<string, List<string>>();
        for (int i = 1; i < lines.Length; ++i)
        {
            string[] values = lines[i].Split(';');

            string key = values[0];
            List<string> translationValues = values.ToList();
            translationValues.RemoveAt(0);

            _translations.Add(key, translationValues);
        }
    }

}
