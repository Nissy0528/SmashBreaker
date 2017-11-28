using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private GameObject smashText;//スマッシュUI
    private SmashGage playerSP;//プレイヤー体力UI

    // Use this for initialization
    void Start()
    {
        playerSP = GameObject.Find("SmashGage").GetComponent<SmashGage>();
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
    public override void TriggerEnter(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack" && playerSP.IsMax)
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
