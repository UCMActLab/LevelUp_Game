using UnityEngine;
using UnityEngine.Localization;
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

    public string GetLocalizedStringValue(string table, string key)
    {
        StringTable tableReference = _database.GetTable(table);
        return tableReference.GetEntry(key).LocalizedValue;
    }
}
