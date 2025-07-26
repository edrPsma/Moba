using System.Collections;
using System.Collections.Generic;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Inspectors
{
    [ReferenceInspector]
    public class ComponentInspector : TableReferenceInspector
    {
        public override string TabName => "组件";
        public override int Order => 1;

        protected override (string, string)[] PropertyArray => new (string, string)[]
        {
            ("名称",nameof(BindComponentData.Name)),
            ("类型",nameof(BindComponentData.Type)),
            ("引用",nameof(BindComponentData.Reference)),
        };

        protected override SerializedProperty Property => SerializedObject.FindProperty(nameof(BindComponent.ComponentDatas));

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        protected override void DrawElement(SerializedProperty element, int index)
        {
            var name = element.FindPropertyRelative(PropertyArray[0].Item2);
            var type = element.FindPropertyRelative(PropertyArray[1].Item2);
            var reference = element.FindPropertyRelative(PropertyArray[2].Item2);

            GameObject targetObj = reference.objectReferenceValue as GameObject;

            if (targetObj != null && name.stringValue == string.Empty)
            {
                name.stringValue = targetObj.name;
            }
            EditorGUILayout.PropertyField(name, GUIContent.none);

            string[] types = SearAllBindableType(element);
            if (types == null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("None");
                EditorGUI.EndDisabledGroup();
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
                var bind = element.FindPropertyRelative(nameof(BindComponentData.BindReference));
                if (selectIndex == 0)
                {
                    bind.objectReferenceValue = targetObj;
                }
                else
                {
                    bind.objectReferenceValue = targetObj.GetComponent(types[selectIndex]);
                }
            }

            EditorGUILayout.PropertyField(reference, GUIContent.none);

            if (GUILayout.Button("✖", GUILayout.Width(ToggleTitleSize)))
            {
                Property.DeleteArrayElementAtIndex(index);
            }
        }

        protected override void DrawOtherTitle()
        {
            base.DrawOtherTitle();
            if (GUILayout.Button("+", GUILayout.Width(ToggleTitleSize)))
            {
                Property.arraySize++;

                // 获取新添加的元素
                SerializedProperty property = Property.GetArrayElementAtIndex(Property.arraySize - 1);

                for (int i = 0; i < PropertyArray.Length; i++)
                {
                    SerializedProperty childProperty = property.FindPropertyRelative(PropertyArray[i].Item2);
                    BindComponentEditorTools.SetDefultValue(childProperty);
                }
            }
        }

        string[] SearAllBindableType(SerializedProperty element)
        {
            SerializedProperty property = element.FindPropertyRelative(PropertyArray[2].Item2);
            if (property.objectReferenceValue == null)
            {
                return null;
            }
            else
            {
                List<string> types = new List<string>
                {
                    nameof(GameObject),
                };

                GameObject targetObj = property.objectReferenceValue as GameObject;

                if (targetObj != null)
                {
                    var components = targetObj.GetComponents<Component>();
                    foreach (var component in components)
                    {
                        types.Add(component.GetType().Name);
                    }
                }
                return types.ToArray();
            }
        }
    }
}