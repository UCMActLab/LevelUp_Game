using System;
using System.Collections.Generic;
using UnityEngine;

namespace BG_Games.Chat_Builder___Mobile_Chat_Quests.Scripts.Localisation.SO
{
    [CreateAssetMenu(fileName = "newLanguagesList",menuName = "Localisation/LanguagesList")]
    public class LanguagesListSO : ScriptableObject
    {
        [SerializeField] private List<LanguageDataElementSO> languageDataElements;

        public List<LanguageDataElementSO> LanguageDataElements => languageDataElements;
    }

    [Serializable]
    public struct LanguageDataElementSO
    {
        public LanguageDataSO LanguageData;
        public bool IsActive;
    }
}