using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MbsCore.Localization.Editor
{
    internal sealed class AdvancedKeyPopup : AdvancedDropdown
    {
        public event Action<AdvancedDropdownItem> OnItemSelected; 
		
        private static readonly float s_headerHeight = EditorGUIUtility.singleLineHeight * 2f;

        private readonly string[] _keys;

        public AdvancedKeyPopup(GUIContent[] guiKeys, AdvancedDropdownState state) : base(state)
        {
            var tempKeys = new List<string>();
            foreach (var guiKey in guiKeys)
            {
                tempKeys.Add(guiKey.text);
            }

            _keys = tempKeys.ToArray();
        }
		
        public static void AddTo(AdvancedDropdownItem root, IEnumerable<string> locKeys)
        {
            int itemCount = 0;
            var nullItem = new AdvancedDropdownItem("None")
                    {
                            id = itemCount++
                    };
			
            root.AddChild(nullItem);
            foreach (var locKey in locKeys)
            {
                var item = new AdvancedDropdownItem(locKey)
                        {
                                id = itemCount++,
                        };
                root.AddChild(item);
            }
        }
		
        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Select Localization Key");
            AddTo(root, _keys);
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            OnItemSelected?.Invoke(item);
        }
    }
}