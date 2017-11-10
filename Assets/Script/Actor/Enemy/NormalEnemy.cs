using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public GameObject bonusText;
    public float speed;//移動速度
    public float shoot_speed;//吹き飛ぶ速度
    public float rotateSpped;//回転速度

    // Use this for initialization
    void Start()
    {
        shootSpeed = shoot_speed;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyUpdate();
        Move();//移動
        Rotate();//プレイヤーの方向を向く
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (isStan) return;

        transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    private void Rotate()
    {
        if (isStan) return;

        float angle = (Mathf.Atan2(-playerVec.y, -playerVec.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//プレイヤーの方向を設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpped * Time.deltaTime);//プレイヤーの方向にゆっくり向く
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    public override void TriggerEnter(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack")
        {
            GetComponent<BoxCollider2D>().isTrigger = true;//あたり判定のトリガーオン
            Shoot(col.gameObject);
        }

        //吹き飛ばされた敵に当たったら消滅
        if (col.transform.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<Enemy>().IsStan && !isStan)
            {
                GameObject text = Instantiate(bonusText);
                text.GetComponent<TextUI>().SetPos(transform.position);
                player.GetComponent<Player>().AddSP(2);//プレイヤーのスマッシュポイント加算
                Destroy(gameObject);
            }
        }
    }
}
