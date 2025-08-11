using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/DamageMarkConfig", fileName = "DamageMarkConfig", order = 1)]
public class DamageMarkConfig : ScriptableObject
{
    [Serializable]
    public struct CustomScalePair
    {
        [VerticalGroup]
        public float TargetScale;

        [VerticalGroup]
        public float Duration;
    }

    [Header("Common")]
    public Color Color = new Color(0, 0, 0, 1);
    public float FontSize;
    public float Duration;

    [Header("MoveX")]
    public bool UseMoveX;
    [ShowIf(nameof(UseMoveX))] public float StartPosOffsetX;
    [ShowIf(nameof(UseMoveX))] public float EndPosOffsetX;
    [ShowIf(nameof(UseMoveX))] public AnimationCurve MoveXCurve;
    [ShowIf(nameof(UseMoveX))] public float MoveXDelay;
    [ShowIf(nameof(UseMoveX))] public float MoveXDuration;

    [Header("MoveY")]
    public bool UseMoveY;
    [ShowIf(nameof(UseMoveY))] public float StartPosOffsetY;
    [ShowIf(nameof(UseMoveY))] public float EndPosOffsetY;
    [ShowIf(nameof(UseMoveY))] public AnimationCurve MoveYCurve;
    [ShowIf(nameof(UseMoveY))] public float MoveYDelay;
    [ShowIf(nameof(UseMoveY))] public float MoveYDuration;

    [Header("Fade")]
    public bool UseFade;
    [ShowIf(nameof(UseFade))] public float StartFade;
    [ShowIf(nameof(UseFade))] public float EndFade;
    [ShowIf(nameof(UseFade))] public AnimationCurve FadeCurve;
    [ShowIf(nameof(UseFade))] public float FadeDelay;
    [ShowIf(nameof(UseFade))] public float FadeDuration;

    [Header("Scale")]
    public bool UseScale;
    [ShowIf(nameof(UseScale))] public bool UseCustomScaleValues;
    [ShowIf(nameof(UseScale))] public float StartScale;
    [HideIf("@!this.UseScale || this.UseCustomScaleValues")] public float EndScale;
    [ShowIf("@this.UseScale && this.UseCustomScaleValues")] public CustomScalePair[] CustomScaleValues;
    [ShowIf(nameof(UseScale))] public AnimationCurve ScaleCurve;
    [ShowIf(nameof(UseScale))] public float ScaleDelay;
    [HideIf("@!this.UseScale || this.UseCustomScaleValues")] public float ScaleDuration;
}
