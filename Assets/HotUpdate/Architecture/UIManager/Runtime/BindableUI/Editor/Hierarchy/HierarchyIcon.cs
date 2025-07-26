using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BindableUI.Runtime;
using UnityEditor;
using UnityEngine;

namespace BindableUI.Editor.Hierarchy
{
    public class HierarchyIcon
    {
        static Texture2D parent;
        static Texture2D child;

        [InitializeOnLoadMethod]
        static void Init()
        {
            // 加载自定义图标

            var texture = Resources.Load<EditorAsset>("EditorAsset").HierarchyIcon;
            // parent = CreateTexture(texture, Color.blue);
            // child = CreateTexture(texture, Color.yellow);
            parent = texture;
            child = texture;

            // 监听 Hierarchy 绘制事件
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            // 获取当前对象
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj != null)
            {
                BindComponent bindComponent = obj.GetComponent<BindComponent>();
                if (bindComponent != null)
                {
                    // 在 Hierarchy 中绘制图标
                    Rect iconRect = new Rect(selectionRect.xMax - 20, selectionRect.y, 16, 16);
                    GUI.Label(iconRect, parent);
                }
                else
                {
                    bindComponent = obj.GetComponentInParent<BindComponent>();
                    if (bindComponent != null)
                    {
                        bool condition = bindComponent.ComponentDatas.Any(item => item.Reference == obj);
                        bool condition2 = bindComponent.ArrayDatas.Any(item => item.Reference.Contains(obj));
                        if (condition || condition2)
                        {
                            Rect iconRect = new Rect(selectionRect.xMax - 20, selectionRect.y, 16, 16);
                            GUI.Label(iconRect, child);
                        }
                    }
                }
            }
        }
    }
}