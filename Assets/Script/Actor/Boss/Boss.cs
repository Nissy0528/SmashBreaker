using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    protected int maxHp;//最大体力
    [SerializeField]
    protected float stanTime;//硬直時間

    protected GameManager gameManager;
    protected GameObject player;//プレイヤー
    protected SmashGage smashGage;
    protected GameObject[] enemys;//フィールド上の敵
    protected bool isStan;//気絶フラグ
    protected float stanDelay;//硬直カウント

    public GameObject dead_effect;//死亡時エフェクト
    public GameObject cutIn;//カットインUI
    public GameObject frash;//フラッシュUI
    public GameObject sound;//サウンド
    public AudioClip se;//効果音
    public SpriteRenderer[] sprites;//スプライトの配列
    public string[] stopAnims;//気絶時に止めるアニメーション
    public float deadTime;//消滅演出時間
    public float deadShakeRange;//消滅時の振動範囲
    public float startSpeed;//出現時間

    private MainCamera mainCamera;//カメラ
    private Animator anim;//アニメーション
    private BossHitEffect hitEffect;//ヒットエフェクト
    private AI[] ai;//人工知能クラスの配列
    private GameObject cutInObj;
    private Vector2 lowRange;//振動時の最小座標
    private Vector2 maxRange;//振動時の最大座標
    private int hp;//体力
    private bool isDamage;//ダメージフラグ
    private bool isDead;//死亡フラグ
    private bool isStart;//開始フラグ

    // Use this for initialization
    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        player = GameObject.Find("Player");//プレイヤーを探す
        smashGage = FindObjectOfType<SmashGage>();
        gameManager = FindObjectOfType<GameManager>();
        anim = transform.Find("Chara").GetComponent<Animator>();
        hitEffect = FindObjectOfType<BossHitEffect>();

        isStan = false;
        isDamage = false;
        isDead = false;
        isStart = false;

        hp = maxHp;
        stanDelay = stanTime;

        foreach (var s in sprites)
        {
            Color color = s.material.color;
            color.a = 0.0f;
            s.material.color = color;
        }

        ai = GetComponents<AI>();
        foreach (var a in ai)
        {
            a.enabled = false;
        }

        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize() { }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        StartEffect();
        if (isStart && cutInObj == null)
        {
            BossUpdate();
            Stan();
            BossDead();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void BossUpdate() { }

    /// <summary>
    /// 硬直
    /// </summary>
    private void Stan()
    {
        if (!isStan || HP <= 0) return;

        if (stanDelay == stanTime)
        {
            //ダメージアニメーション以外は停止
            foreach (var s in stopAnims)
            {
                AnimBool(s, false);
            }
            AnimBool("Hit", true);
            EnemyDead();
        }
        stanDelay -= Time.deltaTime;

        if (stanDelay <= 0.0f)
        {
            AnimBool("Hit", false);
            isStan = false;
            stanDelay = stanTime;
        }
    }

    /// <summary>
    /// 全雑魚敵消滅
    /// </summary>
    private void EnemyDead()
    {
        if (enemys.Length <= 0) return;

        foreach (var e in enemys)
        {
            e.GetComponent<Enemy>().Dead();
        }
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
    /// 消滅
    /// </summary>
    private void BossDead()
    {
        if (hp > 0) return;

        GameObject frash = GameObject.FindGameObjectWithTag("Frash");

        if (frash != null)
        {
            isDead = true;
            EnemyDead();
            return;
        }

        Dead();
    }
    private void Dead()
    {
        if (!isDead) return;

        if (anim.updateMode != AnimatorUpdateMode.UnscaledTime)
        {
            gameManager.SetTimeScale(0.5f, 0.0f);
            SetShake();
            // タイムスケールを無視する
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        if (Time.timeScale == 1.0f)
        {
            DeadShake();
            if (deadTime <= 0.0f)
            {
                GameObject deadEffectObj = Instantiate(dead_effect);
                deadEffectObj.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 死亡時の振動
    /// </summary>
    private void DeadShake()
    {
        if (deadTime <= 0.0f) return;

        deadTime -= Time.deltaTime;
        float x = Random.Range(lowRange.x, maxRange.x);
        float y = Random.Range(lowRange.y, maxRange.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    /// <summary>
    /// 振動の範囲設定
    /// </summary>
    private void SetShake()
    {
        lowRange = new Vector2(transform.position.x - deadShakeRange, transform.position.y - deadShakeRange);
        maxRange = new Vector2(transform.position.x + deadShakeRange, transform.position.y + deadShakeRange);
    }

    /// <summary>
    /// 登場エフェクト
    /// </summary>
    private void StartEffect()
    {
        if (isStart) return;

        isStart = true;
        foreach (var s in sprites)
        {
            Color color = s.material.color;
            color.a = Mathf.Min(color.a + startSpeed * Time.deltaTime, 1.0f);
            s.material.color = color;

            if (color.a < 1.0f)
            {
                isStart = false;
            }
        }

        if (isStart)
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            cutInObj = Instantiate(cutIn, canvas);
            Instantiate(frash, canvas);
            GameObject soundObj = Instantiate(sound);
            soundObj.GetComponent<SE>().SetClip(se);
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
            if (tag == "Boss")
            {
                Damage(smash);
            }
            smash.Hit(tag);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Attack")
        {
            Smash smash = FindObjectOfType<Smash>();
            if (tag == "Boss")
            {
                Damage(smash);
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
    /// アニメーション切り替え
    /// </summary>
    /// <param name="name">切り替えるフラグの名前</param>
    public void AnimBool(string name, bool frag)
    {
        if (!anim.enabled) return;

        anim.SetBool(name, frag);
    }

    /// <summary>
    /// アニメーション終了判定
    /// </summary>
    /// <returns></returns>
    public bool AnimFinish(string name)
    {
        if (!anim.enabled) return false;

        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(name) && animState.normalizedTime >= 1;
    }

    /// <summary>
    /// アニメションの速度変更
    /// </summary>
    /// <param name="speed"></param>
    private void AnimSpeed(float speed)
    {
        if (!anim.enabled) return;
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        anim.speed = speed;
    }

    /// <summary>
    /// 気絶フラグ
    /// </summary>
    public bool IsStan
    {
        get { return isStan; }
        set { isStan = value; }
    }

    /// <summary>
    /// 最大体力
    /// </summary>
    public int MaxHP
    {
        get { return maxHp; }
    }

    /// <summary>
    /// 体力
    /// </summary>
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    /// <summary>
    /// 行動開始フラグ
    /// </summary>
    public bool IsStart
    {
        get { return isStart; }
    }
}
