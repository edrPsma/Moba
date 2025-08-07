using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRenderingActor : RenderingActor
{
    public Transform BodyTrans;
    public Transform HeadTrans;
    public ActorColliderInfo ColliderInfo;
    public HeroActor _heroActor;

    const int PredicMaxCount = 15;
    [SerializeField] bool _predictPos = true;
    [SerializeField] bool _smoothPos = true;
    [SerializeField] float _viewPosAcce = 10;
    [SerializeField] float _viewDirAccer = 10;
    [SerializeField] float _angleMultiplier = 8;
    Vector3 _targetPos;
    Vector3 _targetDir;
    int _predictCount;

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
        Vector3 dir = _heroActor.Velocity.ToVector3().normalized;

        if (_predictPos)
        {
            if (_predictCount > PredicMaxCount) return;

            float deltaTime = Time.deltaTime;
            var predictPos = deltaTime * _heroActor.MoveSpeed.RawFloat * dir;
            _targetPos += predictPos;
            ++_predictCount;

            if (_smoothPos)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _viewPosAcce);
            }
            else
            {
                transform.position = _targetPos;
            }
        }
        else
        {
            transform.position = _heroActor.Position.ToVector3();
        }

        float threshold = Time.deltaTime * _viewDirAccer;
        float angle = Vector3.Angle(transform.forward, _targetDir);
        float angleMult = (angle / 180) * _angleMultiplier * Time.deltaTime;

        if (_targetDir != Vector3.zero)
        {
            Vector3 interDir = Vector3.Lerp(transform.forward, _targetDir, threshold + angleMult);
            transform.forward = interDir;
        }

        ColliderInfo.transform.position = _heroActor.HitBox.Position.ToVector3();
    }

    public void UpdatePosition()
    {
        _predictCount = 0;
        _targetDir = _heroActor.Velocity.ToVector3().normalized;
        _targetPos = _heroActor.Position.ToVector3();
    }

    public void UpdatePositionForce()
    {
        _predictCount = 0;
        _targetDir = _heroActor.Velocity.ToVector3().normalized;
        _targetPos = _heroActor.Position.ToVector3();
        transform.position = _targetPos;
        if (_targetDir != Vector3.zero)
        {
            transform.forward = _targetDir;
        }
    }

    protected virtual void Update()
    {
        SyncInfo();
    }
}
