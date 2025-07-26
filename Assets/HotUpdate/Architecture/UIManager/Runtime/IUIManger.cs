using System;
using Observable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 相机
        /// </summary>
        /// <value></value>
        Camera UICamera { get; }

        /// <summary>
        /// 根画布
        /// </summary>
        /// <value></value>
        Canvas Canvas { get; }

        /// <summary>
        /// Unity事件系统组件
        /// </summary>
        EventSystem UnityEventSystem { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uILoader"></param>
        void Initialize(IUILoader uILoader);

        /// <summary>
        /// 打开一个界面
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        void Push<T>() where T : UIForm, new();

        /// <summary>
        /// 一个界面于指定ui组
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="group">ui组</param>
        void Push<T>(UIGroup group) where T : UIForm, new();

        /// <summary>
        /// 异步打开一个界面
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        void PushAsync<T>() where T : UIForm, new();

        /// <summary>
        /// 异步打开一个界面于指定ui组
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="group">ui组</param>
        void PushAsync<T>(UIGroup group) where T : UIForm, new();

        /// <summary>
        /// 打开一个界面并设置一个用户数据
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <typeparam name="TData">用户数据类型</typeparam>
        /// <param name="data">用户数据</param>
        void Push<T, TData>(TData data) where T : UIForm<TData>, new();

        /// <summary>
        /// 打开一个界面于指定ui组并设置一个用户数据
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <typeparam name="TData">用户数据类型</typeparam>
        /// <param name="group">ui组</param>
        /// <param name="data">用户数据</param>
        void Push<T, TData>(UIGroup group, TData data) where T : UIForm<TData>, new();

        /// <summary>
        /// 异步打开一个界面并设置一个用户数据
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <typeparam name="TData">用户数据类型</typeparam>
        /// <param name="data">用户数据</param>
        void PushAsync<T, TData>(TData data) where T : UIForm<TData>, new();

        /// <summary>
        /// 异步打开一个界面于指定ui组并设置一个用户数据
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <typeparam name="TData">用户数据类型</typeparam>
        /// <param name="group">ui组</param>
        /// <param name="data">用户数据</param>
        void PushAsync<T, TData>(UIGroup group, TData data) where T : UIForm<TData>, new();

        /// <summary>
        /// 关闭一个界面
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="destroy">是否销毁</param>
        void Pop<T>(bool destroy = true) where T : class, IUIForm, new();

        /// <summary>
        /// 关闭指定界面以上的界面
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="destroy">是否销毁</param>
        void PopAllUpper<T>(bool destroy = true) where T : class, IUIForm, new();

        /// <summary>
        /// 关闭指定界面以下的界面
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="destroy">是否销毁</param>
        void PopAllLower<T>(bool destroy = true) where T : class, IUIForm, new();

        /// <summary>
        /// 关闭指定ui组的所有界面
        /// </summary>
        /// <param name="group">ui组</param>
        /// <param name="destroy">是否销毁</param>
        void PopAll(UIGroup group, bool destroy = true);

        /// <summary>
        /// 销毁指定界面，无论其是否关闭
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        void Destroy<T>() where T : class, IUIForm, new();

        /// <summary>
        /// 指定界面是否已打开
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <returns>是否打开</returns>
        bool IsOpened<T>() where T : class, IUIForm, new();

        /// <summary>
        /// 指定界面是否已打开
        /// </summary>
        /// <param name="type">表单类型</param>
        /// <returns>是否打开</returns>
        bool IsOpened(Type type);

        /// <summary>
        /// 搜索表单
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <returns>表单实例</returns>
        T SearchForm<T>() where T : class, IUIForm, new();

        /// <summary>
        /// 搜索表单
        /// </summary>
        /// <param name="type">表单类型</param>
        /// <returns>表单实例</returns>
        IUIForm SearchForm(Type type);

        /// <summary>
        /// 搜索面板
        /// </summary>
        /// <param name="name">ui面板名字</param>
        /// <returns>ui面板</returns>
        GameObject SearchPanel(string name);

        /// <summary>
        /// 获取指定ui组
        /// </summary>
        /// <param name="group">ui组</param>
        /// <returns></returns>
        RectTransform GetGroupNode(UIGroup group);

        /// <summary>
        /// 获取指定ui组的基础层级
        /// </summary>
        /// <param name="group">ui组</param>
        /// <returns></returns>
        int GetOrderLayer(UIGroup group);

        /// <summary>
        /// 隐藏指定ui层的ui
        /// </summary>
        /// <param name="uiGroup"></param>
        void HideGroup(UIGroup uiGroup);

        /// <summary>
        /// 显示指定ui层的ui
        /// </summary>
        /// <param name="uiGroup"></param>
        void ShowGroup(UIGroup uiGroup);

        /// <summary>
        /// 订阅UI打开事件
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="onOpen">打开回调</param>
        IUnRegister SubscribeOnOpen<T>(Action<IUIForm> onOpen) where T : IUIForm;

        /// <summary>
        /// 取消订阅UI打开事件
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="onOpen">打开回调</param>
        void UnsubscribeOnOpen<T>(Action<IUIForm> onOpen) where T : IUIForm;

        /// <summary>
        /// 订阅UI关闭事件
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="onClose">关闭回调</param>
        IUnRegister SubscribeOnClose<T>(Action<IUIForm> onClose) where T : IUIForm;

        /// <summary>
        /// 取消订阅UI关闭事件
        /// </summary>
        /// <typeparam name="T">表单类型</typeparam>
        /// <param name="onClose">关闭回调</param>
        void UnsubscribeOnClose<T>(Action<IUIForm> onClose) where T : IUIForm;

        /// <summary>
        /// 订阅UI打开事件
        /// </summary>
        /// <param name="onOpen"></param>
        IUnRegister SubscribeOnOpen(Action<IUIForm> onOpen);

        /// <summary>
        /// 取消订阅UI打开事件
        /// </summary>
        /// <param name="onOpen"></param>
        void UnsubscribeOnOpen(Action<IUIForm> onOpen);

        /// <summary>
        /// 订阅UI关闭事件
        /// </summary>
        /// <param name="onClose"></param>
        IUnRegister SubscribeOnClose(Action<IUIForm> onClose);

        /// <summary>
        /// 取消订阅UI关闭事件
        /// </summary>
        /// <param name="onClose"></param>
        void UnsubscribeOnClose(Action<IUIForm> onClose);
    }
}