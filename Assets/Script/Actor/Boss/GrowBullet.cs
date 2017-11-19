using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrowBullet : BossBullet
{
	/// <summary>
	/// 大きくなるスピード
	/// </summary>
	[SerializeField]
	private float growSpeed = 4;

	/// <summary>
	/// 弾丸初期化
	/// </summary>
	protected override void BulletInit()
	{
		base.BulletInit();
		GetComponent<Rigidbody2D>().freezeRotation = true;
	}

	/// <summary>
	/// 弾丸更新
	/// </summary>
	protected override void BulletUpdate()
	{
		Growing();

		Move();
	}


	/// <summary>
	/// 成長処理
	/// </summary>
	private void Growing()
	{
		transform.localScale += new Vector3(1, 1, 0) * (growSpeed * Time.deltaTime);
	}

	/// <summary>
	/// 当たり判定
	/// </summary>
	/// <param name="col"></param>
	protected override void Collision(Collider2D col)
	{
		string layer = LayerMask.LayerToName(col.gameObject.layer);
		if (layer == "Wall")
		{
			Destroy(gameObject);
		}
		if (layer == "Player")
		{
			var p = col.gameObject.GetComponent<Player>();
			Destroy(gameObject);
			p.Damage();
		}
		//base.Collision(col);
	}
}
