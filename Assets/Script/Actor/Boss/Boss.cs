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
    protected bool isStan;//気絶フラグ
    protected float stanDelay;//硬直カウント

    public GameObject dead_effect;//死亡時エフェクト
    public float deadTime;//消滅演出時間
    public float deadShakeTime;//消滅時のコントローラー振動時間

    private MainCamera mainCamera;//カメラ
    private Animator anim;//アニメーション
    private GameObject cutIn;//死亡時のカットイン
    private GameObject deadUI;//死亡時の演出UI
    private int hp;//体力
    private bool isDamage;//ダメージフラグ

    // Use this for initialization
    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        player = GameObject.Find("Player");//プレイヤーを探す
        smashGage = FindObjectOfType<SmashGage>();
        gameManager = FindObjectOfType<GameManager>();
        cutIn = FindObjectOfType<CutIn>().gameObject;
        deadUI = FindObjectOfType<BossDown>().gameObject;
        anim = transform.Find("Chara").GetComponent<Animator>();
        isStan = false;
        isDamage = false;
        hp = maxHp;
        stanDelay = stanTime;
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize() { }

    // Update is called once per frame
    void Update()
    {
        BossUpdate();
        Stan();
        BossDead();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void BossUpdate() { }

    /// <summary>
    /// 硬直
    /// </summary>
    public virtual void Stan() { }

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

        if (cutIn != null)
        {
            cutIn.GetComponent<CutIn>().enabled = true;
            cutIn.GetComponent<CutIn>().SetBossDeadUI(deadUI);
        }

        if (deadUI == null)
        {
            GameObject charaPrefab = Instantiate(transform.Find("Chara").gameObject);
            charaPrefab.transform.position = transform.position;
            charaPrefab.transform.localScale = transform.localScale;
            if (charaPrefab.GetComponent<Animator>() != null)
            {
                charaPrefab.GetComponent<Animator>().enabled = false;
            }

            GameObject effect = Instantiate(dead_effect);
            effect.transform.parent = charaPrefab.transform;
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localScale = new Vector3(0.05f, 0.05f, 1);

            gameManager.SetTimeScale(deadTime, 0.25f);
            gameManager.ShakeController(1.0f, deadShakeTime);
            Destroy(gameObject);
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

    /// 消滅
    /// </summary>
    public void Dead()
    {
        GameObject effect = Instantiate(dead_effect);
        effect.transform.position = transform.position;

        Destroy(gameObject);//消滅
    }

    /// <summary>
    /// アニメーション切り替え
    /// </summary>
    /// <param name="name">切り替えるフラグの名前</param>
    public virtual void AnimBool(string name, bool frag)
    {
        anim = transform.Find("Chara").GetComponent<Animator>();
        if (!anim.enabled) return;

        anim.SetBool(name, frag);
    }

    /// <summary>
    /// アニメーション終了判定
    /// </summary>
    /// <returns></returns>
    public virtual bool AnimFinish(string name)
    {
        anim = transform.Find("Chara").GetComponent<Animator>();
        if (!anim.enabled) return false;

        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(name) && animState.normalizedTime >= 1;
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
}
