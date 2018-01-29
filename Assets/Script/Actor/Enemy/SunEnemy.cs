using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunEnemy : Enemy
{
    public float setSpeed;//発射位置に向かう速度
    public float spawnSpeed;//生成時の移動速度
    public float startTime;//行動開始時間

    private Rigidbody2D rigid;
    private FollowPlayer followClass;//プレイヤー追従クラス
    private Vector2 shootPos;//発射位置
    private Vector2 spawnDirec;//生成時に移動する方向
    private bool isShootSet;//発射準備フラグ
    private bool isShoot;//発射フラグ
    private bool isStart;//行動開始フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        isShootSet = false;
        isShoot = false;

        followClass = GetComponent<FollowPlayer>();
        rigid = GetComponent<Rigidbody2D>();

		ai_classes[0].enabled = false;

		float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        spawnDirec = new Vector2(x, y);
        rigid.AddForce(spawnDirec * spawnSpeed, ForceMode2D.Impulse);

        isStart = false;
    }

	public void SetShootReset()
	{
		isShootSet = false;
		isShoot = false;
		followClass.enabled = true;
		followClass.ReMove();
		GetComponent<Collider2D>().enabled = true;
	}

	/// <summary>
	/// 更新
	/// </summary>
	public override void EnemyUpdate()
    {
        startTime -= Time.deltaTime;
        if (!isStart && startTime <= 0.0f)
        {
            rigid.velocity = Vector2.zero;
			isStart = true;
        }

        MoveToPoint();
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
        if (isShootSet)
        {
            GetComponent<FollowPlayer>().MoveStop();
            GetComponent<Collider2D>().enabled = false;
        }
        //if (GetComponent<Dash>().IsActive && ai_classes[0].enabled)
        //{
        //    GetComponent<Collider2D>().enabled = true;
        //    ai_classes[1].Stop();
        //    ai_classes[2].Stop();
        //}
		
        if (isStan)
        {
            gameObject.layer = 0;
            isShootSet = false;
            foreach (var ai in ai_classes)
            {
              //  ai.Stop();
            }
        }
    }

    /// <summary>
    /// 発射位置に向かう
    /// </summary>
    private void MoveToPoint()
    {
        if (!isShootSet || isShoot) return;

        rigid.velocity = Vector2.zero;
        transform.position = Vector2.Lerp(transform.position, shootPos, setSpeed * Time.deltaTime);
        float distance = Vector2.Distance(transform.position, shootPos);
        if (distance <= 0.5f)
        {
            transform.position = shootPos;
            isShoot = true;
        }
    }

    /// <summary>
    /// 発射準備
    /// </summary>
    public void SetShoot(Vector2 shootPos)
    {
		if (isStan)
		{
			return;
		}
		//DebugCommand.DebugLog(gameObject.name);
        this.shootPos = shootPos;
        isShootSet = true;
    }

    /// <summary>
    /// 発射準備フラグ
    /// </summary>
    public bool IsShootSet
    {
        get { return isShootSet; }
    }

    /// <summary>
    /// 発射フラグ
    /// </summary>
    public bool IsShoot
    {
        get { return isShoot; }
    }
}
