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
		isFind = false;
	}

	/// <summary>
	/// 更新
	/// </summary>
	protected override void EnemyUpdate()
	{
		base.EnemyUpdate();
		Wait();
		Move();
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

		isFind = col.gameObject != null;

	}

	private void Move()
	{ 
		if(!isFind)
		{
			return;
		}

		Debug.Log("find");
	}

	/// <summary>
	/// 死亡時
	/// </summary>
	protected override void Dead()
	{
		base.Dead();
	}


	protected override void TriggerStay(Collider2D col)
	{
		//プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
		if (col.transform.tag == "Attack" && !GetComponent<BoxCollider2D>().isTrigger)
		{
			GetComponent<BoxCollider2D>().isTrigger = true;//あたり判定のトリガーオン
			Shoot(col.gameObject);
		}
	}

	protected override void TriggerEnter(Collider2D col)
	{
		//プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
		if (col.transform.tag == "Attack" && !GetComponent<BoxCollider2D>().isTrigger)
		{
			GetComponent<BoxCollider2D>().isTrigger = true;//あたり判定のトリガーオン
			Shoot(col.gameObject);
		}

		//吹き飛ばされた敵に当たったら消滅
		if (col.transform.tag == "Enemy")
		{
			if (col.gameObject.GetComponent<Enemy>().IsStan && !isStan)
			{
				GameObject text = Instantiate(bonusText);
				text.GetComponent<TextUI>().SetPos(transform.position);
				player.GetComponent<Player>().AddSP(point * 2);//プレイヤーのスマッシュポイント加算
				Destroy(gameObject);
			}
		}
	}

}
