using System;
using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using Unity.Mathematics;
using UnityEngine;

namespace OBB
{
    public class UnityOBBBoxCollider : MonoBehaviourGizmos
    {
        [SerializeField] Vector3 _size;
        OBBBoxCollider _boxCollider;
        Color _color = Color.blue;

        void Start()
        {
            _boxCollider = new OBBBoxCollider(new FixIntVector3(_size));
            SetData();
            _boxCollider.OnCollisionEnterAction = OnCollisionEnterFunc;
            _boxCollider.OnCollisionEmptyAction = OnCollisionEmptyFunc;
            _boxCollider.OnCollisionStayAction = OnCollisionStayFunc;
            OBBManager.Instance.AddCollider(_boxCollider);
        }

        private void OnCollisionStayFunc(OBBCollider collider, CollisionData data)
        {
            Draw.Line(transform.position, data.Normal.ToVector3() + transform.position, Color.green);
        }

        private void OnCollisionEmptyFunc()
        {
            _color = Color.blue;
        }

        private void OnCollisionEnterFunc(OBBCollider collider, CollisionData data)
        {
            _color = Color.red;
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
                _color
             );
        }

        void SetData()
        {
            _boxCollider.SetSize(new FixIntVector3(_size));
            _boxCollider.Position = new FixIntVector3(transform.position);
            _boxCollider.SetAxes(new FixIntVector3(transform.right), new FixIntVector3(transform.up), new FixIntVector3(transform.forward));
        }
    }
}