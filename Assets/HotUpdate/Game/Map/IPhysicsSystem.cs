using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using OBB;
using UnityEngine;

public interface IPhysicsSystem : ILogicController
{
    void AddObstacle(OBBCollider collider);

    void RemoveObstacle(OBBCollider collider);

    void AddUnit(LogicActor actor);

    void RemoveUnit(LogicActor actor);

    int OverlapBox(FixIntVector3 pos, FixIntVector3 size, FixIntVector3 dir, ELayer layer, LogicActor[] arr);

    int OverlapSphere(FixIntVector3 pos, FixInt radius, ELayer layer, LogicActor[] arr);
}

[Controller]
public class PhysicsSystem : AbstarctController, IPhysicsSystem
{
    Dictionary<ELayer, List<LogicActor>> _unitsDic = new Dictionary<ELayer, List<LogicActor>>();
    List<OBBCollider> _obstacles = new List<OBBCollider>();
    List<CollisionData> _cacheCollisionDatas = new List<CollisionData>();

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Array array = Enum.GetValues(typeof(ELayer));
        foreach (ELayer item in array)
        {
            _unitsDic.Add(item, new List<LogicActor>());
        }
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        foreach (var item in _unitsDic)
        {
            List<LogicActor> list = item.Value;

            for (int i = 0; i < list.Count; i++)
            {
                var unit = list[i];
                if (unit.Collider == null) continue;

                unit.Collider.SyncCollisionData();
                _cacheCollisionDatas.Clear();

                for (int j = 0; j < _obstacles.Count; j++)
                {
                    var target = _obstacles[j];

                    target.SyncCollisionData();
                    //碰撞检测逻辑
                    if (unit.Collider.DetectCollider(target, out CollisionData collisionData))
                    {
                        if (unit.Collider.IsUseAdjustPos)
                        {
                            _cacheCollisionDatas.Add(collisionData);
                        }
                    }
                }

                AdjustPos(unit.Collider, deltaTime);
            }
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

    public void AddUnit(LogicActor actor)
    {
        _unitsDic[actor.Layer].Add(actor);
    }

    public void RemoveUnit(LogicActor actor)
    {
        _unitsDic[actor.Layer].Remove(actor);
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

    FixIntVector3[] vertexts = new FixIntVector3[8];
    public int OverlapBox(FixIntVector3 pos, FixIntVector3 size, FixIntVector3 dir, ELayer layer, LogicActor[] arr)
    {
        int num = 0;

        FixIntVector3 yAxes = FixIntVector3.up;
        FixIntVector3 xAxes = FixIntVector3.Cross(dir, yAxes);

        FixIntVector3[] axes = new FixIntVector3[]
        {
            xAxes,yAxes,dir
        };

        OBBCollisionTools.CalBoxVertexts(pos, size, FixIntVector3.one, axes, vertexts);
        BoxColliderData box = new BoxColliderData
        {
            Vertexts = vertexts,
            Axes = axes,
            Center = pos,
            Size = size
        };

        CollisionData collisionData;

        foreach (var item in _unitsDic)
        {
            ELayer tempLayer = item.Key;
            if ((tempLayer & layer) != tempLayer) continue;

            List<LogicActor> list = item.Value;
            for (int i = 0; i < list.Count; i++)
            {
                OBBCapsuleCollider capsuleCollider = list[i].Collider as OBBCapsuleCollider;
                if (OBBCollisionTools.CollisionDetect(box, capsuleCollider.GetData(), out collisionData))
                {
                    arr[num] = list[i];
                    num++;
                }
            }
        }

        return num;
    }

    public int OverlapSphere(FixIntVector3 pos, FixInt radius, ELayer layer, LogicActor[] arr)
    {
        int num = 0;

        SphereColliderData sphereColliderData = new SphereColliderData
        {
            Center = pos,
            Radius = radius
        };

        CollisionData collisionData;

        foreach (var item in _unitsDic)
        {
            ELayer tempLayer = item.Key;
            if ((tempLayer & layer) != tempLayer) continue;

            List<LogicActor> list = item.Value;
            for (int i = 0; i < list.Count; i++)
            {
                OBBCapsuleCollider capsuleCollider = list[i].Collider as OBBCapsuleCollider;
                if (OBBCollisionTools.CollisionDetect(sphereColliderData, capsuleCollider.GetData(), out collisionData))
                {
                    arr[num] = list[i];
                    num++;
                }
            }
        }

        return num;
    }
}
