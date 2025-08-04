/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-14 21:55:12
 * @LastEditors: wcc
 * @LastEditTime: 2024-09-05 10:06:58
 * @Description: 状态转换基类
 */


using System;

namespace HFSM
{
    public class BaseTransition<TState> : ITransition<TState>
    {
        /// <summary>
        /// 起始状态
        /// </summary>
        /// <value></value>
        public TState From { get; private set; }

        /// <summary>
        /// 目标状态
        /// </summary>
        /// <value></value>
        public TState To { get; private set; }

        /// <summary>
        /// 状态切换条件
        /// </summary>
        /// <value></value>
        public Func<bool> Condition { get; private set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority{get;private set;}

        /// <summary>
        /// 是否立即切换
        /// </summary>
        /// <value></value>
        public bool Immediately { get; private set; }

        public BaseTransition(TState from, TState to, Func<bool> condition,int priority ,bool immediately)
        {
            From = from;
            To = to;
            Condition = condition;
            Priority=priority;
            Immediately = immediately;
        }
    }
}