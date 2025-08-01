using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using UnityEngine;

namespace OBB
{
    public class OBBCollisionTools
    {
        /// <summary>
        /// SAT分离轴碰撞检测之OBB检测
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// </summary>
        public static bool CollisionDetect(BoxColliderData data1, BoxColliderData data2, out CollisionData collisionData)
        {
            //求与两个OBB包围盒之间两两坐标轴垂直的法线轴 共9个
            int len1 = data1.Axes.Length;
            int len2 = data2.Axes.Length;
            FixIntVector3[] Axes = new FixIntVector3[len1 + len2 + len1 * len2];
            int k = 0;
            int initJ = len2;
            for (int i = 0; i < len1; i++)
            {
                Axes[k++] = data1.Axes[i];
                for (int j = 0; j < len2; j++)
                {
                    if (initJ > 0)
                    {
                        initJ--;
                        Axes[k++] = data2.Axes[j];
                    }
                    Axes[k++] = FixIntVector3.Cross(data1.Axes[i], data2.Axes[j]);
                }
            }


            FixInt minOverlap = FixInt.MaxValue;
            FixIntVector3 minNormal = FixIntVector3.zero;

            for (int i = 0, len = Axes.Length; i < len; i++)
            {
                if (!NotInteractiveOBB(data1.Vertexts, data2.Vertexts, Axes[i], out FixInt overlap))
                {
                    // 更新最小重叠
                    if (overlap < minOverlap)
                    {
                        minOverlap = overlap;
                        minNormal = Axes[i];
                    }
                }
                else
                {
                    collisionData = new CollisionData
                    {
                        IsColliding = false,
                        Normal = FixIntVector3.zero,
                        Penetration = 0

                    };

                    //有一个不相交就退出
                    return false;
                }
            }

            // 3. 确定碰撞法线方向（从A指向B）
            FixIntVector3 centerDir = (data1.Center - data2.Center).normalized;
            if (FixIntVector3.Dot(minNormal, centerDir) < 0)
            {
                minNormal = -minNormal;
            }

            collisionData = new CollisionData
            {
                IsColliding = true,
                Normal = minNormal,
                Penetration = minOverlap
            };

            return true;
        }

        /// <summary>
        /// 球与OBB检测
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="collisionData"></param>
        /// <returns></returns>
        public static bool CollisionDetect(SphereColliderData data1, BoxColliderData data2, out CollisionData collisionData)
        {
            //求最近点
            FixIntVector3 nearP = GetClosestPointOBB(data1.Center, data2);
            //与AABB检测原理相同
            FixIntVector3 dir = data1.Center - nearP;
            FixInt distance = dir.magnitude;
            FixInt radius = data1.Radius;

            bool IsColliding = distance <= radius;

            if (IsColliding)
            {
                collisionData = new CollisionData
                {
                    IsColliding = false,
                    Normal = dir.normalized,
                    Penetration = data1.Radius - distance
                };
                return true;
            }
            else
            {
                collisionData = new CollisionData
                {
                    IsColliding = false,
                    Normal = FixIntVector3.zero,
                    Penetration = 0
                };
                return false;
            }
        }

        /// <summary>
        /// OBB与球检测
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="collisionData"></param>
        /// <returns></returns>
        public static bool CollisionDetect(BoxColliderData data1, SphereColliderData data2, out CollisionData collisionData)
        {
            bool isColliding = CollisionDetect(data2, data1, out collisionData);

            collisionData.Normal *= -1;

            return isColliding;
        }

