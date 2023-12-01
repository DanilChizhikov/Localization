using UnityEngine;

namespace MbsCore.Localization.Infrastructure
{
    public interface ILocalizationMap
    {
        bool HasLanguage(SystemLanguage language);
        bool TryGetTerm(string key, SystemLanguage language, out string term);
    }
}