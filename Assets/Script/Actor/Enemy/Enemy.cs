using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int point;//倒されたときのポイント
    [SerializeField]
    protected float shootSpeed;//吹き飛ぶ速度
    [SerializeField]
    protected float speed;//移動速度
    [SerializeField]
    protected int maxHp;//最大体力

    protected GameObject player;//プレイヤー
    protected bool isStan;//気絶フラグ

    public GameObject dead_effect;//死亡時エフェクト

    private MainCamera mainCamera;//カメラ
    private GameManager gameManager;//ゲーム管理クラス
    private Collider2D col;
    private SmashGage smashGage;
    private Vector3 size;//サイズ
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 lookPos;//見る方向
    private int hp;//体力
    private bool isDamage;//ダメージフラグ

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
        player = GameObject.Find("Player");//プレイヤーを探す
        smashGage = GameObject.Find("SmashGage").GetComponent<SmashGage>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isStan = false;
        isDamage = false;
        size = transform.localScale;
        col = GetComponent<Collider2D>();
        hp = maxHp;
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
        Dead();//消滅
    }

    /// <summary>
    /// 消滅
    /// </summary>
    protected virtual void Dead()
    {
        if (!isStan || tag == "Boss") return;

        Vector3 pos = transform.position;
        Vector3 screenMinPos = mainCamera.ScreenMin;//画面の左下の座標
        Vector3 screenMaxPos = mainCamera.ScreenMax;//画面の右下の座標

        //画面外に出たら消滅
        if (pos.x <= screenMinPos.x - size.x / 2 || pos.x >= screenMaxPos.x + size.x / 2
            || pos.y <= screenMinPos.y - size.y / 2 || pos.y >= screenMaxPos.y + size.y / 2)
        {
            GameObject effect = Instantiate(dead_effect);
            effect.transform.position = transform.position;

            Destroy(gameObject);//消滅
        }
    }

    /// <summary>
    /// 吹き飛ぶ
    /// </summary>
    private void Shoot()
    {
        if (isStan) return;

        lookPos = player.transform.position;//向く方向の座標
        playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.mass = 1.0f;
        rigid.drag = 0.0f;
        rigid.AddForce(-playerVec * shootSpeed, ForceMode2D.Impulse);//後ろに吹き飛ぶ
        player.GetComponent<Player>().AddSP(point, false);//プレイヤーのスマッシュポイント加算
        Time.timeScale = 0.0f;//ゲーム停止
        col.isTrigger = true;//あたり判定のトリガーオン
        isStan = true;//気絶フラグtrue
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="damage"></param>
    private void Damage(Smash smash)
    {
        if (!smashGage.IsMax) return;

        //ダメージ
        int damage = smash.GetParam.power;
        hp = Mathf.Max(hp - damage, 0);
        isStan = true;
        if (hp <= 0)
        {
            hp = 0;
            mainCamera.Stop();
        }
    }

    /// <summary>
    /// 気絶フラグ
    /// </summary>
    public bool IsStan
    {
        get { return isStan; }
    }

    /// <summary>
    /// 体力
    /// </summary>
    public int HP
    {
        get { return hp; }
    }

    /// <summary>
    /// 最大体力
    /// </summary>
    public int MaxHP
    {
        get { return maxHp; }
    }

    /// <summary>
    /// あたり判定（トリガー）
    /// </summary>
    void OnTriggerEnter2D(Collider2D col)
    {
        //if (col.transform.tag == "PlayerBullet")
        //{
        //    Player player_class = player.GetComponent<Player>();
        //    float bulletSize = col.transform.localScale.x;
        //    int damage = (int)(bulletSize);
        //    if (bulletSize < player_class.GetParam.bulletMaxSize)
        //    {
        //        Shoot(damage, false);
        //    }
        //    else
        //    {
        //        Shoot(damage, true);
        //    }
        //    Destroy(col.gameObject);
        //}

        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack")
        {
            Smash smash = col.transform.parent.parent.GetComponent<Smash>();
            if (tag == "Boss")
            {
                Damage(smash);
            }
            if (tag == "Enemy")
            {
                Shoot();
            }
            smash.Hit(tag);
        }

        if (col.transform.tag == "Barrier" || col.transform.tag == "BarrierPoint")
        {
            GameObject effect = Instantiate(dead_effect);
            effect.transform.position = transform.position;

            if (col.transform.tag == "BarrierPoint")
            {
                Destroy(col.gameObject);
            }
            Destroy(gameObject);//消滅
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack")
        {
            Smash smash = col.transform.parent.parent.GetComponent<Smash>();
            if (tag == "Boss")
            {
                Damage(smash);
            }
            if (tag == "Enemy")
            {
                Shoot();
            }
            smash.Hit(tag);
        }
    }
}
