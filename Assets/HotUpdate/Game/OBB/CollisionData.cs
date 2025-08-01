using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointNumber;

namespace OBB
{
    [Serializable]
    public struct BoxColliderData
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public FixIntVector3[] Vertexts;

        /// <summary>
        /// 轴向 x,y,z
        /// </summary>
        public FixIntVector3[] Axes;

        /// <summary>
        /// 中心点
        /// </summary>
        public FixIntVector3 Center;

        /// <summary>
        /// 大小
        /// </summary>
        public FixIntVector3 Size;
    }

    [Serializable]
    public struct SphereColliderData
    {
        /// <summary>
        /// 中心点
        /// </summary>
        public FixIntVector3 Center;

        /// <summary>
        /// 半径
        /// </summary>
        public FixInt Radius;
    }

    [Serializable]
    public struct CapsuleColliderData
    {
        /// <summary>
        /// 中心点
        /// </summary>
        public FixIntVector3 Center;

        /// <summary>
        /// 半径
        /// </summary>
        public FixInt Radius;

        /// <summary>
        /// 高度
        /// </summary>
        public FixInt Height;

        /// <summary>
        /// 轴向 x,y,z
        /// </summary>
        public FixIntVector3 Direction;

        public FixIntVector3 Top => Center + Direction * Height / 2f;
        public FixIntVector3 Down => Center - Direction * Height / 2f;
    }

    [Serializable]
    public struct CollisionData
    {
        /// <summary>
        /// 是否发生碰撞
        /// </summary>
        public bool IsColliding;

        /// <summary>
        /// 碰撞法线
        /// </summary>
        public FixIntVector3 Normal;

        /// <summary>
        /// 穿透深度
        /// </summary>
        public FixInt Penetration;
    }
}