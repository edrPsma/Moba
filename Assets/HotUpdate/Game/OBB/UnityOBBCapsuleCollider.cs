using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using Unity.Mathematics;
using UnityEngine;

namespace OBB
{
    public class UnityOBBCapsuleCollider : MonoBehaviourGizmos
    {
        [SerializeField] float _radius;
        [SerializeField] float _height;

        OBBCapsuleCollider _capsuleCollider;
        Color _color = Color.blue;

        void Start()
        {
            _capsuleCollider = new OBBCapsuleCollider(_radius, _height, new FixIntVector3(transform.up));
            SetData();
            _capsuleCollider.OnCollisionEnterAction = OnCollisionEnterFunc;
            _capsuleCollider.OnCollisionEmptyAction = OnCollisionEmptyFunc;
            OBBManager.Instance.AddCollider2D(_capsuleCollider);
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
}