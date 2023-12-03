#if UNITY_EDITOR
using System.Collections.Generic;
using MbsCore.Localization.Infrastructure;

namespace MbsCore.Localization.Runtime
{
    public sealed partial class LocalizationMap
    {
        public IReadOnlyCollection<ILanguageGroup> Groups => _groups;
    }
}
#endif