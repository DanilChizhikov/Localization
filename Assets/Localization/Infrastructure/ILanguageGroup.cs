using System.Collections.Generic;
using UnityEngine;

namespace MbsCore.Localization.Infrastructure
{
    public interface ILanguageGroup
    {
        SystemLanguage Language { get; }
        IReadOnlyCollection<ITermInfo> TermInfos { get; }
    }
}