using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IUnRegister
    {
        /// <summary>
        /// 取消注册
        /// </summary>
        void UnRegister();
    }
}