using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

namespace OBB
{
    public abstract class OBBCollider
    {
        public FixIntVector3 Position { get; set; }
        public FixIntVector3 Scale { get; protected set; } = FixIntVector3.one;
        public abstract EOBBColliderType ColliderType { get; }

        protected bool _isDirty;
        protected bool _isColliding;

        public Action<OBBCollider, CollisionData> OnCollisionEnterAction;
        public Action<OBBCollider, CollisionData> OnCollisionStayAction;
        public Action<OBBCollider> OnCollisionExitAction;
        public Action OnCollisionEmptyAction;

        protected List<OBBCollider> _alreayOccurCollisionList = new List<OBBCollider>();

        // 移动矫正
        public bool IsUseAdjustPos;
        public FixIntVector3 Velocity;

        public bool DetectCollider(OBBCollider target, out CollisionData collisionData)
        {
            _isColliding = OnDetectCollider(target, out collisionData);

            if (_isColliding)
            {
                if (!_alreayOccurCollisionList.Contains(target))
                {
                    //首次碰撞
                    OnCollisionEnterAction?.Invoke(target, collisionData);
                    _alreayOccurCollisionList.Add(target);
                }
                else
                {
                    //碰撞停留
                    OnCollisionStayAction?.Invoke(target, collisionData);
                }
            }
            else
            {
                //碰撞离开
                if (_alreayOccurCollisionList.Contains(target))
                {
                    _alreayOccurCollisionList.Remove(target);
                    OnCollisionExitAction?.Invoke(target);

                    if (_alreayOccurCollisionList.Count == 0)
                    {
                        OnCollisionEmptyAction?.Invoke();
                    }
                }

            }

            return _isColliding;
        }

        protected abstract bool OnDetectCollider(OBBCollider target, out CollisionData collisionData);
        public abstract void SyncCollisionData();

        public bool DetectCollider(OBBBoxCollider boxCollider, OBBBoxCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            BoxColliderData box1 = new BoxColliderData
            {
                Vertexts = boxCollider.Vertexts,
                Axes = boxCollider.Axes,
                Center = boxCollider.Position,
                Size = boxCollider.Size
            };

            BoxColliderData box2 = new BoxColliderData
            {
                Vertexts = target.Vertexts,
                Axes = target.Axes,
                Center = target.Position,
                Size = target.Size
            };

            bool isColliding = OBBCollisionTools.CollisionDetect(box1, box2, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBBoxCollider boxCollider, OBBSphereCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            BoxColliderData box = new BoxColliderData
            {
                Vertexts = boxCollider.Vertexts,
                Axes = boxCollider.Axes,
                Center = boxCollider.Position,
                Size = boxCollider.Size
            };

            FixInt scale = FixIntMath.Max(target.Scale.x, target.Scale.y);
            scale = FixIntMath.Max(target.Scale.z, scale);

            SphereColliderData sphere = new SphereColliderData
            {
                Center = target.Position,
                Radius = scale * target.Radius
            };

            bool isColliding = OBBCollisionTools.CollisionDetect(box, sphere, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBSphereCollider sphereCollider, OBBSphereCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            FixInt scale1 = FixIntMath.Max(sphereCollider.Scale.x, sphereCollider.Scale.y);
            scale1 = FixIntMath.Max(sphereCollider.Scale.z, scale1);

            SphereColliderData sphere1 = new SphereColliderData
            {
                Center = sphereCollider.Position,
                Radius = scale1 * sphereCollider.Radius
            };

            FixInt scale2 = FixIntMath.Max(target.Scale.x, target.Scale.y);
            scale2 = FixIntMath.Max(target.Scale.z, scale2);

            SphereColliderData sphere2 = new SphereColliderData
            {
                Center = target.Position,
                Radius = scale2 * target.Radius
            };

            bool isColliding = OBBCollisionTools.CollisionDetect(sphere1, sphere2, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBSphereCollider sphereCollider, OBBCapsuleCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            FixInt scale = FixIntMath.Max(sphereCollider.Scale.x, sphereCollider.Scale.y);
            scale = FixIntMath.Max(sphereCollider.Scale.z, scale);
            SphereColliderData sphere = new SphereColliderData
            {
                Center = sphereCollider.Position,
                Radius = scale * sphereCollider.Radius
            };

            FixInt scaleR = FixIntMath.Max(target.Scale.x, target.Scale.z);
            FixInt scaleH = target.Scale.y;

            FixInt h = scaleH * target.Height;
            FixInt r = scaleR * target.Radius;

            FixInt realH = FixIntMath.Clamp(h - r * 2, 0, h);
            CapsuleColliderData capsule = new CapsuleColliderData
            {
                Center = target.Position,
                Direction = target.Direction,
                Radius = r,
                Height = realH,
            };

            bool isColliding = OBBCollisionTools.CollisionDetect(sphere, capsule, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBBoxCollider boxCollider, OBBCapsuleCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            BoxColliderData box = new BoxColliderData
            {
                Vertexts = boxCollider.Vertexts,
                Axes = boxCollider.Axes,
                Center = boxCollider.Position,
                Size = boxCollider.Size
            };

            FixInt scaleR = FixIntMath.Max(target.Scale.x, target.Scale.z);
            FixInt scaleH = target.Scale.y;

            FixInt h = scaleH * target.Height;
            FixInt r = scaleR * target.Radius;

            FixInt realH = FixIntMath.Clamp(h - r * 2, 0, h);
            CapsuleColliderData capsule = new CapsuleColliderData
            {
                Center = target.Position,
                Direction = target.Direction,
                Radius = r,
                Height = realH,
            };

            bool isColliding = OBBCollisionTools.CollisionDetect(box, capsule, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }
    }

    public enum EOBBColliderType
    {
        Box,

        Sphere,

        Capsule
    }
}