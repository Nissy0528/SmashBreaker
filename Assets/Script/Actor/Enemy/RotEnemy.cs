using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotEnemy : Enemy
{
	[SerializeField]
	private float speed;//移動速度
	[SerializeField]
	private float rotateSpped;//回転速度

	/// <summary>
	///旋回速度
	/// </summary>
	[SerializeField, Tooltip("旋回速度")]
	private float turnSpeed = 45f;

	/// <summary>
	/// 旋回を行う間合い(値以下の距離になるまで旋回)
	/// </summary>
	[SerializeField, Tooltip("旋回を行う間合い(値以下の距離になるまで旋回)")]
	private float turnDistanse = 8f;

	// Use this for initialization
	void Start()
	{
		Initialize();
	}
	private void Update()
	{
		// Update is called once per frame
		EnemyUpdate();

		Move();//移動

		Rotate();//プレイヤーの方向を向く
	}

	/// <summary>
	/// 移動
	/// </summary>
	private void Move()
	{
		if (isStan) return;

		TurnBack();

		transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);
	}

	/// <summary>
	/// 背後への旋回
	/// </summary>
	private void TurnBack()
	{
		//ターゲットの取得
		var target = FindObjectOfType<Smash>().transform;
		//ターゲットの背後へのベクトル
		var tBack = target.up * -1;
		Debug.DrawRay(player.transform.position, player.transform.position + (tBack * 5), Color.green, 100f);

		//ターゲットと自身間のベクトル
		var direction = (transform.position - target.position);

		var dot = Vector3.Angle(tBack, direction.normalized);
		var distanse = Vector3.Distance(transform.position, player.transform.position);
		bool isSkip = dot <= 10f | turnDistanse > distanse;
		if (isSkip)
		{
			return;
		}
		var cross = Vector3.Cross(direction, tBack).z;
		//旋回の左右判定
		var angle = turnSpeed * cross < 0f ? -1f : 1f;
		//回転軸
		Vector3 axis = transform.TransformDirection(Vector3.forward);
		transform.RotateAround(target.position, axis, angle);
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

	/// <summary>
	/// あたり判定
	/// </summary>
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
				player.GetComponent<Player>().AddSP(base.point * 2);//プレイヤーのスマッシュポイント加算
				Destroy(gameObject);
			}
		}
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
}