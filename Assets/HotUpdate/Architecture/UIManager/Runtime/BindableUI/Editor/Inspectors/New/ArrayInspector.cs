using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Inspectors
{
    [ReferenceInspector]
    public class ArrayInspector : ReferenceInspector
    {
        public override string TabName => "数组";
        public override int Order => 2;

        protected SerializedProperty Property => SerializedObject.FindProperty(nameof(BindComponent.ArrayDatas));

        protected (string, string)[] PropertyArray => new (string, string)[]
        {
            ("名称",nameof(BindArrayData.Name)),
            ("类型",nameof(BindArrayData.Type)),
            ("引用",nameof(BindArrayData.Reference)),
        };

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                EditorGUILayout.BeginHorizontal(BoxStyle);

                DrawTitleField(PropertyArray[0].Item1);
                DrawTitleField(PropertyArray[1].Item2);

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
            var name = element.FindPropertyRelative(PropertyArray[0].Item2);
            var type = element.FindPropertyRelative(PropertyArray[1].Item2);
            var reference = element.FindPropertyRelative(PropertyArray[2].Item2);
            var bind = element.FindPropertyRelative(nameof(BindArrayData.BindReference));

            EditorGUILayout.BeginHorizontal(BoxStyle);
            EditorGUILayout.PropertyField(name, GUIContent.none);

            string[] types = SearAllBindableType(element);
            if (types == null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("None");
                EditorGUI.EndDisabledGroup();
                bind.arraySize = 0;
            }
            else
            {
                int curIndex = 0;
                for (int i = 0; i < types.Length; i++)
                {
                    if (type.stringValue == types[i])
                    {
                        curIndex = i;
                        break;
                    }
                }
                int selectIndex = EditorGUILayout.Popup(curIndex, SearAllBindableType(element));
                type.stringValue = types[selectIndex];

                bind.arraySize = reference.arraySize;
                for (int i = 0; i < reference.arraySize; i++)
                {
                    GameObject gameObject = reference.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;
                    if (types[selectIndex] == nameof(GameObject) || gameObject == null)
                    {
                        bind.GetArrayElementAtIndex(i).objectReferenceValue = gameObject;
                    }
                    else
                    {
                        bind.GetArrayElementAtIndex(i).objectReferenceValue = gameObject.GetComponent(types[selectIndex]);
                    }
                }
            }

            if (GUILayout.Button("✖", GUILayout.Width(ToggleTitleSize)))
            {
                Property.DeleteArrayElementAtIndex(index);
                return;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(BoxStyle);
            EditorGUILayout.PropertyField(reference, new GUIContent("引用"));
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        private string[] SearAllBindableType(SerializedProperty element)
        {
            SerializedProperty property = element.FindPropertyRelative(PropertyArray[2].Item2);
            if (property.arraySize == 0)
            {
                return null;
            }
            else
            {
                List<string[]> types = new List<string[]>();
                List<string> allTypes = new List<string>();

                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty temp = property.GetArrayElementAtIndex(i);
                    GameObject targetObj = temp.objectReferenceValue as GameObject;
                    if (targetObj != null)
                    {
                        var arr = targetObj.GetComponents<Component>().Select(item => item.GetType().Name).ToArray();
                        types.Add(arr);
                        allTypes.AddRange(arr);
                    }
                }

                for (int i = 0; i < types.Count; i++)
                {
                    allTypes = allTypes.Intersect(types[i]).ToList();
                }

                if (allTypes.Count != 0)
                {
                    allTypes.Insert(0, nameof(GameObject));
                }

                return allTypes.Count == 0 ? null : allTypes.ToArray();
            }
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

                SerializedProperty property1 = property.FindPropertyRelative(PropertyArray[0].Item2);
                BindComponentEditorTools.SetDefultValue(property1);

                SerializedProperty property2 = property.FindPropertyRelative(PropertyArray[1].Item2);
                BindComponentEditorTools.SetDefultValue(property2);

                SerializedProperty property3 = property.FindPropertyRelative(PropertyArray[2].Item2);
                property3.arraySize = 0;

                SerializedProperty property4 = property.FindPropertyRelative(nameof(BindArrayData.BindReference));
                property4.arraySize = 0;
            }
        }
    }
}