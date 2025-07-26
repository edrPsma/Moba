using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BindableUI.Editor.Inspectors;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Core
{
    public class BindComponentInspector
    {
        private readonly SerializedObject _serializedObject;
        private readonly List<ReferenceInspector> _inspectors;
        private readonly BindComponent _bindComponent;
        private readonly string[] _tabs;

        private static int _index;

        public BindComponentInspector(SerializedObject serializedObject, BindComponent bindComponent)
        {
            _serializedObject = serializedObject;
            _bindComponent = bindComponent;
            _inspectors = new List<ReferenceInspector>();
            CollectInspector();
            _tabs = new string[_inspectors.Count];
        }

        public void OnInspectorGUI()
        {
            UpdateTabs();
            _index = GUILayout.SelectionGrid(_index, _tabs, 4);
            _inspectors[_index].OnInspectorGUI();
        }

        private void UpdateTabs()
        {
            for (var index = 0; index < _tabs.Length; index++)
            {
                _tabs[index] = _inspectors[index].TabName;
            }
        }

        private void CollectInspector()
        {
            _inspectors.Clear();
            var list = TypeCache.GetTypesWithAttribute<ReferenceInspectorAttribute>();
            foreach (Type type in list)
            {
                var inspector = typeof(BindComponentInspector).Assembly.CreateInstance(type.FullName) as ReferenceInspector;
                inspector.SerializedObject = _serializedObject;
                inspector.BindComponent = _bindComponent;
                _inspectors.Add(inspector);
            }
            _inspectors.Sort(Sort);
        }

        private int Sort(ReferenceInspector x, ReferenceInspector y)
        {
            if (x.Order == y.Order)
            {
                return string.Compare(x.TabName, y.TabName, StringComparison.Ordinal);
            }
            else
            {
                return x.Order - y.Order;
            }
        }
    }
}