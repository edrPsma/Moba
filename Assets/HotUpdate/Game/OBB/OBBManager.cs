using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Template;
using UnityEngine;

namespace OBB
{
    public class OBBManager : MonoSingleton<OBBManager, OBBManager>
    {
        private List<OBBCollider> _colliders = new List<OBBCollider>();

        protected override void OnInit()
        {
            base.OnInit();
            GameObject.DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            LogicFrameUpdate();
        }

        public void LogicFrameUpdate()
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                var item = _colliders[i];
                item.SyncCollisionData();

                for (int j = 0; j < _colliders.Count; j++)
                {
                    var target = _colliders[j];
                    if (item == target)
                    {
                        continue;
                    }

                    target.SyncCollisionData();
                    //碰撞检测逻辑
                    item.DetectCollider(target);
                }
            }
        }

        public void AddCollider2D(OBBCollider collider)
        {
            _colliders.Add(collider);
        }

        public void RemoveCollider2D(OBBCollider collider)
        {
            _colliders.Remove(collider);
        }

        public bool DetectCollider(OBBBoxCollider boxCollider, OBBBoxCollider target, OBBCollider impactor, out CollisionData collisionData)
        {
            BoxColliderData box1 = new BoxColliderData
            {
                Vertexts = boxCollider.Vertexts,
                Axes = boxCollider.Axes,
                Center = boxCollider.Position,
            };

            BoxColliderData box2 = new BoxColliderData
            {
                Vertexts = target.Vertexts,
                Axes = target.Axes,
                Center = target.Position,
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
}