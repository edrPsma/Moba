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

        public abstract bool DetectCollider(OBBCollider target, out CollisionData collisionData);
        public abstract void SyncCollisionData();
    }

    public enum EOBBColliderType
    {
        Box,

        Sphere,

        Capsule
    }
}