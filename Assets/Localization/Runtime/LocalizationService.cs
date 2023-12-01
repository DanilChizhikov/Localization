using System;
using System.Collections.Generic;
using MbsCore.Localization.Infrastructure;
using UnityEngine;

namespace MbsCore.Localization.Runtime
{
    internal sealed class LocalizationService : ILocalizationService
    {
        private const string EmptyTerm = "EMPTY";
        
        public event Action<SystemLanguage> OnLanguageChanged;

        private readonly HashSet<ILocalizationMap> _localizationMaps;

        public SystemLanguage Language { get; private set; }

        public LocalizationService(IEnumerable<ILocalizationMap> localizationMaps)
        {
            _localizationMaps = new HashSet<ILocalizationMap>(localizationMaps);
        }

        public bool TrySelectLanguage(SystemLanguage value)
        {
            if (value == Language || !ValidateLanguage(value))
            {
                return false;
            }

            Language = value;
            OnLanguageChanged?.Invoke(Language);
            return true;
        }

        public string GetTerm(string key)
        {
            foreach (var map in _localizationMaps)
            {
                if (map.TryGetTerm(key, Language, out string term))
                {
                    return term;
                }
            }

            return EmptyTerm;
        }

        public string GetTermFormat(string key, params object[] args)
        {
            string term = GetTerm(key);
            return string.Format(term, args);
        }

        private bool ValidateLanguage(SystemLanguage language)
        {
            foreach (var map in _localizationMaps)
            {
                if (map.HasLanguage(language))
                {
                    return true;
                }
            }

            return false;
        }
    }
}