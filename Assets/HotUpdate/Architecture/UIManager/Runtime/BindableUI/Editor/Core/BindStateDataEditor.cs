using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Core
{
    [CustomPropertyDrawer(typeof(BindStateData))]
    public class BindStateDataEditor : PropertyDrawer
    {
        public static readonly GUIStyle BoxStyle = new GUIStyle();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var impactType = property.FindPropertyRelative(nameof(BindStateData.ImpactType));
            var bindState = property.FindPropertyRelative(nameof(BindStateData.BindState));
            float typeHeight = EditorGUI.GetPropertyHeight(impactType, true);
            float bindStateHeight = EditorGUI.GetPropertyHeight(impactType, true);

            var typeLabelRect = new Rect(position.x, position.y, 60, typeHeight);
            var typeRect = new Rect(position.x + 60, position.y, position.width - 60, typeHeight);
            var impactTypeLabelRect = new Rect(position.x, position.y + typeHeight, 60, typeHeight);
            var impactTypeRect = new Rect(position.x + 60, position.y + typeHeight, position.width - 60, typeHeight);
            var stateRect = new Rect(position.x, position.y + typeHeight * 2, position.width, bindStateHeight);

            EditorGUI.LabelField(typeLabelRect, new GUIContent("类型:"));
            Dictionary<string, List<Type>> typesMap = GetTypeMap();
            string[] typeGroup = typesMap.Select(item => item.Key).ToArray();
            Array.Sort(typeGroup);

            if (typeGroup == null || typeGroup.Length == 0)
            {
                EditorGUI.Popup(typeRect, 0, new string[] { "None" });
                EditorGUI.LabelField(impactTypeLabelRect, new GUIContent("效果类型:"));
                EditorGUI.Popup(impactTypeRect, 0, new string[] { "None" });
                bindState.managedReferenceValue = null;
                impactType.stringValue = string.Empty;
            }
            else
            {
                bool isTypeNull = true;
                Type impactTypeValue = null;
                string groupType = string.Empty;

                foreach (var item in typesMap)
                {
                    foreach (var temp in item.Value)
                    {
                        if (impactType.stringValue == temp.Name)
                        {
                            isTypeNull = false;
                            impactTypeValue = temp;
                            groupType = item.Key.ToString();
                            break;
                        }
                    }
                }

                if (isTypeNull)
                {
                    groupType = typeGroup[0].ToString(); ;
                    impactTypeValue = typesMap[groupType][0];
                    impactType.stringValue = impactTypeValue.Name;
                    bindState.managedReferenceValue = impactTypeValue.Assembly.CreateInstance(impactTypeValue.FullName);
                }
                else
                {
                    int GroupIndex = -1;

                    for (int i = 0; i < typeGroup.Length; i++)
                    {
                        if (groupType == typeGroup[i])
                        {
                            GroupIndex = i;
                            break;
                        }
                    }

                    int groupSelectIndex = EditorGUI.Popup(typeRect, GroupIndex == -1 ? 0 : GroupIndex, typeGroup);
                    if (GroupIndex == -1)// 更改组则设置Impact类型为该组的首个类型
                    {
                        impactTypeValue = typesMap[typeGroup[groupSelectIndex]][0];
                        impactType.stringValue = impactTypeValue.FullName;
                        bindState.managedReferenceValue = impactTypeValue.Assembly.CreateInstance(impactTypeValue.FullName);
                    }
                    else
                    {
                        int impactIndex = -1;

                        for (int i = 0; i < typesMap[typeGroup[groupSelectIndex]].Count; i++)
                        {
                            if (impactTypeValue.FullName == typesMap[typeGroup[groupSelectIndex]][i].FullName)
                            {
                                impactIndex = i;
                                break;
                            }
                        }

                        string[] impactTypeNames = typesMap[typeGroup[groupSelectIndex]]
                            .Select(item => item.GetCustomAttribute<BindImpactAttribute>().ImpactType).ToArray();
                        EditorGUI.LabelField(impactTypeLabelRect, new GUIContent("效果类型:"));
                        int impactSelectIndex = EditorGUI.Popup(impactTypeRect, impactIndex == -1 ? 0 : impactIndex, impactTypeNames);
                        if (impactSelectIndex != impactIndex || impactIndex == -1)// 更改Impact类型则设置BindState为该类型的实例
                        {
                            impactTypeValue = typesMap[typeGroup[groupSelectIndex]][impactSelectIndex];
                            impactType.stringValue = impactTypeValue.Name;
                            bindState.managedReferenceValue = impactTypeValue.Assembly.CreateInstance(impactTypeValue.FullName);
                        }
                    }
                }
            }

            EditorGUI.PropertyField(stateRect, bindState, new GUIContent("效果数据"), true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var impactType = property.FindPropertyRelative(nameof(BindStateData.ImpactType));
            var bindState = property.FindPropertyRelative(nameof(BindStateData.BindState));

            return EditorGUI.GetPropertyHeight(impactType, true) * 2 + EditorGUI.GetPropertyHeight(bindState, true);
        }

        Dictionary<string, List<Type>> GetTypeMap()
        {
            Dictionary<string, List<Type>> result = new Dictionary<string, List<Type>>();
            var list = TypeCache.GetTypesWithAttribute<BindImpactAttribute>()
                .Where(item => item.GetInterface(typeof(IBindImpact).Name) == typeof(IBindImpact))
                .ToArray();

            for (int i = 0; i < list.Length; i++)
            {
                BindImpactAttribute attribute = list[i].GetCustomAttribute<BindImpactAttribute>();
                string temp = attribute.GroupType.ToString();
                if (result.ContainsKey(temp))
                {
                    result[temp].Add(list[i]);
                }
                else
                {
                    result.Add(temp, new List<Type> { list[i] });
                }
            }
            return result;
        }
    }
}