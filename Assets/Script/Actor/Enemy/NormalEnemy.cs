using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
	[SerializeField]
	private float speed;//移動速度
	[SerializeField]
	private float rotateSpped;//回転速度


	/// <summary>
	/// 更新
	/// </summary>
	protected override void EnemyUpdate()
	{
		base.EnemyUpdate();
		Move();
		Rotate();
	}

	/// <summary>
	/// 移動
	/// </summary>
	private void Move()
	{
		if (isStan) return;

		transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);
	}

	/// <summary>
	/// プレイヤーの方向を向く
	/// </summary>
	private void Rotate()
	{
		if (isStan) return;

		float angle = (Mathf.Atan2(-playerVec.y, -playerVec.x) * Mathf.Rad2Deg) - 90.0f;
		Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//プレイヤーの方向を設定
		transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpped * Time.deltaTime);//プレイヤーの方向にゆっくり向く
	}
}
