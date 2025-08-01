using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class MenuExtend
{
    static MenuExtend()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
        ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
    }

    private static void OnRightToolbarGUI()
    {
        GUILayout.FlexibleSpace();
    }

    static void OnLeftToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("切换到入口场景", "切换到入口场景")))
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Boot.unity");
        }

        // if (GUILayout.Button(new GUIContent("切换到地图场景", "切换到地图场景")))
        // {
        //     UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Resources/i18n_default/scenes/map001.unity");
        // }

        if (GUILayout.Button(new GUIContent("切换到物理测试场景", "切换到物理测试场景")))
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/PhysicsTest.unity");
        }
    }
}
