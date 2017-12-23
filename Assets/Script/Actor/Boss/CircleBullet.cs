using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : BossBullet
{
    private bool isPlayer;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void BulletInit()
    {
        base.BulletInit();
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void BulletUpdate()
    {
        Move();
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col"></param>
    protected override void Trigger(Collider2D col)
    {
        string layer = LayerMask.LayerToName(col.gameObject.layer);
        if (layer == "Wall")
        {
            Destroy(gameObject);
        }

        if (layer == "Player" && !isPlayer)
        {
            col.GetComponent<PlayerDamage>().Damage();
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public float Speed
    {
        set { speed = value; }
        get { return speed; }
    }


    public bool IsPlayer
    {
        set { isPlayer = value; }
        get { return isPlayer; }
    }
}
