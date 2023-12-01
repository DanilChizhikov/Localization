using UnityEngine;

namespace MbsCore.Localization.Infrastructure
{
    public interface ILocalizationMap
    {
        bool HasLanguage(SystemLanguage language);
        string GetTerm(string key, SystemLanguage language);
    }
}