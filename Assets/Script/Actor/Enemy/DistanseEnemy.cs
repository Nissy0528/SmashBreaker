using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanseEnemy : Enemy
{
	
	[SerializeField]
	private float speed;//移動速度
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

		if (col.transform.tag == "Wall")
		{
			isWall = true;
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
