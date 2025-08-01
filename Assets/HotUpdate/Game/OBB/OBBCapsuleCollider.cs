using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;

namespace OBB
{
    public class OBBCapsuleCollider : OBBCollider
    {
        public override EOBBColliderType ColliderType => EOBBColliderType.Capsule;

        public FixInt Height;
        public FixInt Radius;
        public FixIntVector3 Direction;

        public OBBCapsuleCollider(FixInt radius, FixInt height, FixIntVector3 direction)
        {
            Radius = radius;
            Height = height;
            Direction = direction;
        }

        protected override bool OnDetectCollider(OBBCollider target, out CollisionData collisionData)
        {
            if (target.ColliderType == EOBBColliderType.Box)
            {
                return DetectCollider(target as OBBBoxCollider, this, this, out collisionData);
            }
            else if (target.ColliderType == EOBBColliderType.Sphere)
            {
                return DetectCollider(target as OBBSphereCollider, this, this, out collisionData);
            }
            else
            {
                collisionData = default;
                return false;
            }
        }

        public void SetScale(FixIntVector3 scale)
        {
            Scale = scale;
        }

        public void SetRadius(FixInt radius)
        {
            Radius = radius;
        }

        public void SetHeight(FixInt height)
        {
            Height = height;
        }

        public override void SyncCollisionData()
        {

        }
    }
}