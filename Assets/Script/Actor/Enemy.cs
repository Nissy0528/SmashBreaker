using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;//移動速度
    public float shootSpeed;//吹き飛ぶ速度
    public float rotateSpped;//回転速度

    private MainCamera camera;//カメラ
    private GameObject player;//プレイヤー
    private Animator anim;//アニメーション
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 size;//サイズ
    private bool isStan;//気絶フラグ

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        player = GameObject.Find("Player");//プレイヤーを探す
        anim = GetComponent<Animator>();
        anim.SetBool("Run", true);
        isStan = false;
        size = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Move();//移動
        Rotate();//プレイヤーの方向を向く
        Dead();//消滅
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

        Vector3 lookPos = player.transform.position;//向く方向の座標
        playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        float angle = (Mathf.Atan2(-playerVec.y, -playerVec.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion newRota = Quaternion.Euler(0.0f, 0.0f, angle);//プレイヤーの方向を設定
        transform.rotation = Quaternion.Slerp(transform.rotation, newRota, rotateSpped * Time.deltaTime);//プレイヤーの方向にゆっくり向く
    }

    /// <summary>
    /// 消滅
    /// </summary>
    private void Dead()
    {
        if (!isStan) return;

        Vector3 pos = transform.position;
        Vector3 screenMinPos = camera.ScreenMin();//画面の左下の座標
        Vector3 screenMaxPos = camera.ScreenMax();//画面の右下の座標

        //画面外に出たら消滅
        if (pos.x <= screenMinPos.x - size.x / 2 || pos.x >= screenMaxPos.x + size.x / 2
            || pos.y <= screenMinPos.y - size.y / 2 || pos.y >= screenMaxPos.y + size.y / 2)
        {
            camera.SetShake();//画面振動
            Destroy(gameObject);//消滅
        }
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack" && !isStan)
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>().isTrigger = true;
            rigid.AddForce(-playerVec * shootSpeed, ForceMode2D.Impulse);
            anim.SetBool("Stan", true);
            isStan = true;
            col.transform.parent.GetComponent<Player>().ChangeHp(1);
            GameManager.GameStop();//ゲーム停止
        }
    }
}
