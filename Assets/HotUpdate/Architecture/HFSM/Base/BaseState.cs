/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-14 21:44:31
 * @LastEditors: wcc
 * @LastEditTime: 2024-06-20 13:28:35
 * @Description: 状态基类
 */


using UnityEngine;

namespace HFSM
{
    public abstract class BaseState : IState
    {
        public bool Active { get; private set; }
        bool IState.CanExit { get; set; }
        public bool HasExitTime { get; private set; }

        public BaseState(bool hasExitTime)
        {
            HasExitTime = hasExitTime;
        }

        void IState.Initialize()
        {
            OnInit();
        }

        void IState.Enter()
        {
            var self = this as IState;
            self.CanExit = self.HasExitTime ? false : true;
            Active = true;
            OnEnter();
        }

        void IState.Excute()
        {
            OnExcute();
        }

        void IState.Exit()
        {
            OnExit();
            Active = false;
            var self = this as IState;
            self.CanExit = self.HasExitTime ? false : true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// 进入状态
        /// </summary>
        protected virtual void OnEnter() { }

        /// <summary>
        /// 轮询
        /// </summary>
        protected virtual void OnExcute() { }

        /// <summary>
        /// 退出状态
        /// </summary>
        protected virtual void OnExit() { }

        /// <summary>
        /// 准备好退出
        /// </summary>
        protected void ReadyToExit()
        {
            (this as IState).CanExit = true;
        }
    }
}