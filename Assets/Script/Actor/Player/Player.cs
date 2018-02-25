using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //パラメータの構造体
    public struct Parameter
    {
        public int maxSP;//最大スマッシュポイント
        public float speed;//移動速度
        public float sp;//スマシュポイント
        public float spDifTime;//スマッシュポイントが減るまでの時間（設定用）
        public float spDifSpeed;//スマッシュポイントが減る速度
        public float dashInterval;//ダッシュのインターバル
        public float dashSpeed;//ダッシュ速度
    }

    public GameObject damageEffect;//ダメージエフェクト
    public GameObject aura;//オーラ
    public SmashGage smashGage;//スマッシュゲージ
    public GameObject chara;//画像オブジェクト
    public GameObject sound;//サウンド
    public AudioClip[] se;//効果音

    private Parameter parameter;//パラメータ
    private MainCamera mainCamera;//カメラ
    private Animator anim;//アニメーション
    private Rigidbody2D rigid;
    private Vector3 dashPos;//ダッシュ座標
    private Vector3 vec;//ダッシュの方向
    private float x_axis;//横の入力値
    private float y_axis;//縦の入力値
    private float spDifCount;//スマッシュポイントが減るまでの時間
    private float dashCount;
    private bool isDamage;//ダメージフラグ

    /// <summary>
    /// ポイント取得率
    /// </summary>
    [SerializeField]
    private SpRate spRate;

    /// <summary>
    /// 状態を表す列挙型
    /// </summary>
    public enum State
    {
        IDEL,//待機
        MOVE,//移動
        DASH,//突撃
        ATTACK,//攻撃
        DEAD,//死亡
        BROWN,//吹き飛び
    }
    private State state;//状態

    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        anim = chara.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        state = State.IDEL;//最初は待機状態

        isDamage = false;
        parameter.sp = 0.0f;
        spDifCount = 0.0f;
        dashCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        x_axis = Input.GetAxisRaw("Horizontal");
        y_axis = Input.GetAxisRaw("Vertical");

        if (state != State.DEAD)
        {
            Move();//移動
            Rotate();//向き変更
            SmashPoint();
            Dash();//ダッシュ
            BrownOff();//吹き飛び
        }
        Dead();
        AuraAcrive();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (state == State.DASH || state == State.ATTACK || state == State.BROWN) return;

        Vector2 axis = Vector2.zero;

        state = State.IDEL;
        anim.SetBool("Walk", false);

        if (x_axis >= 0.5f || x_axis <= -0.5f
            || y_axis >= 0.5f || y_axis <= -0.5f)
        {
            axis = new Vector2(x_axis, y_axis);
            state = State.MOVE;
            anim.SetBool("Walk", true);
        }

        if (axis.magnitude != 0.0f)
        {
            axis.Normalize();
        }

        transform.Translate(axis * parameter.speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// 向きを変更
    /// </summary>
    private void Rotate()
    {
        if (state != State.MOVE) return;

        if (x_axis > 0)
        {
            chara.transform.rotation = Quaternion.identity;
        }
        if (x_axis < 0)
        {
            chara.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    /// <summary>
    /// 突進
    /// </summary>
    private void Dash()
    {
        if (state == State.ATTACK || state == State.BROWN) return;

        Vector3 lookPos = new Vector3(transform.position.x + x_axis, transform.position.y + y_axis, 0);//向く方向の座標

        dashCount = Mathf.Max(dashCount - Time.deltaTime, 0.0f);

        if (state != State.DASH)
        {
            if (vec == Vector3.zero && dashCount <= 0.0f
                && (Input.GetButtonDown("Dash") || Input.GetAxisRaw("Dash") <= -0.5f))
            {
                vec = (lookPos - transform.position).normalized;//向く方向を正規化
                rigid.AddForce(vec * parameter.dashSpeed, ForceMode2D.Impulse);
                state = State.DASH;
                GameObject soundObj = Instantiate(sound);
                soundObj.GetComponent<SE>().SetClip(se[0]);
            }
            if (vec != Vector3.zero
                && (Input.GetButtonUp("Dash") || Input.GetAxisRaw("Dash") == 0.0f))
            {
                vec = Vector3.zero;
            }
        }

        if (state == State.DASH)
        {
            if (rigid.velocity.magnitude <= 25)
            {
                dashPos = Vector3.zero;
                rigid.velocity = Vector2.zero;
                dashCount = parameter.dashInterval;
                state = State.IDEL;
            }
        }
    }

    /// <summary>
    /// スマッシュポイント
    /// </summary>
    private void SmashPoint()
    {
        if (spDifCount > 0.0f)
        {
            spDifCount -= Time.deltaTime;
            return;
        }

        parameter.sp = Mathf.Max(parameter.sp - Time.deltaTime * parameter.spDifSpeed, 0.0f);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        if (state != State.DEAD) return;

        if (damageEffect == null
            && anim.updateMode != AnimatorUpdateMode.UnscaledTime)
        {
            anim.SetTrigger("Dead");
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    /// <summary>
    /// オーラのアクティブを切り替える
    /// </summary>
    private void AuraAcrive()
    {
        if (smashGage.IsMax && Time.deltaTime == 0.0f)
        {
            aura.SetActive(false);
        }
        else
        {
            aura.SetActive(true);
        }
    }

    /// <summary>
    /// スマッシュポイント変動
    /// </summary>
    public void AddSP(int value, bool isReset)
    {
        if (smashGage.IsMax)
        {
            if (isReset)
            {
                parameter.sp = 0.0f;
            }
            return;
        }

        if (value > 0)
        {
            //value *= spRate.spRates[parameter.hp - 1];
            parameter.sp = Mathf.Min(parameter.sp + value, parameter.maxSP);
            spDifCount = parameter.spDifTime;
        }
    }

    /// <summary>
    /// パラメータ取得
    /// </summary>
    public Parameter GetParam
    {
        get { return parameter; }
    }

    /// <summary>
    /// 状態取得
    /// </summary>
    public bool IsState(State state)
    {
        return this.state == state;
    }

    /// <summary>
    /// パラメータ設定
    /// </summary>
    public void SetParam(float value, int i)
    {
        switch (i)
        {
            case 0:
                parameter.maxSP = (int)value;
                break;
            case 1:
                parameter.speed = value;
                break;
            case 2:
                parameter.spDifTime = value;
                break;
            case 3:
                parameter.spDifSpeed = value;
                break;
            case 4:
                parameter.dashInterval = value;
                break;
            case 5:
                parameter.dashSpeed = value;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 死亡フラグ
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return state == State.DEAD;
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    public void Damage()
    {
        if (state == State.DASH) return;
        damageEffect.SetActive(true);

        Animator damageAnim = damageEffect.GetComponent<Animator>();
        damageAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        FindObjectOfType<GameManager>().ShakeController(1.0f, 0.5f);

        state = State.DEAD;
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.tag == "Wall" && state == State.DASH)
        {
            dashPos = Vector3.zero;
            rigid.velocity = Vector2.zero;
            state = State.IDEL;
        }
    }

    /// <summary>
    /// ボスと攻撃判定が衝突した時の吹き飛び処理
    /// </summary>
    private void BrownOff()
    {
        if (state != State.BROWN) return;

        Vector3 lookPos = new Vector3(transform.position.x + x_axis, transform.position.y + y_axis, 0);//向く方向の座標

        vec = (lookPos - transform.position).normalized * -1;//向く方向を正規化
        rigid.AddForce(vec * parameter.dashSpeed, ForceMode2D.Impulse);
        if (rigid.velocity.magnitude <= 10f)
        {
            rigid.velocity = Vector2.zero;
            state = State.IDEL;
        }
    }

    /// <summary>
    /// 吹っ飛び開始
    /// </summary>
    public void SetBrown()
    {
        if (state == State.DASH) return;
        state = State.BROWN;
    }
}
