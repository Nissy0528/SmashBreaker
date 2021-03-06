﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    public struct Parameter
    {
        public float length;//飛ぶ距離
        public float smashSpeed;//拳を飛ばす速度
        public int maxScale;//ワンパン状態の拳の大きさ
        public int power;//拳の威力
    }

    public Player player;//プレイヤー
    public SmashGage playerSP;//プレイヤーのスマッシュゲージ
    public GameObject frash;

    private Parameter parameter;//パラメータ
    private GameObject smash;//攻撃オブジェクト
    private MainCamera mainCamera;//カメラクラス
    private Vector3 returnPos;//戻る座標
    private Vector3 offset;//プレイヤーとの距離
    private Vector3 moveToPos;//飛ぶ方向
    private Vector3 smashIniScale;
    private float angle;
    private bool isAttack;//攻撃フラグ
    private bool isReturn;//戻るフラグ

    // Use this for initialization
    void Start()
    {
        smash = transform.GetChild(0).gameObject;//攻撃オブジェクト取得
        smashIniScale = transform.localScale;
        mainCamera = FindObjectOfType<MainCamera>();
        isAttack = false;
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsDead()) return;//プレイヤーが死亡状態なら何もしない

        Attack();//攻撃
        Move();//移動
        Rotate();//回転
        ChangeScale();//拳の大きさ変更
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);//入力された方向に向く
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        if (isAttack) return;

        //攻撃コマンドが入力されたら押下フラグをtureに
        if (Input.GetMouseButtonDown(0) || Input.GetAxisRaw("Smash") >= 0.5f)
        {
            smash.GetComponent<CircleCollider2D>().isTrigger = true;//あたり判定を有効に
            moveToPos = smash.transform.position + smash.transform.up * parameter.length;//攻撃を飛ばす方向を設定
            isAttack = true;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (!isAttack) return;//攻撃フラグがfalseならこれ以降何もしない

        smash.transform.position = Vector3.MoveTowards(smash.transform.position, moveToPos, parameter.smashSpeed * Time.deltaTime);//設定された方向に平行移動
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
                player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                isReturn = true;
                return;
            }
            //戻るフラグがtrueなら攻撃フラグと戻るフラグをfalseに
            if (isReturn && (Input.GetButtonUp("Smash") || Input.GetAxisRaw("Smash") == 0.0f))
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
        if (isAttack || Time.timeScale == 0.0f) return;//攻撃中なら何もしない

        //右スティックの入力値を取得
        float x_axis = Input.GetAxisRaw("Smash_H");
        float y_axis = Input.GetAxisRaw("Smash_V");

        //スティックが倒された方向に向く
        if (x_axis >= 0.5f || x_axis <= -0.5f
            || y_axis >= 0.5f || y_axis <= -0.5f)
        {

            Vector3 lookPos = new Vector3(transform.position.x + x_axis, transform.position.y + y_axis * -1, 0);//向く方向の座標
            Vector3 vec = (lookPos - transform.position).normalized;//向く方向を正規化
            angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        }
    }

    /// <summary>
    /// 拳の大きさ変更
    /// </summary>
    private void ChangeScale()
    {
        //if (!playerSP.IsMax)
        //{
        float rate = playerSP.SpRate;

        transform.localScale = new Vector3(
           smashIniScale.x + parameter.maxScale * rate,
           smashIniScale.y + parameter.maxScale * rate,
            1);
        //}
        //else
        //{
        //    transform.localScale = new Vector3(parameter.maxScale, parameter.maxScale, 1);
        //}
    }

    /// <summary>
    /// 攻撃フラグ
    /// </summary>
    public bool IsAttack
    {
        get { return isAttack; }
    }

    /// <summary>
    /// パラメータ取得
    /// </summary>
    public Parameter GetParam
    {
        get { return parameter; }
    }

    /// <summary>
    /// パラメータ設定
    /// </summary>
    public void SetParam(float value, int i)
    {
        switch (i)
        {
            case 6:
                parameter.length = value;
                break;
            case 7:
                parameter.smashSpeed = value;
                break;
            case 8:
                parameter.maxScale = (int)value;
                break;
            case 9:
                parameter.power = (int)value;
                break;
            default:
                break;

        }
    }

    /// <summary>
    ///　敵に当たったときの処理
    /// </summary>
    public void Hit(string colTag)
    {
        if (playerSP.IsMax && colTag == "Boss")
        {
            mainCamera.SetShake(false, 0.5f);
            player.AddSP(0, true);
            FindObjectOfType<GameManager>().ShakeController(1.0f, 0.3f);
            Instantiate(frash, GameObject.Find("Canvas").transform);
        }
        isReturn = true;
        moveToPos = returnPos;//もといた座標に戻る
        smash.GetComponent<CircleCollider2D>().isTrigger = false;//あたり判定を有効に
    }
}
