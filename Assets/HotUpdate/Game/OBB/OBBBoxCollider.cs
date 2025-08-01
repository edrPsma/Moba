using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;

namespace OBB
{
    public class OBBBoxCollider : OBBCollider
    {
        public override EOBBColliderType ColliderType => EOBBColliderType.Box;

        public FixIntVector3 Size { get; private set; }
        public FixIntVector3[] Vertexts { get; private set; }
        public FixIntVector3[] Axes { get; private set; } = GetDefultAxes();

        public OBBBoxCollider(FixIntVector3 size)
        {
            Size = size;

            OBBCollisionTools.CalBoxVertexts(Position, size, Scale, Vertexts = new FixIntVector3[8]);
        }

        public void SetSize(FixIntVector3 size)
        {
            Size = size;
            _isDirty = true;
        }

        public void SetAxes(FixIntVector3 x, FixIntVector3 y, FixIntVector3 z)
        {
            Axes[0] = x;
            Axes[1] = y;
            Axes[2] = z;
            _isDirty = true;
        }

        public void SetScale(FixIntVector3 scale)
        {
            Scale = scale;
            _isDirty = true;
        }

        public override void SyncCollisionData()
        {
            if (_isDirty)
            {
                OBBCollisionTools.CalBoxVertexts(Position, Size, Scale, Vertexts);
                _isDirty = false;
            }
        }

        public override bool DetectCollider(OBBCollider target, out CollisionData collisionData)
        {
            if (target.ColliderType == EOBBColliderType.Box)
            {
                return OBBManager.Instance.DetectCollider(this, target as OBBBoxCollider, this, out collisionData);
            }
            else if (target.ColliderType == EOBBColliderType.Sphere)
            {
                return OBBManager.Instance.DetectCollider(this, target as OBBSphereCollider, this, out collisionData);
            }
            else if (target.ColliderType == EOBBColliderType.Capsule)
            {
                return OBBManager.Instance.DetectCollider(this, target as OBBCapsuleCollider, this, out collisionData);
            }
            else
            {
                collisionData = default;
                return false;
            }
        }

        static FixIntVector3[] GetDefultAxes()
        {
            FixIntVector3[] result = new FixIntVector3[3];

            result[0] = new FixIntVector3(1, 0, 0);
            result[1] = new FixIntVector3(0, 1, 0);
            result[2] = new FixIntVector3(0, 1, 1);

            return result;
        }

        // void Update()
        // {
        //     Position = new FixIntVector3(transform.position.x, transform.position.y, transform.position.z);
        //     SetAxes(transform);
        //     Drawing.Draw.WireBox(new float3(Position.x.RawFloat, Position.y.RawFloat, Position.z.RawFloat),
        //     transform.rotation,
        //     new float3(Size.x.RawFloat, Size.y.RawFloat, Size.z.RawFloat),
        //      Color.blue);
        // }
    }
}