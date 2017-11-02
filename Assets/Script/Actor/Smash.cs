using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    public GameObject player;//プレイヤー
    public float smashSpeed;//拳を飛ばす速度
    public float smashLength;//飛ぶ距離

    private GameObject smash;//攻撃オブジェクト
    private GameObject smashCol;//攻撃あたり判定
    private Vector3 returnPos;//戻る座標
    private Vector3 offset;//プレイヤーとの距離
    private Vector3 moveToPos;//飛ぶ方向
    private bool isAttack;//攻撃フラグ
    private bool isReturn;//戻るフラグ

    // Use this for initialization
    void Start()
    {
        smash = transform.Find("Smash").gameObject;//攻撃オブジェクト取得
        smashCol = smash.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().IsDead()) return;//プレイヤーが死亡状態なら何もしない

        Attack();//攻撃
        Move();//移動
        Rotate();//回転
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        if (isAttack) return;

        //攻撃コマンドが入力されたら攻撃フラグをtureに
        if (Input.GetButtonDown("Smash") || Mathf.Abs(Input.GetAxisRaw("Smash")) >= 0.5f)
        {
            smashCol.GetComponent<CircleCollider2D>().enabled = true;
            moveToPos = smash.transform.position + smash.transform.up * smashLength;//攻撃を飛ばす方向を設定
            offset = smash.transform.position - transform.position;//攻撃オブジェクトとの距離設定
            isAttack = true;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position = player.transform.position;//常にプレイヤーに追従
        if (!isAttack) return;//攻撃フラグがfalseならこれ以降何もしない

        smash.transform.position = Vector3.MoveTowards(smash.transform.position, moveToPos, smashSpeed * Time.deltaTime);//設定された方向に平行移動
        returnPos = transform.position + offset;//戻る座標設定

        //戻るフラグがtureなら
        if (isReturn)
        {
            moveToPos = returnPos;//もといた座標に戻る
            //ある程度戻ったら強制的に元の座標に
            if (Vector3.Distance(smash.transform.position, moveToPos) <= 0.15f)
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
                smashCol.GetComponent<CircleCollider2D>().enabled = false;
                isReturn = true;
            }
            //戻るフラグがtrueなら攻撃フラグと戻るフラグをfalseに
            else
            {
                isAttack = false;
                isReturn = false;
            }
        }
    }

    /// <summary>
    /// 発射方向
    /// </summary>
    private void Rotate()
    {
        if (isAttack) return;//攻撃中なら何もしない

        //右スティックの入力値を取得
        float x_axis = Input.GetAxisRaw("Smash_H");
        float y_axis = Input.GetAxisRaw("Smash_V");

        //スティックが倒された方向に向く
        if (x_axis >= 0.5f || x_axis <= -0.5f
            || y_axis >= 0.5f || y_axis <= -0.5f)
        {

            Vector3 lookPos = new Vector3(transform.position.x + x_axis, transform.position.y + y_axis * -1, 0);//向く方向の座標
            Vector3 vec = (lookPos - transform.position).normalized;//向く方向を正規化
            float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);//入力された方向に向く
        }
    }
}
