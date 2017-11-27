using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲内にプレイヤーがいるかで動きが変わる
/// </summary>
public class AreaEnemy : Enemy
{
	/// <summary>
	/// プレイヤー発見
	/// </summary>
	private bool isFind;

	/// <summary>
	/// 範囲の半径
	/// </summary>
	[SerializeField]
	private float radius;

	/// <summary>
	/// 初期化
	/// </summary>
	public override void Initialize()
	{
		base.Initialize();
	}

	/// <summary>
	/// 更新
	/// </summary>
	protected override void EnemyUpdate()
	{
		base.EnemyUpdate();
	}


	/// <summary>
	///プレイヤー待ち 
	/// </summary>
	private void Wait()
	{
		if (isFind)
		{
			return;
		}

		var col = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));

		isFind = col != null;

	}

	/// <summary>
	/// 死亡時
	/// </summary>
	protected override void Dead()
	{
		base.Dead();
	}
}
