using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss : Enemy
{
    public float shoot_speed;//吹き飛ぶ速度
    public GameObject smashText;//スマッシュUI

    private PlayerHP playerHP;

    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField]
    private int speed = 2;

    /// <summary>
    ///耐力
    /// </summary>
    [SerializeField]
    private float bytal = 10;

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    private float rotSpeed = 30f;

    /// <summary>
    /// 反転
    /// </summary>
    public bool rivers;

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
        Move();

        if (bytal <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (isStan) return;

        Vector3 rotation = Vector3.forward * rotSpeed * Time.deltaTime;
        var riv = rivers ? -1f : 1f;
        transform.Rotate(rotation * riv);

        GetComponent<Rigidbody2D>().MovePosition(transform.position + ((transform.rotation * Vector3.forward) * speed));
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
