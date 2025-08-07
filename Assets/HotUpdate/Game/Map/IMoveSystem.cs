using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using OBB;
using UnityEngine;

public interface IMoveSystem : ILogicController
{
    void AddObstacle(OBBCollider collider);

    void RemoveObstacle(OBBCollider collider);

    void AddUnit(OBBCollider collider);

    void RemoveUnit(OBBCollider collider);
}

[Controller]
public class MoveSystem : AbstarctController, IMoveSystem
{
    List<OBBCollider> _units = new List<OBBCollider>();
    List<OBBCollider> _obstacles = new List<OBBCollider>();
    List<CollisionData> _cacheCollisionDatas = new List<CollisionData>();

    public void LogicUpdate(FixInt deltaTime)
    {
        for (int i = 0; i < _units.Count; i++)
        {
            var item = _units[i];
            item.SyncCollisionData();
            _cacheCollisionDatas.Clear();

            for (int j = 0; j < _obstacles.Count; j++)
            {
                var target = _obstacles[j];
                if (item == target)
                {
                    continue;
                }

                target.SyncCollisionData();
                //碰撞检测逻辑
                if (item.DetectCollider(target, out CollisionData collisionData))
                {
                    if (item.IsUseAdjustPos)
                    {
                        _cacheCollisionDatas.Add(collisionData);
                    }
                }
            }

            AdjustPos(item, deltaTime);
        }
    }

    public void AddObstacle(OBBCollider collider)
    {
        _obstacles.Add(collider);
    }

    public void RemoveObstacle(OBBCollider collider)
    {
        _obstacles.Remove(collider);
    }

    public void AddUnit(OBBCollider collider)
    {
        _units.Add(collider);
    }

    public void RemoveUnit(OBBCollider collider)
    {
        _units.Remove(collider);
    }

    void AdjustPos(OBBCollider collider, FixInt deltaTime)
    {
        if (_cacheCollisionDatas.Count == 0 || !collider.IsUseAdjustPos)
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
}
