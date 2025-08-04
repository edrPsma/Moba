/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-14 21:36:45
 * @LastEditors: wcc
 * @LastEditTime: 2024-07-22 10:12:05
 * @Description: 状态机接口
 */


using System;

namespace HFSM
{
    public interface IFSM<TState> : IState, IFSM
    {
        /// <summary>
        /// 正在执行的状态
        /// </summary>
        /// <value></value>
        IState RunningState { get; }

        /// <summary>
        /// 当前状态
        /// </summary>
        /// <value></value>
        TState CurState { get; }

        /// <summary>
        /// 初始状态
        /// </summary>
        /// <value></value>
        TState InitialState { get; set; }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="stateType">状态类型</param>
        /// <param name="state">状态实例</param>
        void AddState(TState stateType, IState state);

        /// <summary>
        /// 添加任意状态切换
        /// </summary>
        /// <param name="to">目标状态</param>
        /// <param name="condition">状态切换条件</param>
        /// <param name="priority">优先级</param>
        /// <param name="immediately">是否立即切换</param>
        void AddAnyTransition(TState to, Func<bool> condition, int priority, bool immediately);

        /// <summary>
        /// 添加任意状态切换
        /// </summary>
        /// <param name="to">目标状态</param>
        /// <param name="condition">状态切换条件</param>
        /// <param name="immediately">是否立即切换</param>
        void AddAnyTransition(TState to, Func<bool> condition, bool immediately);

        /// <summary>
        /// 添加任意状态切换
        /// </summary>
        /// <param name="to">目标状态</param>
        /// <param name="condition">状态切换条件</param>
        void AddAnyTransition(TState to, Func<bool> condition);

        /// <summary>
        /// 添加状态切换
        /// </summary>
        /// <param name="from">起始状态</param>
        /// <param name="to">目标状态</param>
        /// <param name="condition">状态切换条件</param>
        /// <param name="priority">优先级</param>
        /// <param name="immediately">是否立即切换</param>
        void AddTransition(TState from, TState to, Func<bool> condition, int priority, bool immediately);

        /// <summary>
        /// 添加状态切换
        /// </summary>
        /// <param name="from">起始状态</param>
        /// <param name="to">目标状态</param>
        /// <param name="condition">状态切换条件</param>
        /// <param name="immediately">是否立即切换</param>
        void AddTransition(TState from, TState to, Func<bool> condition, bool immediately);

        /// <summary>
        /// 添加状态切换(不立即切换)
        /// </summary>
        /// <param name="from">起始状态</param>
        /// <param name="to">目标状态</param>
        /// <param name="condition">状态切换条件</param>
        void AddTransition(TState from, TState to, Func<bool> condition);

        /// <summary>
        /// 添加状态切换(没有切换条件)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        void AddTransition(TState from, TState to);

        /// <summary>
        /// 立即切换状态
        /// </summary>
        /// <param name="to">目标状态</param>
        void TransitionImmediately(TState to);

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <param name="key">数据键值</param>
        /// <typeparam name="T">目标值</typeparam>
        /// <returns></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">数据键值</param>
        /// <param name="value">目标值</param>
        /// <returns></returns>
        bool GetValue<T>(string key, out T value);

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <param name="key">数据键值</param>
        /// <returns></returns>
        object GetValue(string key);

        /// <summary>
        /// 设置黑板数据
        /// </summary>
        /// <param name="key">数据键值</param>
        /// <param name="value">目标值</param>
        void SetValue(string key, object value);
    }

    public interface IFSM { }
}