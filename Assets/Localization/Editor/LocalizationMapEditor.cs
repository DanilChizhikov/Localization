using MbsCore.Localization.Runtime;
using UnityEditor;

namespace MbsCore.Localization.Editor
{
    [CustomEditor(typeof(LocalizationMap), true)]
    public class LocalizationMapEditor : UnityEditor.Editor
    {
        private const string GroupsFieldName = "_groups";

        private SerializedProperty _groupsProperty;

        public override void OnInspectorGUI()
        {
            if (_groupsProperty == null)
            {
                return;
            }

            serializedObject.Update();
            EditorGUILayout.PropertyField(_groupsProperty);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _groupsProperty = serializedObject.FindProperty(GroupsFieldName);
        }

        private void OnDisable()
        {
            _groupsProperty = null;
        }
    }
}