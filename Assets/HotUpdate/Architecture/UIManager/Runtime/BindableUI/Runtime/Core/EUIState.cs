using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public enum EUIState : sbyte
    {
        None = 0,

        /// <summary>
        /// 加载数据
        /// </summary>
        LoadData,

        /// <summary>
        /// 加载资源
        /// </summary>
        LoadingAsset,

        /// <summary>
        /// 对应Monobehavior中的Start函数
        /// </summary>
        Start,

        /// <summary>
        /// 准备打开
        /// </summary>
        ReadyToOpen,

        /// <summary>
        /// 打开
        /// </summary>
        Open,

        /// <summary>
        /// 准备关闭
        /// </summary>
        ReadyToClose,

        /// <summary>
        /// 关闭
        /// </summary>
        Close,

        /// <summary>
        /// 销毁
        /// </summary>
        Destory,
    }
}