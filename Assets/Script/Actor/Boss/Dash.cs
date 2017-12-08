using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashInterval;//突撃間隔
    public float chargeTime;//突進の溜め時間
    public float dashSpeed;//突撃速度

    private GameObject player;
    private Rigidbody2D rigid;
    private MainCamera mainCamera;
    private float dashCount;
    private float chargeCount;
    private bool isDash;//突撃フラグ

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        rigid = GetComponent<Rigidbody2D>();
        dashCount = dashInterval;
        chargeCount = chargeTime;
        dashSpeed *= rigid.mass;
    }

    // Update is called once per frame
    void Update()
    {
        DashCount();
        DashMove();
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
                rigid.AddForce(-transform.up * dashSpeed, ForceMode2D.Impulse);
                isDash = true;
            }

            if (rigid.velocity.magnitude <= dashSpeed / (2f * rigid.mass) && isDash)
            {
                rigid.velocity = Vector2.zero;
                dashCount = dashInterval;
                chargeCount = chargeTime;
                isDash = false;
            }
        }
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Wall" && isDash)
        {
            mainCamera.SetShake(true, 0.0f);
        }
    }

    /// <summary>
    /// 突撃開始フラグ
    /// </summary>
    /// <returns></returns>
    public bool IsDash()
    {
        return dashCount <= 0.0f;
    }
}
