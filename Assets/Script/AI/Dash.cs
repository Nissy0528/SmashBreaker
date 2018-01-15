using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : AI
{
    public float dashInterval;//突撃間隔
    public float chargeTime;//突進の溜め時間
    public float dashSpeed;//突撃速度
    public float rotateSpeed;//回転速度
    public int frashTime;//点滅時間
    public bool isWall;//trueならダッシュ中に壁に当たったら消滅
    public bool isStart;//行動開始フラグ

    private GameObject player;
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 lookPos;//見る方向
    private Rigidbody2D rigid;
    private MainCamera mainCamera;
    private float dashCount;
    private float chargeCount;
    private int frashCnt;
    private bool isDash;//突撃フラグ

    // Use this for initialization
    public override void Initialize()
    {
        player = FindObjectOfType<Player>().gameObject;
        mainCamera = FindObjectOfType<MainCamera>();
        rigid = GetComponent<Rigidbody2D>();
        dashCount = dashInterval;
        chargeCount = chargeTime;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        Color color = sprite.material.color;
        color.a = 1.0f;
        sprite.material.color = color;
    }

    // Update is called once per frame
    protected override void AIUpdate()
    {
        if (!isStart) return;

        DashCount();
        DashMove();
        if (Time.timeScale > 0.0f)
        {
            Frash();
        }
    }

    /// <summary>
    /// 突撃カウント
    /// </summary>
    private void DashCount()
    {
        if (dashCount <= 0.0f)
        {
            return;
        }

        dashCount -= Time.deltaTime;
    }

    /// <summary>
    /// 突撃
    /// </summary>
    private void DashMove()
    {
        if (dashCount > 0.0f) return;

        chargeCount -= Time.deltaTime;
        if (chargeCount <= 0.0f)
        {
            if (rigid.velocity.magnitude == 0.0f && !isDash)
            {
                rigid.AddForce(-transform.up * (dashSpeed * rigid.mass), ForceMode2D.Impulse);
                isDash = true;
            }

            if (rigid.velocity.magnitude <= (dashSpeed * rigid.mass) / (2f * rigid.mass) && isDash)
            {
                rigid.velocity = Vector2.zero;
                dashCount = dashInterval;
                chargeCount = chargeTime;
                isDash = false;
            }
        }
        else
        {
            Rotate();
        }
    }

    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    private void Rotate()
    {
        lookPos = player.transform.position;//向く方向の座標
        playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        float angle = (Mathf.Atan2(-playerVec.y, -playerVec.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//プレイヤーの方向を設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpeed * Time.deltaTime);//プレイヤーの方向にゆっくり向く
    }

    /// <summary>
    /// 点滅
    /// </summary>
    private void Frash()
    {
        if (dashCount > 0.0f)
        {
            frashCnt = 0;
            return;
        }

        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        Color color = sprite.material.color;

        frashCnt += 1;
        color.a = (frashCnt / frashTime) % 2;
        if (chargeCount <= 0.0f)
        {
            color.a = 1.0f;
        }
        sprite.material.color = color;
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isDash) return;

        if (col.transform.tag == "Wall")
        {
            if (tag == "Boss")
            {
                mainCamera.SetShake(true, 0.0f);
            }
            if (isWall)
            {
                mainCamera.SetShake(true, 0.0f);
                GetComponent<Enemy>().Dead();
            }
        }

        ///水晶に当たった場合の処理
        if (col.transform.tag == "Attack")
        {
            Smash smash = FindObjectOfType<Smash>();
            if (smash.IsAttack) return;

            var p = player.GetComponent<Player>();
            p.SetBrown();
        }
    }

    /// <summary>
    /// 突撃開始フラグ
    /// </summary>
    /// <returns></returns>
    public override bool IsActive
    {
        get
        {
            return dashCount <= 0.0f;
        }
    }
}
