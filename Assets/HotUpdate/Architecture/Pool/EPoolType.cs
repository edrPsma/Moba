using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    /// <summary>
    /// 对象池类型
    /// </summary>
    public enum EPoolType
    {
        /// <summary>
        /// 可扩展
        /// </summary>
        Scalable,

        /// <summary>
        /// 固定容量
        /// </summary>
        Fixed
    }
}