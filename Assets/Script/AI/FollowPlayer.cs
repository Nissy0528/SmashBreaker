using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : AI
{
    public float rotateSpeed;//回転速度
    public float speed;//移動速度
    public bool isReverse;//反転フラグ

    private GameObject player;//プレイヤー
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 lookPos;//見る方向
    private float currentSpeed;//初期速度
    private float angle;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        player = GameObject.Find("Player");//プレイヤーを探す
        currentSpeed = speed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void AIUpdate()
    {
        Move();
        Rotate();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.Translate(-transform.up * currentSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    private void Rotate()
    {
        lookPos = player.transform.position;//向く方向の座標
        playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        if (!isReverse)
        {
            angle = (Mathf.Atan2(-playerVec.y, -playerVec.x) * Mathf.Rad2Deg) - 90.0f;
        }
        else
        {
            angle = (Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg) - 90.0f;
        }
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//プレイヤーの方向を設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpeed * Time.deltaTime);//プレイヤーの方向にゆっくり向く
    }

    /// <summary>
    /// 反転
    /// </summary>
    public void Rivers()
    {
        currentSpeed *= -1;
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    public void MoveStop()
    {
        currentSpeed = 0.0f;
    }

    /// <summary>
    /// 移動再開
    /// </summary>
    public void ReMove()
    {
        currentSpeed = speed;
    }
}
