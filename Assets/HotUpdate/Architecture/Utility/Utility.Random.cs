using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utility
{
    public static class Random
    {
        private static System.Random _random = new System.Random((int)System.DateTime.UtcNow.Ticks);

        public static int Seed { get; set; }

        static Random()
        {
            SetSeed((int)System.DateTime.UtcNow.Ticks);
        }

        /// <summary>
        /// 设置随机数种子。
        /// </summary>
        /// <param name="seed">随机数种子。</param>
        public static void SetSeed(int seed)
        {
            Seed = seed;
            _random = new System.Random(seed);
        }

        /// <summary>
        /// 返回非负随机数。
        /// </summary>
        /// <returns>大于等于零且小于 System.Int32.MaxValue 的 32 位带符号整数。</returns>
        public static int GetRandom()
        {
            return _random.Next();
        }

        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int GetRandom(int maxValue)
        {
            return _random.Next(maxValue);
        }

        /// <summary>
        /// 返回一个指定范围内的随机数,不包含最大值
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int GetRandom(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 返回一个指定范围内的随机数
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float GetRandom(float minValue, float maxValue)
        {
            return (float)GetRandom01() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数。
        /// </summary>
        /// <returns></returns>
        public static double GetRandom01()
        {
            return _random.NextDouble();
        }

        #region weighted random
        /// <summary>
        /// 权值数据
        /// </summary>
        public struct WeightedItem<T>
        {
            /// <summary>
            /// 索引
            /// </summary>
            public int Index;
            /// <summary>
            /// 用户数据
            /// </summary>
            public T Value;
            /// <summary>
            /// 权重
            /// </summary>
            public float Weight;
        }

        /// <summary>
        /// 加权随机
        /// </summary>
        /// <param name="weightedItems">权值数据</param>
        /// <typeparam name="T">用户数据类型</typeparam>
        /// <returns></returns>
        public static WeightedItem<T> WeightedRandom<T>(WeightedItem<T>[] weightedItems)
        {
            float weight = 0;
            Array.ForEach(weightedItems, item => weight += item.Weight);
            int index = -1;
            float cur = UnityEngine.Random.Range(0, Mathf.Max(0, weight));
            float sum = 0;
            if (weightedItems != null && weightedItems.Length > 0)
            {
                index = 0;
                for (; index < weightedItems.Length; index++)
                {
                    sum += weightedItems[index].Weight;
                    if (sum > cur) break;
                }
            }
            if (index >= weightedItems.Length)
            {
                index = weightedItems.Length - 1;
            }

            return weightedItems[index];
        }

        #endregion

        #region graphics
        /// <summary>
        /// 矩形内随机一点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Vector2 RandomInRect(Vector2 center, float width, float height)
        {
            Vector2 pos = center;
            pos.x = center.x - width * 0.5f + GetRandom(0, width);
            pos.y = center.y - height * 0.5f + GetRandom(0, height);
            return pos;
        }

        /// <summary>
        /// 圆形内随机一点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector2 RandomInCircle(Vector2 center, float radius)
        {
            float x = GetRandom(-1f, 1f);
            float y = GetRandom(-1f, 1f);
            float dis = (float)GetRandom01() * radius;

            Vector2 dir = new Vector2(x, y).normalized;
            return center + dir * dis;
        }

        /// <summary>
        /// 圆周上随机一点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector2 RandomCircleSurface(Vector2 center, float radius)
        {
            float x = GetRandom(-1f, 1f);
            float y = GetRandom(-1f, 1f);

            Vector2 dir = new Vector2(x, y).normalized;
            return center + dir * radius;
        }

        /// <summary>
        /// 三角形内随机一点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Vector2 RandomInTriangle(Vector2 a, Vector2 b, Vector2 c)
        {
            float r1 = GetRandom(0, 1f);
            float r2 = GetRandom(0, 1f);
            return (1 - Mathf.Sqrt(r1)) * a + Mathf.Sqrt(r1) * (1 - r2) * b + Mathf.Sqrt(r1) * r2 * c;
        }

        /// <summary>
        /// 球内随机一点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector3 RandomInSphere(Vector3 center, float radius)
        {
            float x = GetRandom(-1, 1f);
            float y = GetRandom(-1, 1f);
            float z = GetRandom(-1, 1f);
            float dis = GetRandom(0, 1f) * radius;

            Vector3 dir = new Vector3(x, y, z).normalized;
            return center + dir * dis;
        }

        /// <summary>
        /// 球体表面随机一点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector3 RandomSphereSurface(Vector3 center, float radius)
        {
            float x = GetRandom(-1, 1f);
            float y = GetRandom(-1, 1f);
            float z = GetRandom(-1, 1f);

            Vector3 dir = new Vector3(x, y, z).normalized;
            return center + dir * radius;
        }

        /// <summary>
        /// 立方体内随机一点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="lenght">x为长</param>
        /// <param name="width">z为宽</param>
        /// <param name="height">y为高</param>
        /// <returns></returns>
        public static Vector3 RandomInBox(Vector3 center, float lenght, float width, float height)
        {
            Vector3 pos = center;
            pos.x = center.x - lenght * 0.5f + GetRandom(0, lenght);
            pos.y = center.y - height * 0.5f + GetRandom(0, height);
            pos.z = center.y - width * 0.5f + GetRandom(0, width);

            return pos;
        }

        /// <summary>
        /// 随机一个角度（0-360°）
        /// </summary>
        /// <returns>随机角度的弧度值</returns>
        public static float RandomAngle()
        {
            return GetRandom(0, 2 * Mathf.PI);
        }

        /// <summary>
        /// 根据角度范围随机一个角度
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomAngle(float min, float max)
        {
            return GetRandom(min * Mathf.Deg2Rad, max * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 随机一个单位向量
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomUnitVector()
        {
            float x = GetRandom(-1, 1f);
            float y = GetRandom(-1, 1f);
            float z = GetRandom(-1, 1f);

            return new Vector3(x, y, z).normalized;
        }
        #endregion
    }
}
