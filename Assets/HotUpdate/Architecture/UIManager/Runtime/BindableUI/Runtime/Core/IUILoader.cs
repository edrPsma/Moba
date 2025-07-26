using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public interface IUILoader
    {
        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="location">资源位置</param>
        /// <param name="loadOver">加载成功回调</param>
        void Load(string location, Action<GameObject, object> loadOver);

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="location">资源位置</param>
        /// <param name="loadOver">加载成功回调</param>
        void LoadAsync(string location, Action<GameObject, object> loadOver);

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="userData">用户数据</param>
        void UnLoad(GameObject gameObject, object userData);

    }
}