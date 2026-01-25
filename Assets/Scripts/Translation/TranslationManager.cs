using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class TranslationManager : Singleton<TranslationManager>
{
    LocalizedStringDatabase _database;

    protected override void Awake()
    {
        base.Awake();

        _database = LocalizationSettings.StringDatabase;
    }

    private void Start()
    {
        GetAllTableEntries("EVALUATION");
    }

    public void ChangeLanguage(int localeID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
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
        for (int i = startIndex; i < count; i++)
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
