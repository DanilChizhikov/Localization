using System;
using UnityEditor;
using UnityEngine;

namespace MbsCore.Localization.Editor
{
    internal static class EditorHelper
    {
        public static void HorizontalDraw(Action action, string style = "")
        {
            EditorGUILayout.BeginHorizontal(style);
            action?.Invoke();
            EditorGUILayout.EndHorizontal();
        }
        
        public static void VerticalDraw(Action action, string style = "")
        {
            EditorGUILayout.BeginVertical(style);
            action?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void ButtonDraw(string caption, Action callback)
        {
            if (GUILayout.Button(caption))
            {
                callback?.Invoke();
            }
        }
    }
}