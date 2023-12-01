using System;
using System.Collections.Generic;
using MbsCore.Localization.Infrastructure;
using UnityEngine;

namespace MbsCore.Localization.Runtime
{
    [Serializable]
    public sealed class LanguageGroup : ILanguageGroup
    {
        [SerializeField] private SystemLanguage _language = SystemLanguage.English;
        [SerializeField] private TermInfo[] _infos = Array.Empty<TermInfo>();

        public SystemLanguage Language => _language;
        public IReadOnlyCollection<ITermInfo> TermInfos => _infos;

        public LanguageGroup(SystemLanguage language, TermInfo[] infos)
        {
            _language = language;
            _infos = infos;
        }
    }
}