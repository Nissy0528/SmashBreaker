using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲内にプレイヤーがいるかで動きが変わる
/// </summary>
public class AreaEnemy : Enemy
{
	/// <summary>
	///	初期座標
	/// </summary>
	private Vector3 initPosition;

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
	///回転速度
	/// </summary>
	[SerializeField]
	private float rotateSpeed;

	/// <summary>
	/// 追跡距離
	/// </summary>
	[SerializeField]
	private float chaseDistanse;

	/// <summary>
	/// 状態管理
	/// </summary>
	private enum State
	{
		Wait,
		Chase,
		Return,
	}


	/// <summary>
	///状態
	/// </summary>
	private State stete;

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
		Chase();
	}


	/// <summary>
	///プレイヤー待ち 
	/// </summary>
	private void Wait()
	{
		if (stete == State.Wait)
		{
			return;
		}
		var col = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));

		///初期座標へ
		var initVec = (initPosition - transform.position).normalized;//向く方向を正規化

		//回転
		float angle = (Mathf.Atan2(-initVec.y, -initVec.x) * Mathf.Rad2Deg) - 90.0f;
		transform.rotation = Quaternion.Euler(Vector3.forward * angle);//プレイヤーの方向にゆっくり向く
																	   //移動
		transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);

		if (col.gameObject != null)
		{
			ChangeState(State.Chase);
		}
	}

	/// <summary>
	/// 状態変化
	/// </summary>
	/// <param name="st"></param>
	private void ChangeState(State st)
	{
		stete = st;
	}

	/// <summary>
	/// 移動
	/// </summary>
	private void Chase()
	{
		if (!isFind || isStan)
		{
			return;
		}

		Rotate();

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
		transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpeed * Time.deltaTime);//プレイヤーの方向にゆっくり向く
	}


	/// <summary>
	/// 死亡時
	/// </summary>
	protected override void Dead()
	{
		base.Dead();
	}
}
