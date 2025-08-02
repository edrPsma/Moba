using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using Unity.Mathematics;
using UnityEngine;

namespace OBB
{
    public class UnityOBBSphereColliderDemo : MonoBehaviourGizmos
    {
        [SerializeField] float _radius;

        OBBSphereCollider _sphereCollider;
        Color _color = Color.blue;

        void Start()
        {
            _sphereCollider = new OBBSphereCollider(_radius);
            SetData();
            _sphereCollider.OnCollisionEnterAction = OnCollisionEnterFunc;
            _sphereCollider.OnCollisionEmptyAction = OnCollisionEmptyFunc;
            _sphereCollider.OnCollisionStayAction = OnCollisionStayFunc;
            OBBManagerDemo.Instance.AddCollider(_sphereCollider);
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

            Draw.WireSphere(
                new float3(transform.position),
                _radius,
                _color
            );
        }

        void SetData()
        {
            _sphereCollider.Radius = _radius;
            _sphereCollider.Position = new FixIntVector3(transform.position);
        }
    }
}