using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public partial class Utility
{
    public static class Reflection
    {
        /// <summary>
        /// 创建设置属性值的委托，这是一个耗时操作
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <typeparam name="V">目标值类型</typeparam>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public static Action<T, V> CreateSetPropertyFunc<T, V>(string name)
        {
            Type type = typeof(T);
            ParameterExpression targetExp = Expression.Parameter(type, "target");
            ParameterExpression valueExp = Expression.Parameter(typeof(V), "value");
            MemberExpression fieldExp = Expression.PropertyOrField(targetExp, name);
            BinaryExpression assignExp = Expression.Assign(fieldExp, valueExp);

            return Expression.Lambda<Action<T, V>>(assignExp, targetExp, valueExp).Compile();
        }

        /// <summary>
        /// 创建获取属性值的委托，这是一个耗时操作
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <typeparam name="V">目标值类型</typeparam>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public static Func<T, V> CreateGetPropertyFunc<T, V>(string name)
        {
            Type type = typeof(T);
            ParameterExpression targetExp = Expression.Parameter(type, "target");
            MemberExpression fieldExp = Expression.PropertyOrField(targetExp, name);

            return Expression.Lambda<Func<T, V>>(fieldExp, targetExp).Compile();
        }
    }

}
