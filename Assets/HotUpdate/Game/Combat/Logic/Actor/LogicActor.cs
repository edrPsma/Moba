using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;


public abstract class LogicActor
{
    public int ActorID { get; }
    public EActorLayer ActorLayer { get; set; }
    public abstract FixIntVector3 Position { get; }
    public abstract FixIntVector3 EurAngle { get; }

    public LogicActor(int actorID, EActorLayer layer)
    {
        ActorID = actorID;
        ActorLayer = layer;
    }

    public virtual void LogicUpdate(FixInt deltaTime)
    {

    }
}
public abstract class LogicActor<T> : LogicActor where T : RenderingActor
{
    public T RenderingActor { get; }

    public LogicActor(int actorID, EActorLayer layer, T renderingActor) : base(actorID, layer)
    {
        RenderingActor = renderingActor;
        renderingActor.Initialize(this);
    }

    public virtual void Dispose()
    {

    }
}

public enum EActorLayer
{
    Red,

    Blue
}
