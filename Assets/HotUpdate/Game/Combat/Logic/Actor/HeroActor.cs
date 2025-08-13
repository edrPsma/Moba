using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using OBB;
using UnityEngine;
using Zenject;

public class HeroActor : LogicActor<HeroRenderingActor>
{
    public FixIntVector3 BodyPosition => new FixIntVector3(RenderingActor.BodyTrans.position);
    public FixIntVector3 HeadPosition => new FixIntVector3(RenderingActor.HeadTrans.position);
    public override OBBCollider Collider => HitBox;
    public OBBCapsuleCollider HitBox { get; private set; }

    public override FixIntVector3 Position => GetPosition();
    public override FixIntVector3 Velocity
    {
        get => HitBox.Velocity;
        set
        {
            HitBox.Velocity = value;

            if (value != FixIntVector3.zero)
            {
                Direction = value;
            }
        }
    }

    public override FixIntVector3 Direction
    {
        get => _dir;
        set
        {
            if (_dir != value && value != FixIntVector3.zero)
            {
                _dir = value;
                RenderingActor.UpdateDirection();
            }
        }
    }
    FixIntVector3 _dir;

    public override FixInt MoveSpeed { get; set; } = 3;


    public HeroActor(int actorID, ECamp camp, ELayer layer, HeroRenderingActor renderingActor) : base(actorID, camp, layer, renderingActor)
    {
        HitBox = new OBBCapsuleCollider(renderingActor.ColliderInfo.Radius, renderingActor.ColliderInfo.Height, FixIntVector3.up);
        HitBox.IsUseAdjustPos = true;
        HitBox.OnPositionChange += OnPositionChange;
    }

    private void OnPositionChange()
    {
        RenderingActor.UpdatePosition();
    }

    FixIntVector3 GetPosition()
    {
        if (HitBox == null) return FixIntVector3.zero;

        return HitBox.Position - new FixIntVector3(0, HitBox.Height / 2, 0);
    }

    public override void SetPosition(FixIntVector3 pos)
    {
        HitBox.Position = pos + new FixIntVector3(0, HitBox.Height / 2, 0);
        RenderingActor.UpdatePositionForce();
    }

    public override void SetDirection(FixIntVector3 dir)
    {
        Direction = dir.normalized;
        RenderingActor.UpdateDirectionForce();
    }
}
