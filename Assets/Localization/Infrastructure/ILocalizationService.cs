using System;
using UnityEngine;

namespace MbsCore.Localization.Infrastructure
{
    public interface ILocalizationService
    {
        event Action<SystemLanguage> OnLanguageChanged;

        SystemLanguage Language { get; }
        
        bool TrySelectLanguage(SystemLanguage value);
        string GetTerm(string key);
        string GetTermFormat(string key, params object[] args);
    }
}
