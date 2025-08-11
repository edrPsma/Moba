using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Text;
using Task;
using System.Linq;
using Zenject;
using BindableUI.Runtime;

public class DamageMark : MonoView
{
	[Inject] public IDamageMarkFactory DamageMarkFactory;
	[Inject] public IAssetSystem AssetSystem;

	[SerializeField] private long _limitValue;
	[SerializeField] private TMP_Text _content;
	[SerializeField] private BindComponent _bind;

	List<Sequence> _tweenSeq = new List<Sequence>();

	public void Show(Vector3 pos, float dir, ref DamageInfo damageInfo)
	{
		string configName;
		if (damageInfo.IsSkill) configName = damageInfo.IsCriticalStrike ? nameof(EDamageMarkType.SkillCriticalStrike) : nameof(EDamageMarkType.Skill);
		else configName = damageInfo.IsCriticalStrike ? nameof(EDamageMarkType.CriticalStrike) : nameof(EDamageMarkType.Defult);

		Show(pos, dir, _bind.GetAsset<DamageMarkConfig>(configName), damageInfo.FinalDamageValue);
	}

	public void ShowRecover(Vector3 pos, float dir, long recover)
	{
		Show(pos, dir, _bind.GetAsset<DamageMarkConfig>(nameof(EDamageMarkType.Recover)), recover);
	}

	public void ShowDamage(Vector3 pos, float dir, long recover)
	{
		Show(pos, dir, _bind.GetAsset<DamageMarkConfig>(nameof(EDamageMarkType.Skill)), recover);
	}

	void Show(Vector3 pos, float dir, DamageMarkConfig config, long value)
	{
		//TODO 设置父节点
		// transform.SetParent(GameEntry.Prefab.SceneNode);
		transform.position = pos;

		SetText(config, value);
		MoveX(config, dir);
		MoveY(config);
		Fade(config);
		Scale(config);

		//TODO 设置速度
		GameEntry.Task.AddTask(DelayToRecycle)
		  .SetName("DamageMark DelayToRecycle")
		  .Delay(TimeSpan.FromSeconds(config.Duration))
		  .Run();
	}

	private void DelayToRecycle(TaskInfo taskInfo)
	{
		foreach (var item in _tweenSeq)
		{
			item.Kill();
		}

		_tweenSeq.Clear();
		DamageMarkFactory.Release(this);
	}

	void MoveX(DamageMarkConfig config, float dir)
	{
		if (!config.UseMoveX) return;

		Vector3 startPos = transform.position;

		transform.position = startPos + new Vector3(config.StartPosOffsetX, 0, 0) * dir;

		Sequence sequence = DOTween.Sequence();
		sequence
		  .Insert(config.MoveXDelay, transform.DOMoveX(startPos.x + config.EndPosOffsetX * dir, config.MoveXDuration))
		  .SetEase(config.MoveXCurve);

		_tweenSeq.Add(sequence);

		sequence.Play();
	}

	void MoveY(DamageMarkConfig config)
	{
		if (!config.UseMoveY) return;

		Vector3 startPos = transform.position;

		transform.position = startPos + transform.up * config.StartPosOffsetY;

		Sequence sequence = DOTween.Sequence();
		sequence
		  .Insert(config.MoveYDelay, transform.DOMoveY(startPos.y + config.EndPosOffsetY, config.MoveYDuration))
		  .SetEase(config.MoveYCurve);

		_tweenSeq.Add(sequence);

		sequence.Play();
	}

	void Fade(DamageMarkConfig config)
	{
		if (!config.UseFade) return;

		Sequence sequence = DOTween.Sequence();
		sequence.Append(_content.DOFade(config.StartFade, 0));
		sequence
		  .Insert(config.FadeDelay, _content.DOFade(config.EndFade, config.FadeDuration))
		  .SetEase(config.FadeCurve);

		_tweenSeq.Add(sequence);

		sequence.Play();
	}

	void Scale(DamageMarkConfig config)
	{
		if (!config.UseScale) return;

		Sequence sequence = DOTween.Sequence();
		var rawScale = _content.transform.localScale;
		if (!config.UseCustomScaleValues)
		{
			sequence
			  .Insert(config.ScaleDelay, _content.transform.DOScale(rawScale * config.EndScale, config.ScaleDuration).From(rawScale * config.StartScale, true))
			  .SetEase(config.ScaleCurve);
		}
		else
		{
			if (config.CustomScaleValues == null || config.CustomScaleValues.Length == 0) return;
			var customScaleValues = config.CustomScaleValues;
			var length = customScaleValues.Length;
			var tf = _content.transform;

			sequence.Append(tf.DOScale(rawScale * config.StartScale, 0));
			for (int i = 0; i < length; i++)
			{
				var pair = customScaleValues[i];

				if (i == 0)
				{
					sequence.Insert(config.ScaleDelay, tf.DOScale(rawScale * pair.TargetScale, pair.Duration));
					continue;
				}

				sequence.Append(tf.DOScale(rawScale * pair.TargetScale, pair.Duration));
			}

			sequence.SetEase(config.ScaleCurve);
		}

		_tweenSeq.Add(sequence);

		sequence.Play();
	}

	private void SetText(DamageMarkConfig config, long value)
	{
		var color16 = ColorUtility.ToHtmlStringRGBA(config.Color);

		// string content = value <= _limitValueGetter ? value.ToString() : value.FormatNumber();

		//TODO 数值处理
		string content = value.ToString();

		string dmgStr = GetFontText(content, color16);

		_content.text = dmgStr;
		_content.fontSize = config.FontSize;

		string GetFontText(string text, string color)
		{
			char[] arr = text.ToCharArray();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < arr.Length; i++)
			{
				sb.Append($"<sprite={GetFontIndex(arr[i])} color=#{color}>");
			}

			return sb.ToString();
		}

		int GetFontIndex(char c)
		{
			if (c >= '0' && c <= '9')
			{
				return c - 48;
			}
			else if (c >= 'A' && c <= 'Z')
			{
				return c - 65 + 11;
			}
			else
			{
				return 37;
			}
		}
	}
}