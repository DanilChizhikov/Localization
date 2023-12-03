using UnityEditor;

namespace MbsCore.Localization.Editor
{
    public interface ISheetTab
    {
        string TabName { get; }

        void DrawGui(SerializedProperty groupProperty);
    }
}