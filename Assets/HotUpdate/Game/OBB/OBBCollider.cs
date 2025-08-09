using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

namespace OBB
{
    public abstract class OBBCollider
    {
        public FixIntVector3 Position
        {
            get => _position;
            set
            {
                if (value != _position)
                {
                    _position = value;
                    OnPositionChange?.Invoke();
                }
            }
        }
        FixIntVector3 _position;
        public FixIntVector3 Scale { get; protected set; } = FixIntVector3.one;
        public abstract EOBBColliderType ColliderType { get; }

        protected bool _isDirty;
        protected bool _isColliding;

        public Action<OBBCollider, CollisionData> OnCollisionEnterAction;
        public Action<OBBCollider, CollisionData> OnCollisionStayAction;
        public Action<OBBCollider> OnCollisionExitAction;
        public Action OnCollisionEmptyAction;
        public Action OnPositionChange;

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
            BoxColliderData box1 = boxCollider.GetData();
            BoxColliderData box2 = target.GetData();

            bool isColliding = OBBCollisionTools.CollisionDetect(box1, box2, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBBoxCollider boxCollider, OBBSphereCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            BoxColliderData box = boxCollider.GetData();
            SphereColliderData sphere = target.GetData();

            bool isColliding = OBBCollisionTools.CollisionDetect(box, sphere, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBSphereCollider sphereCollider, OBBSphereCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            SphereColliderData sphere1 = sphereCollider.GetData();
            SphereColliderData sphere2 = target.GetData();

            bool isColliding = OBBCollisionTools.CollisionDetect(sphere1, sphere2, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBSphereCollider sphereCollider, OBBCapsuleCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            SphereColliderData sphere = sphereCollider.GetData();
            CapsuleColliderData capsule = target.GetData();

            bool isColliding = OBBCollisionTools.CollisionDetect(sphere, capsule, out collisionData);
            if (target == impactor)
            {
                collisionData.Normal *= -1;
            }

            return isColliding;
        }

        public bool DetectCollider(OBBBoxCollider boxCollider, OBBCapsuleCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            BoxColliderData box = boxCollider.GetData();
            CapsuleColliderData capsule = target.GetData();

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