using System;
using System.Collections.Generic;
using System.Linq;

public static partial class Extension
{
    /// <summary>
    /// 获取元素在集合中的位置
    /// </summary>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int IndexOf<T>(this IEnumerable<T> list, T item)
    {
        int i = 0;
        foreach (var temp in list)
        {
            if (Equals(temp, item))
            {
                return i;
            }
            ++i;
        }

        return -1;
    }

    /// <summary>
    /// 集合中是否有对应索引
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool HasIndex<T>(this IEnumerable<T> list, int index)
    {
        return list != null && index >= 0 && index < list.Count();
    }

    /// <summary>
    /// 尝试获取集合中元素
    /// </summary>
    /// <param name="list"></param>
    /// <param name="match"></param>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool TryFindValue<T>(this IEnumerable<T> list, Predicate<T> match, out T result)
    {
        foreach (var item in list)
        {
            if (match(item))
            {
                result = item;
                return true;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// 对数组进行随机排序
    /// </summary>
    /// <param name="array"></param>
    /// <typeparam name="T"></typeparam>
    public static void RandomSort<T>(this T[] array)
    {
        var len = array.Length;
        for (var i = 0; i < len; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, len);
            var temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }
    }

    /// <summary>
    /// 对列表进行随机排序
    /// </summary>
    /// <param name="array"></param>
    /// <typeparam name="T"></typeparam>
    public static void RandomSort<T>(this List<T> array)
    {
        var len = array.Count;
        for (var i = 0; i < len; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, len);
            var temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }
    }

    /// <summary>
    /// 设置字典值，若key存在则修改，若key不存在则添加
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    public static void SetValue<K, V>(this Dictionary<K, V> dic, K key, V value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }
    }

    /// <summary>
    /// 取出字典元素，若key不存在，则返回默认值
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    public static V GetValue<K, V>(this Dictionary<K, V> dic, K key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// 控制台输出集合类数据
    /// </summary>
    /// <typeparam name="T">集合类数据类型</typeparam>
    public static void Log<T>(this IEnumerable<T> source)
    {
        Log(source, null);
    }

    /// <summary>
    /// 控制台输出集合类数据
    /// </summary>
    /// <typeparam name="T">集合类数据类型</typeparam>
    /// <param name="prefix">前缀</param>
    public static void Log<T>(this IEnumerable<T> source, string prefix)
    {
        string str;
        if (source == null) str = "Null";
        else if (source.Count() == 0) str = "Empty";
        else str = string.Join(", ", source);
        if (!string.IsNullOrEmpty(prefix)) str = prefix + str;
        UnityEngine.Debug.Log(str);
    }

    /// <summary>
    /// 控制台输出集合类经过筛选器返回的数据
    /// </summary>
    /// <typeparam name="TSource">集合类数据类型</typeparam>
    /// <typeparam name="TResult">筛选器返回的数据</typeparam>
    public static void Log<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selecter)
    {
        Log(source, null, selecter);
    }

    /// <summary>
    /// 控制台输出集合类经过筛选器返回的数据
    /// </summary>
    /// <typeparam name="TSource">集合类数据类型</typeparam>
    /// <typeparam name="TResult">筛选器返回的数据</typeparam>
    /// <param name="prefix">前缀</param>
    public static void Log<TSource, TResult>(this IEnumerable<TSource> source, string prefix, Func<TSource, TResult> selecter)
    {
        string str;
        if (source == null) str = "Null";
        else if (source.Count() == 0) str = "Empty";
        else str = string.Join(", ", source.Select(selecter));
        if (!string.IsNullOrEmpty(prefix)) str = prefix + str;
        UnityEngine.Debug.Log(str);
    }

    /// <summary>
    /// 从只读列表中查找值
    /// </summary>
    /// <typeparam name="T">只读列表中值的类型</typeparam>
    /// <param name="source">只读列表</param>
    /// <param name="match">匹配表达式</param>
    public static T Find<T>(this IReadOnlyList<T> source, Func<T, bool> match)
    {
        if (source == null || match == null) return default;
        int count = source.Count;
        for (int i = 0; i < count; i++)
        {
            var value = source[i];
            if (match(value)) return value;
        }
        return default;
    }

    /// <summary>
    /// 从只读列表中查找值的索引
    /// </summary>
    /// <typeparam name="T">只读列表中值的类型</typeparam>
    /// <param name="source">只读列表</param>
    /// <param name="match">匹配表达式</param>
    /// <returns>若找到则返回对应的索引，否则返回-1</returns>
    public static int FindIndex<T>(this IReadOnlyList<T> source, Func<T, bool> match)
    {
        if (source == null || match == null) return -1;
        int count = source.Count;
        for (int i = 0; i < count; i++)
        {
            var value = source[i];
            if (match(value)) return i;
        }
        return -1;
    }
}
