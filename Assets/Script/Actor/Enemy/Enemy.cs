using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected GameObject bonusText;
    [SerializeField]
    protected int point;//倒されたときのポイント
    [SerializeField]
    protected float shootSpeed;//吹き飛ぶ速度
    [SerializeField]
    protected float speed;//移動速度

    protected GameObject player;//プレイヤー
    protected Vector3 playerVec;//プレイヤーの方向
    protected Vector3 lookPos;//見る方向
    protected bool isStan;//気絶フラグ

    private MainCamera mainCamera;//カメラ
    private Collider2D col;
    private Vector3 size;//サイズ

    // Use this for initialization
    void Start()
    {
        Initialize();
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        player = GameObject.Find("Chara");//プレイヤーを探す
        isStan = false;
        size = transform.localScale;
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyUpdate();
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected virtual void EnemyUpdate()
    {
        lookPos = player.transform.position;//向く方向の座標
        playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        Dead();//消滅
    }

    /// <summary>
    /// 消滅
    /// </summary>
    protected virtual void Dead()
    {
        if (!isStan) return;

        Vector3 pos = transform.position;
        Vector3 screenMinPos = mainCamera.ScreenMin;//画面の左下の座標
        Vector3 screenMaxPos = mainCamera.ScreenMax;//画面の右下の座標

        //画面外に出たら消滅
        if (pos.x <= screenMinPos.x - size.x / 2 || pos.x >= screenMaxPos.x + size.x / 2
            || pos.y <= screenMinPos.y - size.y / 2 || pos.y >= screenMaxPos.y + size.y / 2)
        {
            mainCamera.SetShake(true, 0.0f);//画面振動
            Destroy(gameObject);//消滅
        }
    }

    /// <summary>
    /// 吹き飛ぶ
    /// </summary>
    public virtual void Shoot()
    {
        if (isStan) return;

        //Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        //rigid.bodyType = RigidbodyType2D.Dynamic;
        //rigid.mass = 1.0f;
        //rigid.AddForce(-playerVec * shootSpeed, ForceMode2D.Impulse);//後ろに吹き飛ぶ
        //isStan = true;//気絶フラグtrue
        //player.GetComponent<Player>().AddSP(point);//プレイヤーのスマッシュポイント加算
        //Time.timeScale = 0.0f;//ゲーム停止
        //col.isTrigger = true;//あたり判定のトリガーオン
        Destroy(gameObject);
        mainCamera.Stop();
    }

    /// <summary>
    /// 気絶フラグ
    /// </summary>
    public bool IsStan
    {
        get { return isStan; }
    }

    ///// <summary>
    ///// あたり判定（トリガー）
    ///// </summary>
    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
    //    if (col.transform.tag == "Player" && col.GetComponent<Player>().IsState(Player.State.DASH))
    //    {
    //        Shoot();
    //    }
    //}
    //void OnTriggerStay2D(Collider2D col)
    //{ 
    //    //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
    //    if (col.transform.tag == "Player" && col.GetComponent<Player>().IsState(Player.State.DASH))
    //    {
    //        Shoot();
    //    }
    //}

    ///// <summary>
    ///// あたり判定
    ///// </summary>
    ///// <param name="col"></param>
    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
    //    if (col.transform.tag == "Player" && col.gameObject.GetComponent<Player>().IsState(Player.State.DASH))
    //    {
    //        Shoot();
    //    }
    //}
    //void OnCollisionStay2D(Collision2D col)
    //{
    //}
}
