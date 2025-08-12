using System.Collections;
using System.Collections.Generic;
using BindableUI.Runtime;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class HPBar : MonoBehaviour
{
    public LogicActor Actor { get; private set; }
    [SerializeField] SpriteRenderer _sprite;

    public void Init(LogicActor actor)
    {
        Actor = actor;
        transform.localPosition = Vector3.zero;
        actor.AttributeSet.HPAttribute.Subscribe(OnHpChange);
    }

    void OnDestroy()
    {
        Actor?.AttributeSet.HPAttribute.Unsubscribe(OnHpChange);
        Actor = null;
    }

    private void OnHpChange(long value)
    {
        float percentage = value * 1f / Actor.AttributeSet.HPAttribute.Max;
        _sprite.size = new Vector2(percentage, 1);
    }
}