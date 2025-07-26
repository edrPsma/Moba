using System.Collections;
using System.Collections.Generic;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Inspectors
{
    [ReferenceInspector]
    public class AssetInspector : TableReferenceInspector
    {
        public override string TabName => "资源";
        public override int Order => 4;

        protected override (string, string)[] PropertyArray => new (string, string)[]
        {
            ("名称",nameof(BindAssetData.Name)),
            ("引用",nameof(BindAssetData.BindReference)),
        };

        protected override SerializedProperty Property => SerializedObject.FindProperty(nameof(BindComponent.AssetDatas));

        protected override void DrawElement(SerializedProperty element, int index)
        {
            var name = element.FindPropertyRelative(PropertyArray[0].Item2);
            var reference = element.FindPropertyRelative(PropertyArray[1].Item2);

            Object targetObj = reference.objectReferenceValue;

            if (targetObj != null && name.stringValue == string.Empty)
            {
                name.stringValue = targetObj.name;
            }
            base.DrawElement(element, index);
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
    }
}