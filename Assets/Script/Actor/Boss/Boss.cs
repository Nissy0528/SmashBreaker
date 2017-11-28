using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private GameObject smashText;//スマッシュUI
    private PlayerHP playerHP;//プレイヤー体力UI

    // Use this for initialization
    void Start()
    {
        playerHP = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
        smashText = GameObject.Find("SmashText");
        smashText.SetActive(false);
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
    protected override void TriggerEnter(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack" && playerHP.IsHPMax)
        {
            if (!isStan)
            {
                smashText.SetActive(true);
            }
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<CircleCollider2D>().isTrigger = true;//あたり判定のトリガーオン
            Shoot(col.gameObject);
        }
    }
}
