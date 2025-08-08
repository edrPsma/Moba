using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public class HeroRenderingActor : RenderingActor
{
    public Transform BodyTrans;
    public Transform HeadTrans;
    public ActorColliderInfo ColliderInfo;
    public HeroActor _heroActor;

    public override void Initialize(LogicActor logicActor)
    {
        base.Initialize(logicActor);

        _heroActor = LogicActor as HeroActor;
        BodyTrans = transform.Find("Body");
        HeadTrans = transform.Find("Head");
        ColliderInfo = transform.Find<ActorColliderInfo>("HitBox");
    }

    public override void PlayAnimation(string name)
    {
        base.PlayAnimation(name);
    }
}
