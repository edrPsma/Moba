using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Unity.Mathematics;
using UnityEngine;

namespace OBB
{
    public class OBBSphereCollider : OBBCollider
    {
        public override EOBBColliderType ColliderType => EOBBColliderType.Sphere;

        /// <summary>
        /// 半径
        /// </summary>
        public FixInt Radius { get; set; }

        public OBBSphereCollider(FixInt radius)
        {
            Radius = radius;
        }

        public void SetScale(FixIntVector3 scale)
        {
            Scale = scale;
        }

        protected override bool OnDetectCollider(OBBCollider target, out CollisionData collisionData)
        {
            if (target.ColliderType == EOBBColliderType.Box)
            {
                return DetectCollider(target as OBBBoxCollider, this, this, out collisionData);
            }
            else if (target.ColliderType == EOBBColliderType.Sphere)
            {
                return DetectCollider(this, target as OBBSphereCollider, this, out collisionData);
            }
            else if (target.ColliderType == EOBBColliderType.Capsule)
            {
                return DetectCollider(this, target as OBBCapsuleCollider, this, out collisionData);
            }
            else
            {
                collisionData = default;
                return false;
            }
        }

        public override void SyncCollisionData()
        {

        }
    }
}