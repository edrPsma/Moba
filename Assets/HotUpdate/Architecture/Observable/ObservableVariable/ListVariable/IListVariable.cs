using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IListVariable<T> : IReadOnlyListVariable<T>
    {
        /// <summary>
        /// 修改列表
        /// </summary>
        /// <param name="action"></param>
        void Modifly(Action<List<T>> action);
    }
}