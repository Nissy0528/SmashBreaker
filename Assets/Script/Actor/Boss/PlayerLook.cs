using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float speed;//速度

    private GameObject player;
    private Vector3 lookPos;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Chara");
        lookPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Look();//プレイヤーを見る
    }

    /// <summary>
    /// プレイヤーを見る
    /// </summary>
    private void Look()
    {
        lookPos = player.transform.position;//向く方向の座標
        Vector2 playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        float angle = (Mathf.Atan2(-playerVec.y, -playerVec.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//プレイヤーの方向を設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, speed * Time.deltaTime);//プレイヤーの方向にゆっくり向く
    }
}
