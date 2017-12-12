using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;//回転速度

    public float speed;

    private GameObject player;//プレイヤー
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 lookPos;//見る方向

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        player = GameObject.Find("Chara");//プレイヤーを探す
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Move();
        Rotate();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);
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
    /// 反転
    /// </summary>
    public void Rivers()
    {
        speed *= -1;
    }
}
