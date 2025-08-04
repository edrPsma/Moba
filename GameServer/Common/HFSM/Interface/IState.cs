/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-14 21:31:17
 * @LastEditors: wcc
 * @LastEditTime: 2024-06-20 13:26:03
 * @Description: 状态接口
 */

namespace HFSM
{
    public interface IState
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        /// <value></value>
        bool Active { get; }

        /// <summary>
        /// 能否退出
        /// </summary>
        /// <value></value>
        bool CanExit { get; set; }

        /// <summary>
        /// 是否有退出时间
        /// </summary>
        /// <value></value>
        bool HasExitTime { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 进入状态
        /// </summary>
        void Enter();

        /// <summary>
        /// 轮询
        /// </summary>
        void Excute();

        /// <summary>
        /// 退出状态
        /// </summary>
        void Exit();
    }
}