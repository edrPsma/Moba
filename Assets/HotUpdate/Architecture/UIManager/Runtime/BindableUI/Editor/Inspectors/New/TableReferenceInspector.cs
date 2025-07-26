using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Inspectors
{
    public abstract class TableReferenceInspector : ReferenceInspector
    {
        protected abstract (string, string)[] PropertyArray { get; }
        protected abstract SerializedProperty Property { get; }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                EditorGUILayout.BeginHorizontal(BoxStyle);

                for (int i = 0; i < PropertyArray.Length; i++)
                {
                    DrawTitleField(PropertyArray[i].Item1);
                }

                DrawOtherTitle();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);

                for (var index = 0; index < Property.arraySize; index++)
                {
                    var element = Property.GetArrayElementAtIndex(index);

                    EditorGUILayout.BeginHorizontal(BoxStyle);
                    DrawElement(element, index);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawElement(SerializedProperty element, int index)
        {
            for (int i = 0; i < PropertyArray.Length; i++)
            {
                var property = element.FindPropertyRelative(PropertyArray[i].Item2);
                EditorGUILayout.PropertyField(property, GUIContent.none);
            }

            // DrawOtherFields(element, index);

            if (GUILayout.Button("✖", GUILayout.Width(ToggleTitleSize)))
            {
                Property.DeleteArrayElementAtIndex(index);
            }
        }

        protected void DrawTitleField(string title)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(title, TitleStyle.Center);
            EditorGUI.EndDisabledGroup();
        }

        protected virtual void DrawOtherTitle() { }


    }
}