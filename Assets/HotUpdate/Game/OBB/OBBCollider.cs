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

        public bool DetectCollider(OBBCollider target)
        {
            _isColliding = OnDetectCollider(target, out CollisionData collisionData);

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
    }

    public enum EOBBColliderType
    {
        Box,

        Sphere,

        Capsule
    }
}