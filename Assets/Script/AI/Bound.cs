using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : AI
{
    public float speed;//移動速度

    private Vector2 v;//移動ベクトル
    private Vector2 refV;//マイナスの移動ベクトル
    private Vector2 n;//移動ベクトルの法線
    private Vector2 p;//vを投影したベクトル
    private bool isHit;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        v = new Vector2(x, y).normalized;
        refV = new Vector2(-v.x, v.y);
        isHit = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void AIUpdate()
    {
        transform.Translate(v * speed * Time.deltaTime);//移動
    }

    /// <summary>
    /// 反射ベクトル
    /// </summary>
    private Vector2 RefVector()
    {
        n = new Vector2(-v.y, v.x);//移動ベクトルの法線
        Vector2 nDush = n.normalized;//正規化
        float refVDotNorm = refV.x * nDush.x + refV.y * nDush.y;
        p = refVDotNorm * nDush;
        Vector2 vf = 2 * p + (-refV);

        return new Vector2(vf.x, vf.y).normalized;
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && !isHit)
        {
            string name = collision.gameObject.name;
            if (name == "Top" || name == "Under")
            {
                v.y *= -v.y;
            }
            if (name == "Left" || name == "Right")
            {
                v.x *= -v.x;
            }

            isHit = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && isHit)
        {
            isHit = false;
        }
    }
}