        public static bool CollisionCapsule(CapsuleColliderData data1, CapsuleColliderData data2)
        {
            // 求两条线段的最短距离
            FixInt distance = GetClosestDistanceBetweenLinesSqr(data1.Top, data2.Down, data2.Top, data2.Down);

            //求两个球半径和
            FixInt totalRadius = FixIntMath.Pow(data1.Radius + data2.Radius, 2);
            //距离小于等于半径和则碰撞
            if (distance <= totalRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CollisionDetect(SphereColliderData data1, SphereColliderData data2, out CollisionData collisionData)
        {
            FixIntVector3 dir = data1.Center - data2.Center;
            FixInt len = dir.magnitude;
            bool isColliding = data1.Radius + data2.Radius > len;
            if (isColliding)
            {
                collisionData = new CollisionData
                {
                    IsColliding = true,
                    Normal = dir,
                    Penetration = data1.Radius + data2.Radius - len
                };
            }
            else
            {
                collisionData = new CollisionData
                {
                    IsColliding = true,
                    Normal = FixIntVector3.zero,
                    Penetration = 0
                };
            }

            return isColliding;
        }

        public static bool CollisionDetect(BoxColliderData data1, CapsuleColliderData data2, out CollisionData collisionData)
        {
            bool isColliding = CollisionDetect(data2, data1, out collisionData);

            collisionData.Normal *= -1;

            return isColliding;
        }

        public static bool CollisionDetect(CapsuleColliderData data1, BoxColliderData data2, out CollisionData collisionData)
        {
            FixIntVector3 closest = GetClosestPointOnLineSegment(data1.Top, data1.Down, data2.Center);

            //求最近点
            FixIntVector3 nearP = GetClosestPointOBB(closest, data2);
            //与AABB检测原理相同

            FixIntVector3 dir = closest - nearP;
            FixInt distance = dir.magnitude;
            FixInt radius = data1.Radius;

            bool IsColliding = distance <= radius;

            if (IsColliding)
            {
                collisionData = new CollisionData
                {
                    IsColliding = false,
                    Normal = dir.normalized,
                    Penetration = data1.Radius - distance
                };
                return true;
            }
            else
            {
                collisionData = new CollisionData
                {
                    IsColliding = false,
                    Normal = FixIntVector3.zero,
                    Penetration = 0
                };
                return false;
            }
        }

        public static bool CollisionDetect(SphereColliderData data1, CapsuleColliderData data2, out CollisionData collisionData)
        {
            bool isColliding = CollisionDetect(data2, data1, out collisionData);

            collisionData.Normal *= -1;

            return isColliding;
        }

        public static bool CollisionDetect(CapsuleColliderData data1, SphereColliderData data2, out CollisionData collisionData)
        {
            FixIntVector3 closest = GetClosestPointOnLineSegment(data1.Top, data1.Down, data2.Center);

            //求两个球半径和
            FixInt totalRadius = data1.Radius + data2.Radius;
            //球两个球心之间的距离
            FixIntVector3 dir = closest - data2.Center;
            FixInt distance = dir.magnitude;
            //距离小于等于半径和则碰撞
            if (distance <= totalRadius)
            {
                collisionData = new CollisionData
                {
                    IsColliding = false,
                    Normal = dir.normalized,
                    Penetration = data1.Radius + data2.Radius - distance
                };
                return true;
            }
            else
            {
                collisionData = new CollisionData
                {
                    IsColliding = false,
                    Normal = FixIntVector3.zero,
                    Penetration = 0
                };
                return false;
            }
        }

        /// <summary>
        /// 获取一点到OBB的最近点
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// </summary>
        /// <returns></returns>
        public static FixIntVector3 GetClosestPointOBB(FixIntVector3 point, BoxColliderData data2)
        {
            FixIntVector3 nearP = data2.Center;
            //求球心与OBB中心的距离向量 从OBB中心指向球心
            FixIntVector3 center1 = point;
            FixIntVector3 center2 = data2.Center;
            FixIntVector3 dist = center1 - center2;

            FixInt[] extents = new FixInt[3] { data2.Size.x / 2f, data2.Size.y / 2f, data2.Size.z / 2f };
            FixIntVector3[] axes = data2.Axes;

            for (int i = 0; i < 3; i++)
            {
                //计算距离向量到OBB坐标轴的投影长度 即距离向量在OBB坐标系中的对应坐标轴的长度
                FixInt distance = FixIntVector3.Dot(dist, axes[i]);
                distance = FixIntMath.Clamp(distance, -extents[i], extents[i]);
                //还原到世界坐标
                nearP.x += distance * axes[i].x;
                nearP.y += distance * axes[i].y;
                nearP.z += distance * axes[i].z;
            }
            return nearP;
        }

        /// <summary>
        /// 点到线段上的最近点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static FixIntVector3 GetClosestPointOnLineSegment(FixIntVector3 start, FixIntVector3 end, FixIntVector3 point)
        {
            FixIntVector3 line = end - start;
            //dot line line 求长度平方
            FixInt ratio = FixIntVector3.Dot(point - start, line) / FixIntVector3.Dot(line, line);
            ratio = FixIntMath.Min(FixIntMath.Max(ratio, 0), 1);

            return start + ratio * line;
        }


        // 获取两条线段的最短距离的平方
        private static FixInt GetClosestDistanceBetweenLinesSqr(FixIntVector3 start1, FixIntVector3 end1, FixIntVector3 start2, FixIntVector3 end2)
        {
            FixIntVector3 line1 = end1 - start1;
            FixIntVector3 line2 = end2 - start2;
            bool isParallel = FixIntVector3.Cross(line1, line2).sqrMagnitude < 0.0001f;

            FixInt dis;

            // 若平行 取四个端点到对应线段的长度的最小值
            if (isParallel)
            {
                FixInt minValue = FixInt.MaxValue;
                minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start1, end1, start2) - start2).sqrMagnitude, minValue);
                minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start1, end1, end2) - end2).sqrMagnitude, minValue);
                minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start2, end2, start1) - start1).sqrMagnitude, minValue);
                minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start2, end2, end1) - end1).sqrMagnitude, minValue);

                dis = minValue;
            }
            else
            {
                FixIntVector3 normal = FixIntVector3.Cross(line1, line2).normalized;

                // 公垂线长度，若公垂线长度约等于0 则认为两个线段共面
                FixInt dis2Line = FixIntMath.Abs(FixIntVector3.Dot(start2 - start1, normal));

                if (dis2Line < 0.001f)
                {
                    bool isLineCross = CheckLineCross(start1, end1, start2, end2);
                    if (isLineCross)
                    {
                        dis = 0;
                    }
                    else
                    {
                        FixInt minValue = FixInt.MaxValue;
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start1, end1, start2) - start2).sqrMagnitude, minValue);
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start1, end1, end2) - end2).sqrMagnitude, minValue);
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start2, end2, start1) - start1).sqrMagnitude, minValue);
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start2, end2, end1) - end1).sqrMagnitude, minValue);

                        dis = minValue;
                    }
                }
                else
                {
                    //计算line2相对line1的方位
                    FixIntVector3 directionStart = start2 - start1;
                    FixInt direction = FixIntVector3.Dot(directionStart, normal) > 0 ? 1 : -1;
                    // 检测线段相交
                    bool isLineCross = CheckLineCross(start1, end1, start2 - normal * (dis2Line * direction),
                        end2 - normal * (dis2Line * direction));

                    // 若相交 则表示公垂线长度为最小距离
                    if (isLineCross)
                    {
                        dis = dis2Line * dis2Line;
                    }
                    else
                    {
                        FixInt minValue = FixInt.MaxValue;
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start1, end1, start2) - start2).sqrMagnitude, minValue);
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start1, end1, end2) - end2).sqrMagnitude, minValue);
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start2, end2, start1) - start1).sqrMagnitude, minValue);
                        minValue = FixIntMath.Min((GetClosestPointOnLineSegment(start2, end2, end1) - end1).sqrMagnitude, minValue);

                        dis = minValue;
                    }
                }

            }

            return dis;
        }


        /// <summary>
        /// 计算OBB顶点数据
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <param name="vertexts"></param>
        public static void CalBoxVertexts(FixIntVector3 pos, FixIntVector3 size, FixIntVector3 scale, FixIntVector3[] axes, FixIntVector3[] vertexts)
        {
            FixInt halfSizeX = size.x * scale.x / 2f;
            FixInt halfSizeY = size.y * scale.y / 2f;
            FixInt halfSizeZ = size.z * scale.z / 2f;

            vertexts[0] = -halfSizeX * axes[0] + halfSizeY * axes[1] + halfSizeZ * axes[2] + pos;
            vertexts[1] = halfSizeX * axes[0] + halfSizeY * axes[1] + halfSizeZ * axes[2] + pos;
            vertexts[2] = halfSizeX * axes[0] + halfSizeY * axes[1] - halfSizeZ * axes[2] + pos;
            vertexts[3] = -halfSizeX * axes[0] + halfSizeY * axes[1] - halfSizeZ * axes[2] + pos;
            vertexts[4] = -halfSizeX * axes[0] - halfSizeY * axes[1] + halfSizeZ * axes[2] + pos;
            vertexts[5] = halfSizeX * axes[0] - halfSizeY * axes[1] + halfSizeZ * axes[2] + pos;
            vertexts[6] = halfSizeX * axes[0] - halfSizeY * axes[1] - halfSizeZ * axes[2] + pos;
            vertexts[7] = -halfSizeX * axes[0] - halfSizeY * axes[1] - halfSizeZ * axes[2] + pos;
        }

        // 计算投影是否不相交
        private static bool NotInteractiveOBB(FixIntVector3[] vertexs1, FixIntVector3[] vertexs2, FixIntVector3 axis, out FixInt overlap)
        {
            //计算OBB包围盒在分离轴上的投影极限值
            FixInt[] limit1 = GetProjectionLimit(vertexs1, axis);
            FixInt[] limit2 = GetProjectionLimit(vertexs2, axis);

            // 计算重叠量
            overlap = FixIntMath.Min(limit1[0], limit2[0]) - FixIntMath.Max(limit1[1], limit2[1]);

            //两个包围盒极限值不相交，则不碰撞
            return limit1[0] > limit2[1] || limit2[0] > limit1[1];
        }

        // 计算顶点投影极限值
        private static FixInt[] GetProjectionLimit(FixIntVector3[] vertexts, FixIntVector3 axis)
        {
            FixInt[] result = new FixInt[2] { FixInt.MaxValue, FixInt.MinValue };
            for (int i = 0, len = vertexts.Length; i < len; i++)
            {
                FixIntVector3 vertext = vertexts[i];
                FixInt dot = FixIntVector3.Dot(vertext, axis);

                result[0] = FixIntMath.Min(dot, result[0]);
                result[1] = FixIntMath.Max(dot, result[1]);
            }
            return result;
        }

        // 检查线段是否相交
        private static bool CheckLineCross(FixIntVector3 start1, FixIntVector3 end1, FixIntVector3 start2, FixIntVector3 end2)
        {
            // 快速排斥
            FixInt minX1 = FixIntMath.Min(start1.x, end1.x);
            FixInt minX2 = FixIntMath.Min(start2.x, end2.x);
            FixInt maxX1 = FixIntMath.Max(start1.x, end1.x);
            FixInt maxX2 = FixIntMath.Max(start2.x, end2.x);
            if (minX1 > maxX2 || minX2 > maxX1) return false;

            FixInt minY1 = FixIntMath.Min(start1.y, end1.y);
            FixInt minY2 = FixIntMath.Min(start2.y, end2.y);
            FixInt maxY1 = FixIntMath.Max(start1.y, end1.y);
            FixInt maxY2 = FixIntMath.Max(start2.y, end2.y);
            if (minY1 > maxY2 || minY2 > maxY1) return false;

            FixInt minZ1 = FixIntMath.Min(start1.z, end1.z);
            FixInt minZ2 = FixIntMath.Min(start2.z, end2.z);
            FixInt maxZ1 = FixIntMath.Max(start1.z, end1.z);
            FixInt maxZ2 = FixIntMath.Max(start2.z, end2.z);
            if (minZ1 > maxZ2 || minZ2 > maxZ1) return false;

            if (IsPointOnSegment(start1, minX2, maxX2, minY2, maxY2, minZ2, maxZ2)) return true;
            if (IsPointOnSegment(end1, minX2, maxX2, minY2, maxY2, minZ2, maxZ2)) return true;
            if (IsPointOnSegment(start2, minX1, maxX1, minY1, maxY1, minZ1, maxZ1)) return true;
            if (IsPointOnSegment(end2, minX1, maxX1, minY1, maxY1, minZ1, maxZ1)) return true;

            FixIntVector3 line1 = end1 - start1;
            FixIntVector3 line2 = end2 - start2;

            // 跨立
            FixIntVector3 cross1 = FixIntVector3.Cross(line1, start2 - start1);
            FixIntVector3 cross2 = FixIntVector3.Cross(line1, end2 - start1);
            FixIntVector3 cross3 = FixIntVector3.Cross(line2, start1 - start2);
            FixIntVector3 cross4 = FixIntVector3.Cross(line2, end1 - start2);

            if (FixIntVector3.Dot(cross1, cross2) > 0) // 同侧，不相交
            {
                return false;
            }
            if (FixIntVector3.Dot(cross3, cross4) > 0) // 同侧，不相交
            {
                return false;
            }

            return true;
        }

        // 判断点是否在线段上
        private static bool IsPointOnSegment(FixIntVector3 point, FixInt minX, FixInt maxX, FixInt minY, FixInt maxY, FixInt minZ, FixInt maxZ)
        {
            return point.x >= minX && point.x <= maxX &&
                   point.y >= minY && point.y <= maxY && point.z >= minZ && point.z <= maxZ;
        }
    }
}
