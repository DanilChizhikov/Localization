#if UNITY_EDITOR
using UnityEditor;

namespace MbsCore.Localization.Runtime
{
    public sealed partial class LocalizationMap
    {
        public void UpdateGroups(LanguageGroup[] groups)
        {
            _groups = groups;
            EditorUtility.SetDirty(this);
        }
    }
}
#endif