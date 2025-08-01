using System.Collections;
using System.Collections.Generic;
using Drawing;
using FixedPointNumber;
using Unity.Mathematics;
using UnityEngine;

namespace OBB
{
    public class PlayerMove : MonoBehaviourGizmos
    {
        [SerializeField] float _radius;
        [SerializeField] float _height;
        [SerializeField] float _moveSpeed;

        OBBCapsuleCollider _capsuleCollider;
        Color _color = Color.blue;

        void Start()
        {
            _capsuleCollider = new OBBCapsuleCollider(_radius, _height, new FixIntVector3(transform.up));
            _capsuleCollider.IsUseAdjustPos = true;
            _capsuleCollider.Position = new FixIntVector3(transform.position);
            SetData();
            _capsuleCollider.OnCollisionEnterAction = OnCollisionEnterFunc;
            _capsuleCollider.OnCollisionEmptyAction = OnCollisionEmptyFunc;
            _capsuleCollider.OnCollisionStayAction = OnCollisionStayFunc;
            OBBManager.Instance.AddCollider(_capsuleCollider);
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
            // SetData();
            FixInt moveHorizontal = Input.GetAxis("Horizontal"); // A/D 或 左右箭头
            FixInt moveVertical = Input.GetAxis("Vertical"); // W/S 或 上下箭头

            // 移动
            FixIntVector3 movement = new FixIntVector3(moveHorizontal * _moveSpeed, 0.0f, moveVertical * _moveSpeed);

            _capsuleCollider.Velocity = movement;

            transform.position = _capsuleCollider.Position.ToVector3();
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
            // _capsuleCollider.Position = new FixIntVector3(transform.position);
        }
    }
}