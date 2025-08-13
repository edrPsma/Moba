using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using OBB;
using UnityEngine;


public abstract class LogicActor
{
    public int ActorID { get; }
    public ECamp Camp { get; set; }
    public ELayer Layer { get; set; }
    public abstract FixIntVector3 Position { get; }
    public abstract FixIntVector3 Velocity { get; set; }
    public abstract FixInt MoveSpeed { get; set; }
    public abstract OBBCollider Collider { get; }
    public AttributeSet AttributeSet { get; }
    public abstract RenderingActor Rendering { get; }
    public SkillOwner SkillOwner { get; }
    public BuffOwner BuffOwner { get; }

    public LogicActor(int actorID, ECamp camp, ELayer layer)
    {
        ActorID = actorID;
        Camp = camp;
        Layer = layer;
        AttributeSet = new AttributeSet();
        AttributeSet.Initialize(this);
        SkillOwner = MVCContainer.NewAndInject<SkillOwner>();
        SkillOwner.Initialize(this);
        BuffOwner = MVCContainer.NewAndInject<BuffOwner>();
        BuffOwner.Initialize(this);
    }

    public virtual void LogicUpdate(FixInt deltaTime)
    {
        SkillOwner.LogicUpdate(deltaTime);
        BuffOwner.LogicUpdate(deltaTime);
    }

    public abstract FixIntVector3 Direction { get; set; }
}
public abstract class LogicActor<T> : LogicActor where T : RenderingActor
{
    public T RenderingActor { get; }
    public override RenderingActor Rendering => RenderingActor;

    public LogicActor(int actorID, ECamp camp, ELayer layer, T renderingActor) : base(actorID, camp, layer)
    {
        RenderingActor = renderingActor;
        renderingActor.Initialize(this);
    }

    public virtual void Dispose()
    {

    }
}

public enum ECamp
{
    Neutral = 0,

    Red = 1,

    Blue = 2,
}
