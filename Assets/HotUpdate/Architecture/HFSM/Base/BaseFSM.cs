/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-14 22:00:29
 * @LastEditors: wcc
 * @LastEditTime: 2024-09-05 10:12:03
 * @Description: 状态机基类
 */


using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace HFSM
{
    public abstract class BaseFSM<TState> : IFSM<TState>
    {
        public bool HasExitTime { get; private set; }
        public TState InitialState { get; set; }
        bool IState.CanExit { get; set; }
        public IState RunningState { get; private set; }
        [ShowInInspector, ReadOnly] public TState CurState { get; private set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        /// <value></value>
        [ShowInInspector, ReadOnly] public bool Active { get; private set; }

        [ShowInInspector] Dictionary<TState, IState> _stateDic;
        [ShowInInspector] Dictionary<TState, List<ITransition<TState>>> _transitionDic;
        [ShowInInspector] List<ITransition<TState>> _anyStatetransitionList;
        [ShowInInspector] Dictionary<string, object> _blackboard;

        public BaseFSM(bool hasExitTime, TState initialState)
        {
            InitialState = initialState;
            HasExitTime = hasExitTime;

            _transitionDic = new Dictionary<TState, List<ITransition<TState>>>();
            _stateDic = new Dictionary<TState, IState>();
            _anyStatetransitionList = new List<ITransition<TState>>();
            _blackboard = new Dictionary<string, object>();
        }

        public void AddState(TState stateType, IState state)
        {
            if (_stateDic.ContainsKey(stateType))
            {
                throw new DuplicateStateException<TState>(stateType);
            }

            state.Initialize();
            _stateDic.Add(stateType, state);
        }

        public void AddTransition(TState from, TState to, Func<bool> condition, int priority, bool immediately)
        {
            BaseTransition<TState> t = new BaseTransition<TState>(from, to, condition, priority, immediately);

            if (from == null)
            {
                _anyStatetransitionList.Add(t);
                return;
            }

            if (!_transitionDic.ContainsKey(from))
            {
                _transitionDic.Add(from, new List<ITransition<TState>>());
            }
            _transitionDic[from].Add(t);
        }

        public void AddTransition(TState from, TState to, Func<bool> condition, bool immediately)
        {
            AddTransition(from, to, condition, 1, immediately);
        }

        public void AddTransition(TState from, TState to, Func<bool> condition)
        {
            AddTransition(from, to, condition, 1, false);
        }

        public void AddTransition(TState from, TState to)
        {
            AddTransition(from, to, FunTrue, 1, false);
        }

        bool FunTrue() => true;

        public void AddAnyTransition(TState to, Func<bool> condition, int priority, bool immediately)
        {
            BaseTransition<TState> t = new BaseTransition<TState>(to, to, condition, priority, immediately);
            _anyStatetransitionList.Add(t);
        }

        public void AddAnyTransition(TState to, Func<bool> condition, bool immediately)
        {
            AddAnyTransition(to, condition, 1, immediately);
        }

        public void AddAnyTransition(TState to, Func<bool> condition)
        {
            AddAnyTransition(to, condition, 1, false);
        }

        public void Initialize()
        {
            OnInitialize();
            SortTransition();
        }

        public void Enter()
        {
            TState initialState = (this as IFSM<TState>).InitialState;
            RunningState = GetState(initialState);
            CurState = initialState;
            var self = this as IState;
            self.CanExit = self.HasExitTime ? false : true;
            OnEnter();
            RunningState.Enter();
            Active = true;
        }

        public void Excute()
        {
            if (!Active) return;

            TransitionDetect();
            RunningState?.Excute();
            OnExcute();
        }

        public void Exit()
        {
            OnExit();
            var self = this as IState;
            self.CanExit = self.HasExitTime ? false : true;
            RunningState?.Exit();
            RunningState = null;
            Active = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// 状态进入
        /// </summary>
        protected virtual void OnEnter() { }

        /// <summary>
        /// 状态机轮询
        /// </summary>
        protected virtual void OnExcute() { }

        /// <summary>
        /// 状态退出后执行
        /// </summary>
        protected virtual void OnExit() { }

        /// <summary>
        /// 状态切换前执行
        /// </summary>
        /// <param name="from">起始状态</param>
        /// <param name="to">目标状态</param>
        /// <param name="fromExist">起始状态是否存在</param>
        protected virtual void OnTransitionBefore(TState from, TState to, bool fromExist) { }

        /// <summary>
        /// 状态切换后执行
        /// </summary>
        /// <param name="from">起始状态</param>
        /// <param name="to">起始状态</param>
        /// <param name="fromExist">起始状态是否存在</param>
        protected virtual void OnTransitionAfter(TState from, TState to, bool fromExist) { }

        void SortTransition()
        {
            _anyStatetransitionList.Sort(SortTransition);
            foreach (var item in _transitionDic)
            {
                item.Value.Sort(SortTransition);
            }
        }

        int SortTransition(ITransition<TState> transition1, ITransition<TState> transition2)
        {
            if (transition1.Priority > transition2.Priority)
            {
                return -1;
            }
            else if (transition1.Priority < transition2.Priority)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // 状态切换检测
        void TransitionDetect()
        {
            if (RunningState == null) return;
            //检测任意状态切换
            foreach (var item in _anyStatetransitionList)
            {
                if (!item.Immediately && !RunningState.CanExit) continue;
                //if (item.To.ToString() == CurStateName) continue; //切换至自身
                if (item.Condition != null && item.Condition.Invoke())
                {
                    TransitionImmediately(item.To);
                    return;
                }
            }

            //检测普通状态切换
            if (CurState == null) return;
            if (!_transitionDic.ContainsKey(CurState)) return;
            List<ITransition<TState>> transitionList = _transitionDic[CurState];
            foreach (var item in transitionList)
            {
                if (!item.Immediately && !RunningState.CanExit) continue;
                if (item.Condition != null && item.Condition.Invoke())
                {
                    TransitionImmediately(item.To);
                    return;
                }
            }
        }

        // 获取状态
        IState GetState(TState state)
        {
            if (state == null) return null;

            if (!_stateDic.ContainsKey(state))
            {
                throw new StateNotExitException<TState>(state);
            }
            return _stateDic[state];
        }

        /// <summary>
        /// 立即切换状态
        /// </summary>
        /// <param name="to">目标状态</param>
        [Button]
        public void TransitionImmediately(TState to)
        {
            TState from = CurState;
            bool fromExist = RunningState != null;
            OnTransitionBefore(from, to, fromExist);
            RunningState?.Exit();
            CurState = to;
            RunningState = GetState(CurState);
            RunningState?.Enter();
            OnTransitionAfter(from, to, fromExist);
        }

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <param name="key">数据键值</param>
        /// <typeparam name="T">目标值</typeparam>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            GetValue(key, out T value);
            return value;
        }

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">数据键值</param>
        /// <param name="value">目标值</param>
        /// <returns></returns>
        public bool GetValue<T>(string key, out T value)
        {
            if (_blackboard.TryGetValue(key, out object val))
            {
                value = (T)val;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        /// <param name="key">数据键值</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            if (_blackboard.TryGetValue(key, out object value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置黑板数据
        /// </summary>
        /// <param name="key">数据键值</param>
        /// <param name="value">目标值</param>
        public void SetValue(string key, object value)
        {
            if (_blackboard.ContainsKey(key))
            {
                _blackboard[key] = value;
            }
            else
            {
                _blackboard.Add(key, value);
            }
        }
    }
}