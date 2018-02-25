using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSmash : MonoBehaviour
{
    public float smashSpeed;//拳を飛ばす速度
    public float length;//飛ぶ距離
    public float attackInterval;

    private GameObject smash;//攻撃オブジェクト
    private Vector3 returnPos;//戻る座標
    private Vector3 offset;//プレイヤーとの距離
    private Vector3 moveToPos;//飛ぶ方向
    private bool isAttack;//攻撃フラグ
    private bool isReturn;//戻るフラグ
    private int attackCount;//攻撃回数
    private float intervalCount;

    // Use this for initialization
    void Start()
    {
        smash = transform.GetChild(0).gameObject;//攻撃オブジェクト取得
        isAttack = false;
        isReturn = false;
        attackCount = 0;
        intervalCount = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Move();
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        if (isAttack || attackCount >= 2) return;

        intervalCount -= Time.deltaTime;

        if (intervalCount <= 0.0f)
        {
            smash.GetComponent<CircleCollider2D>().isTrigger = true;//あたり判定を有効に
            moveToPos = smash.transform.position + smash.transform.up * length;//攻撃を飛ばす方向を設定
            isAttack = true;
            attackCount++;
            intervalCount = attackInterval;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (!isAttack) return;//攻撃フラグがfalseならこれ以降何もしない

        smash.transform.position = Vector3.MoveTowards(smash.transform.position, moveToPos, smashSpeed * Time.deltaTime);//設定された方向に平行移動
        returnPos = transform.GetChild(1).transform.position;//戻る座標設定

        //戻るフラグがtureなら
        if (isReturn)
        {
            moveToPos = returnPos;//もといた座標に戻る
            //ある程度戻ったら強制的に元の座標に
            if (Vector3.Distance(smash.transform.position, moveToPos) <= 0.17f)
            {
                smash.transform.position = moveToPos;
            }
        }
        //設定した座標に到達したら
        if (smash.transform.position == moveToPos)
        {
            //戻るフラグがfalseならtureに
            if (!isReturn)
            {
                isReturn = true;
                return;
            }
            //戻るフラグがtrueなら攻撃フラグと戻るフラグをfalseに
            if (isReturn)
            {
                isAttack = false;
                isReturn = false;
            }
        }
    }

    /// <summary>
    /// 攻撃終了フラグ
    /// </summary>
    /// <returns></returns>
    public bool SmashEnd()
    {
        return !isAttack && attackCount >= 2;
    }
}
