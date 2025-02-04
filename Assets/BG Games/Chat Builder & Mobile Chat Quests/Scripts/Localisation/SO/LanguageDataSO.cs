using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO
{
    [CreateAssetMenu(fileName = "newLanguageData",menuName = "Localisation/LanguageData")]
    public class LanguageDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name;
        [field: SerializeField] public string Suffix;
        [field: SerializeField] public LanguageType Language;
    
    }
}
