using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowDrillEnemy : Enemy
{
    private Rigidbody2D rigid;
    private Vector2 currentRV;//移動ベクトル
    private GrowBullet bulletClass;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(transform.up * speed, ForceMode2D.Impulse);
        currentRV = rigid.velocity;
        bulletClass = GetComponent<GrowBullet>();
        bulletClass.enabled = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void EnemyUpdate()
    {
        point = (int)transform.localScale.x / 4;
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
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
}
