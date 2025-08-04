/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-14 21:38:52
 * @LastEditors: wcc
 * @LastEditTime: 2024-06-20 13:26:42
 * @Description: 状态转换接口
 */


using System;

namespace HFSM
{
    public interface ITransition<TState>
    {
        /// <summary>
        /// 起始状态
        /// </summary>
        /// <value></value>
        TState From { get; }

        /// <summary>
        /// 目标状态
        /// </summary>
        /// <value></value>
        TState To { get; }

        /// <summary>
        /// 状态切换条件
        /// </summary>
        /// <value></value>
        Func<bool> Condition { get; }

        /// <summary>
        /// 优先级
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 是否立即切换
        /// </summary>
        /// <value></value>
        bool Immediately { get; }
    }
}