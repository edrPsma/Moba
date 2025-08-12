using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class SkillReleaseItem : MonoView, IPointerDownHandler, IPointerUpHandler
{
    [Inject] public IAssetSystem AssetSystem;
    [Inject] public IOperateSystem OperateSystem;
    [Inject] public IActorManager ActorManager;

    [SerializeField] ESkillReleaseType _type;
    [SerializeField] Button _button;
    [SerializeField] Joystick _joystick;
    [SerializeField] GameObject _bgObj;
    [SerializeField] GameObject _handleObj;

    [SerializeField] int _minAngle = 10;

    List<LogicActor> _cacheArr;
    LogicActor _lockTarget;

    int _skillID;
    SkillConfig _skillConfig;

    void Start()
    {
        _cacheArr = new List<LogicActor>();
        _joystick.OnDragEvent += OnDrag;
        _joystick.OnDragEndEvent += OnDragEnd;
        _joystick.OnDragStartEvent += OnDragStart;
    }

    [Button]
    public void Refresh(int skillID)
    {
        _skillID = skillID;
        _skillConfig = AssetSystem.GetSkillConfig(skillID);
        SetType(_skillConfig.ReleaseType);
    }

    [Button]
    public void SetType(ESkillReleaseType type)
    {
        if (ESkillReleaseType.NoTarget == type)
        {
            _button.enabled = true;
            _joystick.enabled = false;
        }
        else
        {
            _button.enabled = false;
            _joystick.enabled = true;
        }
        _type = type;
    }

    private void OnDragStart(Vector2 vector)
    {
        _bgObj.SetActive(true);
        _handleObj.SetActive(true);
        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, vector, _skillConfig.SelectArea, _skillConfig.DamageArea[0], null));
    }

    private void OnDragEnd(Vector2 vector)
    {
        Debug.Log($"释放技能, Vet:{vector}");
        _bgObj.SetActive(false);
        _handleObj.SetActive(false);

        if (_type == ESkillReleaseType.NoTarget)
        {
            HeroActor heroActor = ActorManager.GetSelfHero();
            OperateSystem.SendSkillOperate(_skillID, FixIntVector3.zero, heroActor.ActorID);
        }
        else if (_type == ESkillReleaseType.TargetUnit)
        {
            HeroActor heroActor = ActorManager.GetSelfHero();
            if (_lockTarget == null)
            {
                ELayer layer = CombatUtility.GetSkillLayer(_skillConfig, heroActor.Camp);
                _cacheArr.Clear();
                CombatUtility.SelectActor(heroActor.Position, _skillConfig.SelectArea, layer, _cacheArr);
                if (_cacheArr.Count != 0)
                {
                    _lockTarget = CombatUtility.SelectClosestActor(heroActor.Position, _cacheArr);
                    OperateSystem.SendSkillOperate(_skillID, FixIntVector3.zero, _lockTarget.ActorID);
                }
            }
            else
            {
                OperateSystem.SendSkillOperate(_skillID, FixIntVector3.zero, _lockTarget.ActorID);
            }

        }
        else if (_type == ESkillReleaseType.TargetPoint)
        {
            HeroActor heroActor = ActorManager.GetSelfHero();
            FixIntVector3 dir = new FixIntVector3(vector.x, 0, vector.y);
            FixIntVector3 targetDir = RotateY45(dir);

            SkillConfig skillConfig = AssetSystem.GetSkillConfig(_skillID);
            FixIntVector3 pos = targetDir * skillConfig.SelectArea + heroActor.Position;

            OperateSystem.SendSkillOperate(_skillID, pos, 0);
        }
        else if (_type == ESkillReleaseType.VectorSkill)
        {
            HeroActor heroActor = ActorManager.GetSelfHero();
            FixIntVector3 dir = new FixIntVector3(vector.x, 0, vector.y);
            FixIntVector3 targetDir = RotateY45(dir);

            OperateSystem.SendSkillOperate(_skillID, targetDir, 0);
        }
        else
        {
            Debug.LogError($"位置技能释放类型,Type: {_type}");
        }

        _lockTarget = null;
    }

    void FixedUpdate()
    {
        if (_skillConfig == null) return;

        if (_type == ESkillReleaseType.TargetUnit)
        {
            if (_joystick.Direction == Vector2.zero) return;

            HeroActor heroActor = ActorManager.GetSelfHero();
            ELayer layer = CombatUtility.GetSkillLayer(_skillConfig, heroActor.Camp);
            _cacheArr.Clear();
            CombatUtility.SelectActor(heroActor.Position, _skillConfig.SelectArea, layer, _cacheArr);
            if (_cacheArr.Count != 0)
            {
                FixIntVector3 dir = new FixIntVector3(_joystick.Direction.x, 0, _joystick.Direction.y);
                FixIntVector3 targetDir = RotateY45(dir);
                FixIntVector3 pos = targetDir * _skillConfig.SelectArea + heroActor.Position;

                // 若夹角小于_minAngle 且pos到自身距离大于目标到自身距离 则认为锁定
                FixInt minAnlge = FixInt.MaxValue;
                LogicActor target = null;
                foreach (var item in _cacheArr)
                {
                    FixIntVector3 curDir = item.Position - heroActor.Position;
                    FixInt angle = FixIntVector3.Angle(targetDir, curDir);
                    if (angle <= _minAngle)
                    {
                        FixInt dist1 = (pos - heroActor.Position).sqrMagnitude;
                        FixInt dist2 = curDir.sqrMagnitude;
                        if (dist1 > dist2)
                        {
                            if (angle < minAnlge)
                            {
                                minAnlge = angle;
                                target = item;
                            }
                        }
                    }
                }
                if (target != null)
                {
                    _lockTarget = target;
                }
            }
            else
            {
                _lockTarget = null;
            }

            GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, _joystick.Direction, _skillConfig.SelectArea, _skillConfig.DamageArea[0], _lockTarget));
        }
    }

    private void OnDrag(Vector2 vector)
    {
        Debug.Log($"调整技能方向, Vet:{vector}");

        _lockTarget = null;
        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, vector, _skillConfig.SelectArea, _skillConfig.DamageArea[0], _lockTarget));
    }

    FixIntVector3 RotateY45(FixIntVector3 v)
    {
        FixInt c = 0.7071067f; // sqrt(2) / 2

        FixInt x = v.x * c + v.z * c;
        FixInt z = -v.x * c + v.z * c;
        return new FixIntVector3(x, v.y, z);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, _joystick.Direction, _skillConfig.SelectArea, _skillConfig.DamageArea[0], _lockTarget));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameEntry.Event.Trigger(new EventShowSkillIndicator(ESkillReleaseType.NoTarget, Vector2.zero, 0, 0, null));
    }
}
