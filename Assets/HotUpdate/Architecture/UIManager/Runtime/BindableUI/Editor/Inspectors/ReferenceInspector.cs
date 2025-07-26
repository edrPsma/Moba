using System.Collections;
using System.Collections.Generic;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Inspectors
{
    public abstract class ReferenceInspector
    {
        public static readonly GUIStyle BoxStyle = new GUIStyle();

        protected const int ToggleSize = 16;
        protected const int ToggleTitleSize = 32;

        public abstract string TabName { get; }
        public abstract int Order { get; }
        public SerializedObject SerializedObject { get; set; }
        public BindComponent BindComponent { get; set; }

        public abstract void OnInspectorGUI();
    }
}