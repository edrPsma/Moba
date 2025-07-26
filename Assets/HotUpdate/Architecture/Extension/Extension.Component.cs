using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extension
{
    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Component target) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.gameObject.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="target"></param>
    /// <param name="add">是否添加</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Component target, out bool add) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.gameObject.AddComponent<T>();
            add = true;
            return component;
        }
        add = false;
        return component;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="target"></param>
    /// <param name="addAction">添加组件后执行的回调</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Component target, Action<T> addAction) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.gameObject.AddComponent<T>();
            addAction?.Invoke(component);
            return component;
        }
        return component;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="target"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject target) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        component ??= target.gameObject.AddComponent<T>();
        return component;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="target"></param>
    /// <param name="add">是否添加</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject target, out bool add) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.gameObject.AddComponent<T>();
            add = true;
            return component;
        }
        add = false;
        return component;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    /// <param name="target"></param>
    /// <param name="addAction">添加组件后执行的回调</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject target, Action<T> addAction) where T : UnityEngine.Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.gameObject.AddComponent<T>();
            addAction?.Invoke(component);
            return component;
        }
        return component;
    }

    /// <summary>
    /// 重置Transform的位置、旋转和缩放
    /// </summary>
    /// <param name="target"></param>
    public static void ResetTransform(this Component target)
    {
        target.transform.position = Vector3.zero;
        target.transform.localScale = Vector3.one;
        target.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 重置Transform的位置、旋转和缩放
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static GameObject ResetTransform(this GameObject target)
    {
        target.transform.position = Vector3.zero;
        target.transform.localScale = Vector3.one;
        target.transform.rotation = Quaternion.identity;
        return target;
    }

    /// <summary>
    /// 设置显隐
    /// </summary>
    /// <param name="component"></param>
    /// <param name="active"></param>
    public static void SetActive(this Component component, bool active)
    {
        component.gameObject.SetActive(active);
    }

    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="component"></param>
    /// <param name="parent">父物体</param>
    /// <param name="worldPositionStays">是否保留世界位置</param>
    public static void SetParent(this Component component, Transform parent, bool worldPositionStays = false)
    {
        component.transform.SetParent(parent, worldPositionStays);
    }

    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="component"></param>
    /// <param name="parent">父物体</param>
    /// <param name="worldPositionStays">是否保留世界位置</param>
    public static void SetParent(this Component component, GameObject parent, bool worldPositionStays = false)
    {
        component.transform.SetParent(parent.transform, worldPositionStays);
    }

    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="parent">父物体</param>
    /// <param name="worldPositionStays">是否保留世界位置</param>
    /// <returns></returns>
    public static GameObject SetParent(this GameObject gameObject, Transform parent, bool worldPositionStays = false)
    {
        gameObject.transform.SetParent(parent, worldPositionStays);
        return gameObject;
    }

    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="parent">父物体</param>
    /// <param name="worldPositionStays">是否保留世界位置</param>
    /// <returns></returns>
    public static GameObject SetParent(this GameObject gameObject, GameObject parent, bool worldPositionStays = false)
    {
        gameObject.transform.SetParent(parent.transform, worldPositionStays);
        return gameObject;
    }

    /// <summary>
    /// 递归查找第一个符合条件的组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="component">组件</param>
    /// <param name="name">物体名字</param>
    /// <returns></returns>
    public static T Find<T>(this Component component, string name) where T : Component
    {
        int childCount = component.transform.childCount;
        T t = null;
        for (int i = 0; i < childCount; i++)
        {
            Transform cur = component.transform.GetChild(i);
            if (cur.name != name)
            {
                t = cur.Find<T>(name);
            }
            else
            {
                t = cur.GetComponent<T>();
                if (t == null)
                {
                    t = cur.Find<T>(name);
                }
            }

            if (t != null) break;
        }

        return t;
    }

    /// <summary>
    /// 递归查找第一个符合条件的组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="gameObject">游戏物体</param>
    /// <param name="name">物体名字</param>
    /// <returns></returns>
    public static T Find<T>(this GameObject gameObject, string name) where T : Component
    {
        return gameObject.transform.Find<T>(name);
    }

    /// <summary>
    /// 通过路径递归查找第一个符合条件的物体
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Transform FindByPath(this GameObject gameObject, string path)
    {
        return gameObject.transform.FindByPath(path);
    }

    /// <summary>
    /// 通过路径递归查找第一个符合条件的物体
    /// </summary>
    /// <param name="component"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Transform FindByPath(this Component component, string path)
    {
        if (path == component.name) return component.transform;

        string[] arr = path.Split("/");
        if (component.name == arr[0])
        {
            return FindByPath(component.transform, arr, 1);
        }
        else
        {
            return FindByPath(component.transform, arr, 0);
        }
    }

    static Transform FindByPath(Transform transform, string[] arr, int index)
    {
        if (transform == null) return null;
        if (index >= arr.Length) return null;

        int childCount = transform.childCount; ;
        for (int i = 0; i < childCount; i++)
        {
            Transform cur = transform.GetChild(i);
            if (cur.name != arr[index])
            {
                Transform trans = FindByPath(cur, arr, index);
                if (trans != null) return trans;
            }
            else
            {
                if (index == arr.Length - 1)
                {
                    return cur;
                }
                else
                {
                    Transform trans = FindByPath(cur, arr, index + 1);
                    if (trans != null) return trans;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 获取某个对象不超过指定最大递归深度的所有子对象
    /// </summary>
    /// <param name="result">结果集</param>
    /// <param name="includeInactive">是否包含未激活对象</param>
    /// <param name="maxRecursionDepth">最大递归深度</param>
    public static void GetChildren(this GameObject gameObject, List<GameObject> result, bool includeInactive = false, int maxRecursionDepth = 10)
    {
        if (gameObject == null || result == null)
        {
            return;
        }

        GetChildrenInternal(gameObject.transform, result, includeInactive, maxRecursionDepth);
    }

    /// <summary>
    /// 获取某个对象不超过指定最大递归深度的所有子对象
    /// </summary>
    /// <param name="result">结果集</param>
    /// <param name="includeInactive">是否包含未激活对象</param>
    /// <param name="maxRecursionDepth">最大递归深度</param>
    public static void GetChildren(this Transform transform, List<Transform> result, bool includeInactive = false, int maxRecursionDepth = 10)
    {
        if (transform == null || result == null)
        {
            return;
        }

        GetChildrenInternal(transform, result, includeInactive, maxRecursionDepth);
    }

    private static void GetChildrenInternal(Transform transform, List<GameObject> result, bool includeInactive, int maxRecursionDepth)
    {
        if (maxRecursionDepth == 0)
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (includeInactive || child.gameObject.activeSelf)
            {
                result.Add(child.gameObject);
                GetChildrenInternal(child, result, includeInactive, maxRecursionDepth - 1);
            }
        }
    }

    private static void GetChildrenInternal(Transform transform, List<Transform> result, bool includeInactive, int maxRecursionDepth)
    {
        if (maxRecursionDepth == 0)
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (includeInactive || child.gameObject.activeSelf)
            {
                result.Add(child);
                GetChildrenInternal(child, result, includeInactive, maxRecursionDepth - 1);
            }
        }
    }

    /// <summary>
    /// 判断 ScrollRect 是否已经停止滚动
    /// </summary>
    /// <param name="threshold">速度阈值</param>
    public static bool IsScrollStopped(this ScrollRect scrollRect, float threshold = 0.01f)
    {
        return scrollRect != null && scrollRect.velocity.sqrMagnitude < threshold * threshold;
    }
}
