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

    [SerializeField] float area1;
    [SerializeField] float area2;

    int _skillID;

    void Start()
    {
        _joystick.OnDragEvent += OnDrag;
        _joystick.OnDragEndEvent += OnDragEnd;
        _joystick.OnDragStartEvent += OnDragStart;
    }

    [Button]
    public void Refresh(int skillID)
    {
        _skillID = skillID;
        DTSkill table = DataTable.GetItem<DTSkill>(skillID);
        SkillConfig skillConfig = AssetSystem.Get<SkillConfig>($"Assets/GameAssets/So/Skill/{table.Config}.asset");
        SetType(skillConfig.ReleaseType);
    }

    [Button]
    public void SetType(ESkillReleaseType type)
    {
        if (ESkillReleaseType.NoTarget == type || ESkillReleaseType.TargetUnit == type)
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
        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, vector, area1, area2));
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
            OperateSystem.SendSkillOperate(_skillID, FixIntVector3.zero, 0);
        }
        else if (_type == ESkillReleaseType.TargetPoint)
        {
            HeroActor heroActor = ActorManager.GetSelfHero();
            FixIntVector3 dir = new FixIntVector3(vector.x, 0, vector.y);
            FixIntVector3 targetDir = RotateY45(dir);

            DTSkill table = DataTable.GetItem<DTSkill>(_skillID);
            SkillConfig skillConfig = AssetSystem.Get<SkillConfig>($"Assets/GameAssets/So/Skill/{table.Config}.asset");
            FixIntVector3 pos = targetDir * skillConfig.SelectArea * 0.001f * 0.5f + heroActor.Position;

            OperateSystem.SendSkillOperate(_skillID, pos, 0);
        }
        else if (_type == ESkillReleaseType.VectorSkill)
        {
            FixIntVector3 dir = new FixIntVector3(vector.x, 0, vector.y);
            FixIntVector3 targetDir = RotateY45(dir);

            OperateSystem.SendSkillOperate(_skillID, targetDir, 0);
        }
        else
        {
            Debug.LogError($"位置技能释放类型,Type: {_type}");
        }
    }

    private void OnDrag(Vector2 vector)
    {
        Debug.Log($"调整技能方向, Vet:{vector}");

        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, vector, area1, area2));
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
        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, _joystick.Direction, area1, area2));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameEntry.Event.Trigger(new EventShowSkillIndicator(ESkillReleaseType.NoTarget, Vector2.zero, 0, 0));
    }
}
