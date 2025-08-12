using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public abstract class RenderingActor : MonoBehaviour
{
    public LogicActor LogicActor { get; private set; }

    public abstract Transform BodyTrans { get; protected set; }
    public abstract Transform HeadTrans { get; protected set; }

    const int PredicMaxCount = 15;
    [SerializeField] bool _predictPos = true;
    [SerializeField] bool _smoothPos = true;
    [SerializeField] bool _smoothRotate = true;

    [SerializeField] float _viewPosAcce = 10;
    [SerializeField] float _viewDirAccer = 10;
    [SerializeField] float _angleMultiplier = 8;
    Vector3 _targetPos;
    Vector3 _targetDir;
    int _predictCount;
    bool _isPosChange;

    public virtual void Initialize(LogicActor logicActor)
    {
        LogicActor = logicActor;
    }

    protected virtual void Update()
    {
        SyncMove();
        SyncDir();
    }

    public void UpdatePosition()
    {
        _predictCount = 0;
        _targetPos = LogicActor.Position.ToVector3();
        _isPosChange = true;
    }

    public void UpdateRotate()
    {
        _targetDir = LogicActor.Direction.ToVector3().normalized;
    }

    public void UpdatePositionForce()
    {
        _predictCount = 0;
        _targetDir = LogicActor.Direction.ToVector3().normalized;
        _targetPos = LogicActor.Position.ToVector3();
        transform.position = _targetPos;
        if (_targetDir != Vector3.zero)
        {
            transform.forward = _targetDir;
        }
    }

    public virtual void PlayAnimation(string name)
    {

    }

    #region 移动平滑
    void SyncMove()
    {
        if (_predictPos)
        {
            if (!_isPosChange)
            {
                if (_predictCount > PredicMaxCount) return;

                Vector3 dir = LogicActor.Velocity.ToVector3().normalized;
                float deltaTime = Time.deltaTime;
                var predictPos = deltaTime * LogicActor.MoveSpeed.RawFloat * dir;
                _targetPos += predictPos;
                ++_predictCount;
            }
            else
            {
                if (transform.position != _targetPos)
                {
                    transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _viewPosAcce);
                }
                else
                {
                    _isPosChange = false;
                }
            }

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
            transform.position = LogicActor.Position.ToVector3();
        }

        if (LogicActor.Velocity != FixIntVector3.zero)
        {
            PlayAnimation("run");
        }
        else
        {
            PlayAnimation("idle");
        }
    }

    void SyncDir()
    {
        if (_smoothRotate)
        {
            float threshold = Time.deltaTime * _viewDirAccer;
            float angle = Vector3.Angle(transform.forward, _targetDir);
            float angleMult = (angle / 180) * _angleMultiplier * Time.deltaTime;

            if (_targetDir != Vector3.zero)
            {
                Vector3 interDir = Vector3.Lerp(transform.forward, _targetDir, threshold + angleMult);
                transform.forward = interDir;
            }
        }
        else
        {
            if (_targetDir != Vector3.zero)
            {
                transform.forward = _targetDir;
            }
        }
    }

    #endregion


}
