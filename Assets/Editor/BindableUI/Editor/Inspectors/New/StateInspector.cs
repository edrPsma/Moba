using System.Collections;
using System.Collections.Generic;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Inspectors
{
    [ReferenceInspector]
    public class StateInspector : ReferenceInspector
    {
        public override string TabName => "状态";
        public override int Order => 3;
        protected SerializedProperty Property => SerializedObject.FindProperty(nameof(BindComponent.StateDatas));

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                EditorGUILayout.BeginHorizontal(BoxStyle);

                DrawTitleField("状态列表");

                DrawOtherTitle();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);

                for (var index = 0; index < Property.arraySize; index++)
                {
                    var element = Property.GetArrayElementAtIndex(index);

                    DrawElement(element, index);
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawElement(SerializedProperty element, int index)
        {
            var name = element.FindPropertyRelative(nameof(BindStateGroup.Name));
            var desc = element.FindPropertyRelative(nameof(BindStateGroup.Desc));
            var stateGroups = element.FindPropertyRelative(nameof(BindStateGroup.StateGroups));

            EditorGUILayout.BeginHorizontal(BoxStyle);
            EditorGUILayout.LabelField("名称:", GUILayout.Width(40));
            EditorGUILayout.PropertyField(name, GUIContent.none);

            if (GUILayout.Button("Test", GUILayout.Width(60)))
            {
                BindStateData[] datas = BindComponent.StateDatas[index].StateGroups;
                for (int i = 0; i < datas.Length; i++)
                {
                    datas[i].BindState.Invoke();
                }
                EditorUtility.SetDirty(BindComponent.gameObject);
                EditorUtility.SetDirty(BindComponent);
            }

            if (GUILayout.Button("✖", GUILayout.Width(ToggleTitleSize)))
            {
                Property.DeleteArrayElementAtIndex(index);
                return;
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginVertical(BoxStyle);
            EditorGUILayout.LabelField("描述:");
            desc.stringValue = EditorGUILayout.TextArea(desc.stringValue, GUILayout.MinHeight(30));
            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(BoxStyle);
            EditorGUILayout.PropertyField(stateGroups, new GUIContent("动作组"));
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        protected void DrawTitleField(string title)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextArea(title, TitleStyle.Center);
            EditorGUI.EndDisabledGroup();
        }

        protected void DrawOtherTitle()
        {
            if (GUILayout.Button("+", GUILayout.Width(ToggleTitleSize)))
            {
                Property.arraySize++;

                // 获取新添加的元素
                SerializedProperty property = Property.GetArrayElementAtIndex(Property.arraySize - 1);

                SerializedProperty property1 = property.FindPropertyRelative(nameof(BindStateGroup.Name));
                BindComponentEditorTools.SetDefultValue(property1);

                SerializedProperty property3 = property.FindPropertyRelative(nameof(BindStateGroup.StateGroups));
                property3.arraySize = 0;
            }
        }
    }
}