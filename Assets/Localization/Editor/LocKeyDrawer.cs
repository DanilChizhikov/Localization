using System.Collections.Generic;
using MbsCore.Localization.Infrastructure;
using MbsCore.Localization.Runtime;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MbsCore.Localization.Editor
{
    [CustomPropertyDrawer(typeof(LocKeyAttribute))]
    internal sealed class LocKeyDrawer : PropertyDrawer
    {
        private struct KeyPopupCache
        {
            public AdvancedKeyPopup KeyPopup { get; }
            public AdvancedDropdownState State { get; }

            public KeyPopupCache(AdvancedKeyPopup keyPopup, AdvancedDropdownState state)
            {
                KeyPopup = keyPopup;
                State = state;
            }
        }

        private GUIContent[] Keys
        {
            get
            {
                var keys = new List<GUIContent>();
                LocalizationMap[] maps = GetMaps();
                for (int i = maps.Length - 1; i >= 0; i--)
                {
                    IReadOnlyCollection<ILanguageGroup> groups = maps[i].Groups;
                    foreach (var group in groups)
                    {
                        foreach (var termInfo in group.TermInfos)
                        {
                            keys.Add(new GUIContent(termInfo.Key));
                        }
                    }
                }

                return keys.ToArray();
            }
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position, "Unsupported property type!, Please use string type!", MessageType.Error);
                return;
            }
            
            EditorGUI.BeginProperty(position, label, property);
            var popupPosition = new Rect(position);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            if (EditorGUI.DropdownButton(popupPosition, GetLastKey(property), FocusType.Keyboard))
            {
                KeyPopupCache popup = GetKeyPopup(property);
                popup.KeyPopup.Show(popupPosition);
            }
            
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }

        private LocalizationMap[] GetMaps()
        {
            var maps = new List<LocalizationMap>();
            string[] assetGuids = AssetDatabase.FindAssets($"t:{typeof(LocalizationMap)}");
            for (int i = assetGuids.Length - 1; i >= 0; i--)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
                var map = AssetDatabase.LoadAssetAtPath<LocalizationMap>(path);
                if (map != null && !maps.Contains(map))
                {
                    maps.Add(map);
                }
            }

            return maps.ToArray();
        }
        
        private KeyPopupCache GetKeyPopup(SerializedProperty property)
        {
            var state = new AdvancedDropdownState();
            var popup = new AdvancedKeyPopup(Keys, state);
            popup.OnItemSelected += item =>
            {
                property.stringValue = item.name;
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            };
            return new KeyPopupCache(popup, state);
        }

        private GUIContent GetLastKey(SerializedProperty property)
        {
            string lastValue = property.stringValue;
            int lastSelectedId = -1;
            GUIContent[] keys = Keys;
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                if (keys[i].text == lastValue)
                {
                    lastSelectedId = i;
                    break;
                }
            }

            return lastSelectedId > -1 ? keys[lastSelectedId] : new GUIContent("None");
        }
    }
}