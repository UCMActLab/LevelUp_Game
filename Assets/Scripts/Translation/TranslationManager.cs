using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class TranslationManager : Singleton<TranslationManager>
{
    LocalizedStringDatabase _database;

    int _currentLocaleID = 0;

    protected override void Awake()
    {
        base.Awake();

        _database = LocalizationSettings.StringDatabase;
    }

    public string GetCurrentCountryLabel()
    {
        string label = "";

        switch (_currentLocaleID)
        {
            case (int)Language.bulgarian:
                label = "BG";
                break;
            case (int)Language.czech:
                label = "CZ";
                break;
            case (int)Language.spanish:
                label = "ES";
                break;
            case (int)Language.english:
                label = "EN";
                break;
            default:
                label = "UNK";
                break;
        }

        return label;
    }

    public void ChangeLanguage(int localeID)
    {
        _currentLocaleID = localeID;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentLocaleID];

        StartCoroutine(ServerManager.Instance.PostUserToDatabase(GetCurrentCountryLabel()));
    }

    public string GetRandomEntryKey(string table)
    {
        StringTable nameTable = _database.GetTable(table);

        long entry = nameTable.SharedData.Entries[Random.Range(0, nameTable.Values.Count)].Id;

        return nameTable.GetEntry(entry).Key;
    }

    public List<string> GetLocalizedStringsList(string table, string baseKey, int count, int startIndex = 0)
    {
        List<string> strings = new List<string>();
        StringTable tableReference = _database.GetTable(table);
        for (int i = startIndex; i < count + startIndex; i++)
        {
            strings.Add(tableReference.GetEntry(baseKey + i.ToString()).LocalizedValue);
        }

        return strings;
    }

    public string GetLocalizedStringValue(string table, string key)
    {
        StringTable tableReference = _database.GetTable(table);
        return tableReference.GetEntry(key).LocalizedValue;
    }

    public List<StringTableEntry> GetAllTableEntries(string table)
    {
        StringTable tableReference = _database.GetTable(table);

        return tableReference.Values.ToList();
    }
}
