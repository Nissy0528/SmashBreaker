using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanseEnemy : Enemy
{
	[SerializeField]
	private float rotateSpped;//回転速度

	/// <summary>
	/// 保ちたい距離 
	/// </summary>
	[SerializeField]
	private float keepDistanse = 4.5f;

	/// <summary>
	/// 壁の判定
	/// </summary>
	private bool isWall;

	protected override void EnemyUpdate()
	{
		base.EnemyUpdate();
		Move();
		Rotate();
	}
	private float angle = 15f;
	private float count = 0f;
	/// <summary>
	/// 移動
	/// </summary>
	private void Move()
	{
		if (isStan) return;

		var distanse = Vector3.Distance(transform.position, player.transform.position);

		if (keepDistanse != distanse)
		{
			var direction = (transform.position - player.transform.position).normalized;
			var moved = (keepDistanse - distanse) * speed * Time.deltaTime;
			transform.Translate(direction * moved, Space.World);
		}

		//var ang = angle * Mathf.Sin(count);
		//count += Time.deltaTime;
		////回転軸
		//Vector3 axis = transform.TransformDirection(Vector3.forward);
		//transform.RotateAround(player.transform.position, axis, ang * Time.deltaTime);
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
