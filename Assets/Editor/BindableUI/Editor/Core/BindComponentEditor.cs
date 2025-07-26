using System.Collections;
using System.Collections.Generic;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Core
{
    [CustomEditor(typeof(BindComponent))]
    public class BindComponentEditor : UnityEditor.Editor
    {
        BindComponentInspector _inspector;
        BindComponent _bindComponent;

        private void OnEnable()
        {
            if (target == null) return;

            _bindComponent = serializedObject.targetObject as BindComponent;
            _inspector = new BindComponentInspector(serializedObject, _bindComponent);
        }

        public override void OnInspectorGUI()
        {
            if (target == null) return;

            serializedObject.UpdateIfRequiredOrScript();
            _inspector.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();

            // EditorGUILayout.BeginVertical();
            // EditorGUI.BeginDisabledGroup(true);
            // EditorGUI.EndDisabledGroup();

            // if (Application.isPlaying)
            // {
            //     var history = _statefulComponent.StateHistory
            //         .ConvertAll(state => RoleUtils.GetName(typeof(StateRoleAttribute), state))
            //         .ToList();

            //     _isHistoryFoldout = DrawList(_isHistoryFoldout, nameof(StatefulComponent.StateHistory), history);
            // }

            // EditorGUILayout.EndVertical();
        }
    }
}