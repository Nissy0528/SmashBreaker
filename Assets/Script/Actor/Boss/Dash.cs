using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashInterval;//突撃間隔
    public float chargeTime;//突進の溜め時間
    public float dashSpeed;//突撃速度
    public float rotateSpeed;//回転速度
    public int frashTime;//点滅時間

    private GameObject player;
    private GameObject chara;
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 lookPos;//見る方向
    private Rigidbody2D rigid;
    private MainCamera mainCamera;
    private float dashCount;
    private float chargeCount;
    private bool isDash;//突撃フラグ

    // Use this for initialization
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        chara = transform.Find("Chara").gameObject;
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
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Wall" && isDash)
        {
            mainCamera.SetShake(true, 0.0f);
        }

		///水晶に当たった場合の処理
		if(col.transform.tag == "Attack" && isDash)
		{
			var p = player.GetComponent<Player>();
			p.SetBrown();
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
