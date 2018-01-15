using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Enemy
{
    private Rigidbody2D rigid;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void EnemyUpdate()
    {
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
        if(isStan)
        {
            foreach (var ai in ai_classes)
            {
                ai.Stop();
            }
        }
    }
}
