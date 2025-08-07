using System.Collections;
using System.Collections.Generic;
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

    void SyncInfo()
    {
        ColliderInfo.transform.position = _heroActor.HitBox.Position.ToVector3();
        transform.position = _heroActor.Position.ToVector3();
        transform.eulerAngles = LogicActor.EurAngle.ToVector3();
    }

    protected virtual void Update()
    {
        SyncInfo();
    }
}
