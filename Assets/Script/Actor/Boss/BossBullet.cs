using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossBullet : MonoBehaviour
{
	/// <summary>
	/// 速度
	/// </summary>
	[SerializeField]
	protected float speed = 5f;

	// Use this for initialization
	void Start()
	{
		GetComponent<Rigidbody2D>().gravityScale = 0;
		BulletInit();
	}
	/// <summary>
	/// 初期化
	/// </summary>
	protected virtual void BulletInit() { }

	// Update is called once per frame
	void Update()
	{
		BulletUpdate();
	}

	/// <summary>
	///更新
	/// </summary>
	protected virtual void BulletUpdate() { }

	/// <summary>
	/// 移動
	/// </summary>
	protected virtual void Move()
	{
		var moved = transform.position + (transform.rotation * Vector3.up * speed * Time.deltaTime);
		GetComponent<Rigidbody2D>().MovePosition(moved);
	}



	/// <summary>
	/// 当たり判定
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Collision(collision);
	}

	protected virtual void Collision(Collision2D col) { }

}
