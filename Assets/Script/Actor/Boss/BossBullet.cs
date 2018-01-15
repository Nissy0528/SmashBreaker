using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossBullet : AI
{
	/// <summary>
	/// 速度
	/// </summary>
    [SerializeField]
	protected float speed = 5f;

    // Use this for initialization
    public override void Initialize()
	{
		GetComponent<Rigidbody2D>().gravityScale = 0;
		BulletInit();
	}
	/// <summary>
	/// 初期化
	/// </summary>
	protected virtual void BulletInit() { }

    // Update is called once per frame
    protected override void AIUpdate()
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
	/// 当たり判定（トリガー）
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
        Trigger(collision);
    }

	protected virtual void Trigger(Collider2D col) { }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collision(collision);
    }

    protected virtual void Collision(Collision2D col) { }
}
