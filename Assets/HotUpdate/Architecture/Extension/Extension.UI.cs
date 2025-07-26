/*
****** Author:'WCC'、'HGH'
****** Description:UI元素扩展方法
****** Create:2024.07.31 20:32
*/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static partial class Extension
{
    #region Layout
    /// <summary>
    /// 刷新布局
    /// </summary>
    /// <param name="gameObject"></param>
    public static void ForceRebuildLayout(this GameObject gameObject)
    {
        foreach (var group in gameObject.GetComponentsInChildren<LayoutGroup>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)group.transform);
        }
    }

    /// <summary>
    /// 刷新布局
    /// </summary>
    /// <param name="component"></param>
    public static void ForceRebuildLayout(this Component component)
    {
        foreach (var group in component.GetComponentsInChildren<LayoutGroup>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)group.transform);
        }
    }

    public static void RefreshLayoutGroup(this HorizontalOrVerticalLayoutGroup group)
    {
        foreach (var item in group.GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(group.transform as RectTransform);
    }
    #endregion

    #region UI Display
    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="renderer"></param>
    public static void Show(this CanvasRenderer renderer)
    {
        renderer.SetAlpha(1);
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    /// <param name="renderer"></param>
    public static void Hide(this CanvasRenderer renderer)
    {
        renderer.SetAlpha(0);
    }

    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="group"></param>
    public static void Show(this CanvasGroup group)
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    /// <param name="group"></param>
    public static void Hide(this CanvasGroup group)
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
    }

    /// <summary>
    /// 显示或隐藏
    /// </summary>
    /// <param name="group"></param>
    /// <param name="show"></param>
    public static void Display(this CanvasGroup group, bool show)
    {
        if (show)
        {
            group.alpha = 1;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = 0;
            group.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// 显示或隐藏
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="show"></param>
    public static void Display(this CanvasRenderer renderer, bool show)
    {
        if (show)
        {
            renderer.SetAlpha(1);
        }
        else
        {
            renderer.SetAlpha(0);
        }
    }

    #endregion

    #region UI Event
    /// <summary>
    /// 绑定事件
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Button Subscribe(this Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
        return btn;
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    /// <param name="tog"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Toggle Subscribe(this Toggle tog, UnityAction<bool> action)
    {
        tog.onValueChanged.AddListener(action);
        return tog;
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Slider Subscribe(this Slider slider, UnityAction<float> action)
    {
        slider.onValueChanged.AddListener(action);
        return slider;
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    /// <param name="dropdown"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Dropdown Subscribe(this Dropdown dropdown, UnityAction<int> action)
    {
        dropdown.onValueChanged.AddListener(action);
        return dropdown;
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    /// <param name="inputField"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static InputField Subscribe(this InputField inputField, UnityAction<string> action)
    {
        inputField.onValueChanged.AddListener(action);
        return inputField;
    }

    /// <summary>
    /// 取消绑定事件
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Button UnSubscribe(this Button btn, UnityAction action)
    {
        btn.onClick.RemoveListener(action);
        return btn;
    }

    /// <summary>
    /// 取消绑定事件
    /// </summary>
    /// <param name="tog"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Toggle UnSubscribe(this Toggle tog, UnityAction<bool> action)
    {
        tog.onValueChanged.RemoveListener(action);
        return tog;
    }

    /// <summary>
    /// 取消绑定事件
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Slider UnSubscribe(this Slider slider, UnityAction<float> action)
    {
        slider.onValueChanged.RemoveListener(action);
        return slider;
    }

    /// <summary>
    /// 取消绑定事件
    /// </summary>
    /// <param name="dropdown"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Dropdown UnSubscribe(this Dropdown dropdown, UnityAction<int> action)
    {
        dropdown.onValueChanged.RemoveListener(action);
        return dropdown;
    }

    /// <summary>
    /// 取消绑定事件
    /// </summary>
    /// <param name="dropdown"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static InputField UnSubscribe(this InputField dropdown, UnityAction<string> action)
    {
        dropdown.onValueChanged.RemoveListener(action);
        return dropdown;
    }

    /// <summary>
    /// 取消绑定所有事件
    /// </summary>
    /// <param name="btn"></param>
    /// <returns></returns>
    public static Button UnSubscribeAll(this Button btn)
    {
        btn.onClick.RemoveAllListeners();
        return btn;
    }

    /// <summary>
    /// 取消绑定所有事件
    /// </summary>
    /// <param name="tog"></param>
    /// <returns></returns>
    public static Toggle UnSubscribeAll(this Toggle tog)
    {
        tog.onValueChanged.RemoveAllListeners();
        return tog;
    }

    /// <summary>
    /// 取消邦迪管所有事件
    /// </summary>
    /// <param name="slider"></param>
    /// <returns></returns>
    public static Slider UnSubscribeAll(this Slider slider)
    {
        slider.onValueChanged.RemoveAllListeners();
        return slider;
    }

    /// <summary>
    /// 取消绑定所有事件
    /// </summary>
    /// <param name="dropdown"></param>
    /// <returns></returns>
    public static Dropdown UnSubscribeAll(this Dropdown dropdown)
    {
        dropdown.onValueChanged.RemoveAllListeners();
        return dropdown;
    }

    /// <summary>
    /// 取消绑定所有事件
    /// </summary>
    /// <param name="dropdown"></param>
    /// <returns></returns>
    public static InputField UnSubscribeAll(this InputField dropdown)
    {
        dropdown.onValueChanged.RemoveAllListeners();
        return dropdown;
    }

    #endregion

    public static Vector3 GetAbsolutePosition(this RectTransform rect, Canvas canvas)
    {
        Vector3[] targetCorners = new Vector3[4];
        rect.GetWorldCorners(targetCorners);

        Vector3 center;
        // 计算中心点
        center.x = targetCorners[0].x + (targetCorners[3].x - targetCorners[0].x) / 2;
        center.y = targetCorners[0].y + (targetCorners[1].y - targetCorners[0].y) / 2;
        center.z = rect.position.z;

        return center;
    }
}
