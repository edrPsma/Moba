using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using OBB;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class CapsuleObstacle : Obstacle
{
    [Inject] public IMoveSystem MoveSystem;

    [SerializeField] float _radius;
    [SerializeField] float _height;

    OBBCapsuleCollider _capsuleCollider;
    Color _color = Color.blue;

    void Start()
    {
        _capsuleCollider = new OBBCapsuleCollider(_radius, _height, new FixIntVector3(transform.up));
        SetData();
        MoveSystem.AddObstacle(_capsuleCollider);
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        Draw.WireCapsule(
            new float3(transform.position - transform.up * 0.5f * _height),
            new float3(transform.up),
            _height,
            _radius,
            _color
        );
    }

    void SetData()
    {
        _capsuleCollider.Radius = _radius;
        _capsuleCollider.Height = _height;
        _capsuleCollider.Direction = new FixIntVector3(transform.up);
        _capsuleCollider.Position = new FixIntVector3(transform.position);
    }
}
