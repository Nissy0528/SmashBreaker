using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : Boss
{
    public float speed;//初期速度

    private Rigidbody2D rigid;
    private GameObject[] enemys;//フィールド上のオブジェクト

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody2D>();
        float x = 0f, y = 0f;
        while (x == 0f)
        {
            x = Random.Range(-1f, 1f);
        }
        while (y == 0f)
        {
            y = Random.Range(-1f, 1f);
        }
        Vector2 velocity = new Vector2(x, y);
        rigid.AddForce(velocity * speed, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void BossUpdate()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    ///// <summary>
    ///// 人工知能
    ///// </summary>
    //public override void AI()
    //{
    //    if (enemys.Length > 0)
    //    {
    //        ai_classes[0].Stop();
    //    }
    //    else
    //    {
    //        ai_classes[0].enabled = true;
    //    }

    //    if (HP <= 0 || isStan)
    //    {
    //        foreach (var ai in ai_classes)
    //        {
    //            ai.Stop();
    //        }
    //    }
    //}

    ///// <summary>
    ///// 硬直
    ///// </summary>
    //public override void Stan()
    //{
    //    if (!isStan) return;

    //    if (stanDelay == stanTime)
    //    {
    //        EnemyDead();
    //    }
    //    stanDelay -= Time.deltaTime;

    //    if (stanDelay <= 0.0f)
    //    {
    //        isStan = false;
    //        stanDelay = stanTime;
    //        foreach (var ai in ai_classes)
    //        {
    //            ai.enabled = true;
    //        }
    //    }
    //}

    ///// <summary>
    ///// 全雑魚敵消滅
    ///// </summary>
    //private void EnemyDead()
    //{
    //    foreach (var e in enemys)
    //    {
    //        e.GetComponent<Enemy>().Dead();
    //    }
    //}
}
