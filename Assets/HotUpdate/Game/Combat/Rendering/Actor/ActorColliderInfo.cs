using System.Collections;
using System.Collections.Generic;
using Drawing;
using Unity.Mathematics;
using UnityEngine;

public class ActorColliderInfo : MonoBehaviourGizmos
{
    public float Radius;
    public float Height;
    public Color Color;

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        Draw.WireCapsule(
            new float3(transform.position - transform.up * 0.5f * Height),
            new float3(transform.up),
            Height,
            Radius,
            Color
        );
    }
}
