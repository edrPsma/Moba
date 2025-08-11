using Pool;
using UnityEngine;
using Zenject;


public interface IDamageMarkFactory
{
	void InitPool();

	void Dispose();

	/// <summary>
	/// 回收伤害标记
	/// </summary>
	/// <param name="damageMark"></param>
	void Release(DamageMark damageMark);

	/// <summary>
	/// 显示伤害标记
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="dir"></param>
	/// <param name="damageInfo"></param>
	void Show(Vector3 pos, float dir, ref DamageInfo damageInfo);

	/// <summary>
	/// 显示恢复标记
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="dir"></param>
	/// <param name="value"></param>
	void ShowRecover(Vector3 pos, float dir, long value);

	/// <summary>
	/// 显示伤害标记
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="dir"></param>
	/// <param name="value"></param>
	void ShowDamage(Vector3 pos, float dir, long value);
}

[Controller]
public class DamageMarkFactory : AbstarctController, IDamageMarkFactory
{
	GameObjectPool<DamageMark> _pool;

	public void InitPool()
	{
		GameObject recycleNode = new GameObject(nameof(DamageMarkFactory));
		recycleNode.SetActive(false);

		DamageMark damageMark = GameEntry.Scene.GetSceneState<GameScene>().DamageMark;
		_pool = new GameObjectPool<DamageMark>(EPoolType.Scalable, 30, damageMark,
		  clone => clone.SetParent(recycleNode), null);

		_pool.OnReleaseEvent += clone => clone.SetParent(recycleNode);
		_pool.OnGetEvent += clone => clone.transform.SetParent(null);
		damageMark.SetActive(false);
	}

	public void Dispose()
	{
		_pool.Dispose();
	}

	public void Show(Vector3 pos, float dir, ref DamageInfo damageInfo)
	{
		if (damageInfo.FinalDamageValue <= 0) return;

		_pool.SpawnByType().Show(pos, dir, ref damageInfo);
	}

	public void Release(DamageMark damageMark)
	{
		_pool.Release(damageMark);
	}

	public void ShowRecover(Vector3 pos, float dir, long value)
	{
		if (value <= 0) return;

		_pool.SpawnByType().ShowRecover(pos, dir, value);
	}

	public void ShowDamage(Vector3 pos, float dir, long value)
	{
		if (value <= 0) return;

		_pool.SpawnByType().ShowDamage(pos, dir, value);
	}
}
