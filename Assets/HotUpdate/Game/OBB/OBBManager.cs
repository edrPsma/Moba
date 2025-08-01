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
        List<CollisionData> _cacheCollisionDatas = new List<CollisionData>();

        protected override void OnInit()
        {
            base.OnInit();
            GameObject.DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            LogicFrameUpdate(Time.deltaTime);
        }

        public void LogicFrameUpdate(FixInt deltaTime)
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                var item = _colliders[i];
                item.SyncCollisionData();
                _cacheCollisionDatas.Clear();

                for (int j = 0; j < _colliders.Count; j++)
                {
                    var target = _colliders[j];
                    if (item == target)
                    {
                        continue;
                    }

                    target.SyncCollisionData();
                    //碰撞检测逻辑
                    if (item.IsUseAdjustPos && item.DetectCollider(target, out CollisionData collisionData))
                    {
                        _cacheCollisionDatas.Add(collisionData);
                    }
                }

                AdjustPos(item, deltaTime);
            }
        }

        void AdjustPos(OBBCollider collider, FixInt deltaTime)
        {
            if (!collider.IsUseAdjustPos) return;

            if (_cacheCollisionDatas.Count == 0)
            {
                collider.Position += collider.Velocity * deltaTime;
                return;
            }
            else if (_cacheCollisionDatas.Count == 1)
            {
                FixIntVector3 velocity = CorrectVelocity(collider.Velocity, _cacheCollisionDatas[0].Normal);

                collider.Position += velocity * deltaTime;
            }
            else
            {
                FixIntVector3 normal = FixIntVector3.zero;
                FixInt maxAngle = FixInt.MinValue;
                for (int i = 0; i < _cacheCollisionDatas.Count; i++)
                {
                    normal += _cacheCollisionDatas[i].Normal;
                }

                for (int i = 0; i < _cacheCollisionDatas.Count; i++)
                {
                    maxAngle = FixIntMath.Max(maxAngle, FixIntVector3.AngleBetween(normal, _cacheCollisionDatas[i].Normal));
                }

                FixInt speedAngle = FixIntVector3.AngleBetween(-collider.Velocity, normal);
                if (speedAngle > maxAngle)
                {
                    FixIntVector3 velocity = CorrectVelocity(collider.Velocity, normal);
                    collider.Position += velocity * deltaTime;
                }
            }
        }

        private FixIntVector3 CorrectVelocity(FixIntVector3 velocity, FixIntVector3 normal)
        {
            if (normal.sqrMagnitude < 0.001f || velocity.sqrMagnitude < 0.001f)
            {
                return velocity;
            }

            if (FixIntVector3.AngleBetween(normal, velocity) > 3.1415926f / 2f)
            {
                FixInt len = FixIntVector3.Dot(velocity, normal);

                if (len != 0)
                {
                    velocity -= len * normal;
                }
            }

            return velocity;
        }

        public void AddCollider(OBBCollider collider)
        {
            _colliders.Add(collider);
        }

        public void RemoveCollider(OBBCollider collider)
        {
            _colliders.Remove(collider);
        }
    }
}