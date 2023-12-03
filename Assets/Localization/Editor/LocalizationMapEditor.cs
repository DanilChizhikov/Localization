using System;
using System.Collections.Generic;
using System.Linq;
using MbsCore.Localization.Runtime;
using UnityEditor;

namespace MbsCore.Localization.Editor
{
    [CustomEditor(typeof(LocalizationMap), true)]
    public class LocalizationMapEditor : UnityEditor.Editor
    {
        private const string GroupsFieldName = "_groups";

        private readonly Dictionary<string, ISheetTab> _tabsMap = new();

        private SerializedProperty _groupsProperty;
        private int _selectedTabIndex = -1;

        public override void OnInspectorGUI()
        {
            if (_groupsProperty == null)
            {
                return;
            }

            serializedObject.Update();
            EditorGUILayout.PropertyField(_groupsProperty);
            EditorHelper.VerticalDraw(DrawTab, "box");
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTab()
        {
            string[] tabNames = _tabsMap.Keys.ToArray();
            _selectedTabIndex = EditorGUILayout.Popup("Tabs", _selectedTabIndex, tabNames);
            if (_selectedTabIndex > -1 &&
                _tabsMap.TryGetValue(tabNames[_selectedTabIndex], out ISheetTab tab))
            {
                tab.DrawGui(_groupsProperty);
            }
        }

        private void OnEnable()
        {
            _groupsProperty = serializedObject.FindProperty(GroupsFieldName);
            _selectedTabIndex = -1;
            Type[] types = typeof(ISheetTab).GetImplementations();
            for (int i = types.Length - 1; i >= 0; i--)
            {
                var tab = Activator.CreateInstance(types[i]) as ISheetTab;
                _tabsMap.TryAdd(tab.TabName, tab);
            }
        }

        private void OnDisable()
        {
            _groupsProperty = null;
            _tabsMap.Clear();
        }
    }
}