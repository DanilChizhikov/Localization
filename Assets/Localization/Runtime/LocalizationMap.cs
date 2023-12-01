using System;
using System.Collections.Generic;
using MbsCore.Localization.Infrastructure;
using UnityEngine;

namespace MbsCore.Localization.Runtime
{
    [CreateAssetMenu(menuName = "Localization/Map", fileName = "New" + nameof(LocalizationMap), order = 51)]
    public sealed class LocalizationMap : ScriptableObject, ILocalizationMap
    {
        private const string InvalidTerm = "EMPTY";
        
        private readonly Dictionary<SystemLanguage, ILanguageGroup> _groupMap = new();
        
        [SerializeField] private LanguageGroup[] _groups = Array.Empty<LanguageGroup>();

        private IReadOnlyDictionary<SystemLanguage, ILanguageGroup> GroupMap
        {
            get
            {
                if (Application.isEditor)
                {
                    _groupMap.Clear();
                }

                if (_groupMap.Count <= 0)
                {
                    for (int i = _groups.Length - 1; i >= 0; i--)
                    {
                        LanguageGroup group = _groups[i];
                        _groupMap.Add(group.Language, group);
                    }
                }

                return _groupMap;
            }
        }

        public bool HasLanguage(SystemLanguage language) => GroupMap.ContainsKey(language);

        public string GetTerm(string key, SystemLanguage language)
        {
            if (GroupMap.TryGetValue(language, out ILanguageGroup group))
            {
                foreach (var termInfo in group.TermInfos)
                {
                    if (termInfo.Key == key)
                    {
                        return termInfo.Term;
                    }
                }
            }

            return InvalidTerm;
        }
    }
}