using System;
using HFSM;

namespace GameServer.Controller
{
    public class BasePvpState : BaseState
    {
        public BasePvpState(bool hasExitTime = true) : base(hasExitTime) { }

    }

    public enum EPvpState
    {
        /// <summary>
        /// 确认
        /// </summary>
        Confirm,

        /// <summary>
        /// 选择英雄
        /// </summary>
        SelectHero,

        /// <summary>
        /// 加载
        /// </summary>
        Loading,

        /// <summary>
        /// 战斗
        /// </summary>
        Fight,

        /// <summary>
        /// 结束
        /// </summary>
        End
    }
}