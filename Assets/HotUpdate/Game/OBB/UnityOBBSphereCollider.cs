using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using Unity.Mathematics;
using UnityEngine;

namespace OBB
{
    public class UnityOBBSphereCollider : MonoBehaviourGizmos
    {
        [SerializeField] float _radius;

        OBBSphereCollider _sphereCollider;

        void Start()
        {
            _sphereCollider = new OBBSphereCollider(_radius);
            SetData();
        }

        public override void DrawGizmos()
        {
            base.DrawGizmos();

            Draw.WireSphere(
                new float3(transform.position),
                _radius,
                Color.blue
            );
        }

        void SetData()
        {
            _sphereCollider.Radius = _radius;
            _sphereCollider.Position = new FixIntVector3(transform.position);
        }
    }
}