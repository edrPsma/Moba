/*
 * @Author: edR && pkq3344520@gmail.com
 * @Date: 2023-04-15 10:55:10
 * @LastEditors: wcc
 * @LastEditTime: 2024-06-20 12:08:17
 * @Description: 状态重复添加异常类
 */

using System;

namespace HFSM
{
    public class DuplicateStateException<TState> : Exception
    {
        public DuplicateStateException(TState state) : base($"Status added repeatedly :{state}")
        {

        }
    }
}