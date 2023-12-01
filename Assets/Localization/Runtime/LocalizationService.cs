using System;
using MbsCore.Localization.Infrastructure;
using UnityEngine;

namespace MbsCore.Localization.Runtime
{
    internal sealed class LocalizationService : ILocalizationService
    {
        public event Action<SystemLanguage> OnLanguageChanged;

        private readonly ILocalizationMap _localizationMap;

        public SystemLanguage Language { get; private set; }

        public bool TrySelectLanguage(SystemLanguage value)
        {
            if (value == Language || !_localizationMap.HasLanguage(value))
            {
                return false;
            }

            Language = value;
            OnLanguageChanged?.Invoke(Language);
            return true;
        }

        public string GetTerm(string key) => _localizationMap.GetTerm(key, Language);

        public string GetTermFormat(string key, params object[] args)
        {
            string term = GetTerm(key);
            return string.Format(term, args);
        }
    }
}