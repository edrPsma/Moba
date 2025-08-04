using System;
using GameServer.Common;
using HFSM;

namespace GameServer.Controller
{
    public class BasePvpState : BaseState
    {
        public PvpFSM FSM { get; }
        public BasePvpState(PvpFSM fsm, bool hasExitTime = true) : base(hasExitTime)
        {
            FSM = fsm;
        }

        protected override void OnInit()
        {
            base.OnInit();
            Builder.Inject(this);
        }
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