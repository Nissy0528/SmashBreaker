using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Enemy
{
    public float[] bulletSpeed;//弾の速度

    private Rigidbody2D rigid;
    private Vector2 currentRV;//移動ベクトル
    private CircleBulletShooter bulletClass;//円状弾クラス
    private RazerShooter razerClass;//レーザークラス
    private int num;//ID番号
    private bool isStart;//行動開始フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        //初期加速
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        rigid.AddForce(transform.up * speed, ForceMode2D.Impulse);
        currentRV = rigid.velocity;

        bulletClass = GetComponent<CircleBulletShooter>();
        bulletClass.Stop();
        razerClass = GetComponent<RazerShooter>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void EnemyUpdate()
    {
        if (!isStart)
        {
            SetBulletSpeed();
            isStart = true;
        }
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
        if (!isStart) return;

        if (!razerClass.IsActive)
        {
            bulletClass.enabled = true;
            if (rigid.velocity == Vector2.zero)
            {
                Initialize();
            }
        }
        else
        {
            bulletClass.Stop();
            rigid.velocity = Vector2.zero;
            currentRV = rigid.velocity;
        }

        if (isStan)
        {
            foreach (var ai in ai_classes)
            {
                ai.Stop();
            }
        }
        else
        {
            rigid.velocity = currentRV;
        }
    }

    /// <summary>
    /// 弾の速度設定
    /// </summary>
    private void SetBulletSpeed()
    {
        //IDによって弾の移動速度を変える
        int n = num / 2;
        int speedNum = n % 2;
        bulletClass.bulletSpeed = bulletSpeed[speedNum];
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    public override void Collision(Collision2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Pillar")
        {
            currentRV = rigid.velocity;
        }
    }


    /// <summary>
    /// 番号設定
    /// </summary>
    /// <param name="num"></param>
    public void SetNumber(int num)
    {
        this.num = num;
    }
}
