/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-15 11:10:46
 * @LastEditors: fuck czk pkq3344520@gmail.com
 * @LastEditTime: 2023-09-26 20:29:27
 * @Description: 状态不存在异常类
 */

using System;

namespace HFSM
{
    public class StateNotExitException<TState> : Exception
    {
        public StateNotExitException(TState state) : base($"Status does not exist: {state}")
        {

        }
    }
}