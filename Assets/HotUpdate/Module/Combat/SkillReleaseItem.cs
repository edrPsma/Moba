using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillReleaseItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] ESkillReleaseType _type;
    [SerializeField] Button _button;
    [SerializeField] Joystick _joystick;
    [SerializeField] GameObject _bgObj;
    [SerializeField] GameObject _handleObj;

    [SerializeField] float area1;
    [SerializeField] float area2;

    void Start()
    {
        _joystick.OnDragEvent += OnDrag;
        _joystick.OnDragEndEvent += OnDragEnd;
        _joystick.OnDragStartEvent += OnDragStart;
    }

    public void Refresh(int skillID)
    {
        //TODO 读取技能配置表
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
    }

    private void OnDrag(Vector2 vector)
    {
        Debug.Log($"调整技能方向, Vet:{vector}");
        GameEntry.Event.Trigger(new EventShowSkillIndicator(_type, vector, area1, area2));
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
