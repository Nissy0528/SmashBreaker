using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float shoot_speed;//吹き飛ぶ速度
    public GameObject smashText;//スマッシュUI

    private PlayerHP playerHP;

    // Use this for initialization
    void Start()
    {
        shootSpeed = shoot_speed;
        playerHP = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyUpdate();
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    public override void TriggerEnter(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack" && playerHP.IsHPMax)
        {
            if (!isStan)
            {
                smashText.SetActive(true);
            }
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Shoot(col.gameObject);
        }
    }
}
