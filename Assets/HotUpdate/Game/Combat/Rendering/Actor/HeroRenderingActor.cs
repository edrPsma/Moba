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

    Animator _animator;

    public override void Initialize(LogicActor logicActor)
    {
        base.Initialize(logicActor);

        _heroActor = LogicActor as HeroActor;
        BodyTrans = transform.Find("Body");
        HeadTrans = transform.Find("Head");
        ColliderInfo = transform.Find<ActorColliderInfo>("HitBox");
        _animator = GetComponentInChildren<Animator>();
    }

    static string[] _animations = new string[]
    {
        "idle",
        "run",
        "attack",
        "skill"
    };

    public override void PlayAnimation(string name)
    {
        base.PlayAnimation(name);
        for (int i = 0; i < _animations.Length; i++)
        {
            if (_animations[i] == name)
            {
                _animator.SetBool(_animations[i], true);
            }
            else
            {
                _animator.SetBool(_animations[i], false);
            }
        }
    }
}
