using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrowBullet : BossBullet
{
    /// <summary>
    /// 大きくなるスピード
    /// </summary>
    public float growSpeed = 4;
    public float growRange = 10;

    private Rigidbody2D rigid;
    private Vector3 vec;//向く方向

    /// <summary>
    /// 弾丸初期化
    /// </summary>
    protected override void BulletInit()
    {
        base.BulletInit();
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
        //GetComponent<CircleCollider2D>().isTrigger = true;
    }

    /// <summary>
    /// 弾丸更新
    /// </summary>
    protected override void BulletUpdate()
    {
        Growing();
        Rotate();
    }


    /// <summary>
    /// 成長処理
    /// </summary>
    private void Growing()
    {
        Vector3 size = transform.localScale;
        size += new Vector3(1, 1, 0) * (growSpeed * Time.deltaTime);
        size.x = Mathf.Clamp(size.x, 4.0f, growRange);
        size.y = Mathf.Clamp(size.y, 4.0f, growRange);
        transform.localScale = size;
    }

    /// <summary>
    /// 反射した方向に向く
    /// </summary>
    private void Rotate()
    {
        vec = rigid.velocity.normalized;//向く方向の座標
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.rotation = newRota;
    }

    private void Hit(Collision2D col)
    {
        string layer = LayerMask.LayerToName(col.gameObject.layer);
        Debug.Log(layer);
        if (layer == "Wall")
        {
            //Destroy(gameObject);
        }
        if (layer == "Player")
        {
            Destroy(gameObject);
            FindObjectOfType<PlayerDamage>().Damage();
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col"></param>
    protected override void Collision(Collision2D col)
    {
        Hit(col);
    }
}
