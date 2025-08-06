using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using OBB;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class BoxObstacle : Obstacle
{
    [Inject] public IMoveSystem MoveSystem;

    [SerializeField] Vector3 _size;
    OBBBoxCollider _boxCollider;

    protected virtual void Start()
    {
        _boxCollider = new OBBBoxCollider(new FixIntVector3(_size));
        SetData();
        MoveSystem.AddObstacle(_boxCollider);
    }

    void Update()
    {
        SetData();
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        Draw.WireBox(new float3(transform.position),
            transform.rotation,
            new float3(_size),
            Color.blue
         );
    }

    void SetData()
    {
        _boxCollider.SetSize(new FixIntVector3(_size));
        _boxCollider.Position = new FixIntVector3(transform.position);
        _boxCollider.SetAxes(new FixIntVector3(transform.right), new FixIntVector3(transform.up), new FixIntVector3(transform.forward));
    }
}
