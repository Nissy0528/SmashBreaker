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
    protected AI[] ai_classes;//人工知能用配列


    protected GameManager gameManager;
    protected GameObject player;//プレイヤー
    protected bool isStan;//気絶フラグ

    public GameObject dead_effect;//死亡時エフェクト
    public GameObject sp_effect;//スマッシュポイントエフェクト
    public GameObject audio;
    public AudioClip[] se;//効果音

    private MainCamera mainCamera;//カメラ
    private Collider2D col;
    private Vector3 size;//サイズ
    private Vector3 playerVec;//プレイヤーの方向
    private Vector3 lookPos;//見る方向

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        player = GameObject.Find("Player");//プレイヤーを探す
        isStan = false;
        size = transform.localScale;
        col = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.LayerCollision("Enemy", "Enemy", true);
        Initialize();
    }

    public virtual void Initialize() { }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        EnemyUpdate();
        AI();
        ScreenOut();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void EnemyUpdate() { }

    /// <summary>
    /// 人工知能
    /// </summary>
    public virtual void AI() { }

    /// <summary>
    /// 吹き飛ぶ
    /// </summary>
    private void Shoot()
    {
        if (isStan) return;

        lookPos = player.transform.position;//向く方向の座標
        playerVec = (lookPos - transform.position).normalized;//向く方向を正規化
        SetRigid();
        SpawnSPEffect();
        player.GetComponent<Player>().AddSP(point, false);//プレイヤーのスマッシュポイント加算
        Time.timeScale = 0.0f;//ゲーム停止
        col.isTrigger = true;//あたり判定のトリガーオン
        point = 0;

        //ヒット効果音生成
        GameObject audioObj = Instantiate(audio);
        audioObj.GetComponent<SE>().SetClip(se[0]);

        isStan = true;//気絶フラグtrue
    }

    /// <summary>
    /// RigidBodyの状態を設定
    /// </summary>
    private void SetRigid()
    {
        //RgidBodyを初期化
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.mass = 1.0f;
        rigid.drag = 0.0f;
        rigid.velocity = Vector2.zero;

        rigid.AddForce(-playerVec * shootSpeed, ForceMode2D.Impulse);//後ろに吹き飛ぶ
    }

    /// <summary>
    /// スマシュポイントエフェクト生成
    /// </summary>
    private void SpawnSPEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject effect = Instantiate(sp_effect);
            effect.transform.position = transform.position;

            GameObject smash = FindObjectOfType<SmashGage>().gameObject;
            Vector3 pos = FindObjectOfType<Camera>().ScreenToWorldPoint(smash.transform.position);

            SPEffect effect_class = effect.GetComponent<SPEffect>();
            pos.y += 4;
            effect_class.SetEndPoint(new Vector2(-pos.x, pos.y));

            if (i == 1)
            {
                effect_class.radius *= -1;
            }
        }
    }

    /// <summary>
    /// あたり判定（トリガー）
    /// </summary>
    void OnTriggerEnter2D(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack")
        {
            Smash smash = FindObjectOfType<Smash>();
            if (tag == "Enemy")
            {
                Shoot();
            }
            smash.Hit(tag);
        }

        if (col.transform.tag == "Barrier" || col.transform.tag == "BarrierPoint")
        {
            if (col.transform.tag == "BarrierPoint")
            {
                Destroy(col.gameObject);
            }

            Dead();
        }

        if (col.tag == "Wall")
        {
            Dead();
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack")
        {
            Smash smash = FindObjectOfType<Smash>();
            if (tag == "Enemy")
            {
                Shoot();
            }
            smash.Hit(tag);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Collision(col);
    }

    public virtual void Collision(Collision2D col) { }

    /// <summary>
    /// 画面外に出たら消滅
    /// </summary>
    private void ScreenOut()
    {
        Vector3 pos = transform.position;
        Vector3 screenMinPos = mainCamera.ScreenMin;//画面の左下の座標
        Vector3 screenMaxPos = mainCamera.ScreenMax;//画面の右下の座標

        //画面外に出たら消滅
        if (pos.x <= screenMinPos.x - size.x / 2 || pos.x >= screenMaxPos.x + size.x / 2
            || pos.y <= screenMinPos.y - size.y / 2 || pos.y >= screenMaxPos.y + size.y / 2)
        {
            Dead();
        }
    }

    /// <summary>
    /// 消滅
    /// </summary>
    public void Dead()
    {
        GameObject effect = Instantiate(dead_effect);
        effect.transform.position = transform.position;

        //爆破効果音生成
        GameObject audioObj = Instantiate(audio);
        audioObj.GetComponent<SE>().SetClip(se[1]);

        Destroy(gameObject);//消滅
    }

    /// <summary>
    /// 気絶フラグ
    /// </summary>
    public bool IsStan
    {
        get { return isStan; }
    }
}
