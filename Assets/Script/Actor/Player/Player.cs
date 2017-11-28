using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //パラメータの構造体
    public struct Parameter
    {
        public int hp;//体力
        public int maxHP;//最大体力
        public int maxSP;//最大スマッシュポイント
        public float speed;//移動速度
        public float sp;//スマシュポイント
        public float spDifTime;//スマッシュポイントが減るまでの時間（設定用）
        public float spDifSpeed;//スマッシュポイントが減る速度
    }

    public GameObject damageEffect;//ダメージエフェクト
    public SmashGage smashGage;

    private Parameter parameter;//パラメータ
    private MainCamera mainCamera;//カメラ
    private Animator anim;//アニメーション
    private Vector3 size;//大きさ
    private Vector3 backPos;//後ろに下がる座標
    private float x_axis;//横の入力値
    private float y_axis;//縦の入力値
    private float spDifCount;//スマッシュポイントが減るまでの時間
    private bool isDamage;//ダメージ

    //↓仮変数（後で使わなくなるかも）
    private int flashCnt;//点滅カウント

	/// <summary>
	/// ポイント取得率
	/// </summary>
	[SerializeField]
	private SpRate spRate;

    /// <summary>
    /// 状態を表す列挙型
    /// </summary>
    private enum State
    {
        IDEL,//待機
        MOVE,//移動
        DEAD,//死亡
    }
    private State state;//状態

    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        anim = transform.Find("body").GetComponent<Animator>();
        size = transform.localScale;//大きさ取得
        state = State.IDEL;//最初は待機状態

        //各フラグをfalseに
        isDamage = false;

        parameter.sp = 0.0f;

        spDifCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.DEAD)
        {
            Move();//移動
            Rotate();//向き変更
            DamageEffect();//ダメージ演出
            SmashPoint();
            //Back();//後ろに下がる
        }
        //Clamp();//移動制限
    }

    /// <summary>
    /// 移動制限
    /// </summary>
    private void Clamp()
    {
        Vector3 screenMinPos = mainCamera.ScreenMin;//画面の左下の座標
        Vector3 screenMaxPos = mainCamera.ScreenMax;//画面の右下の座標

        //座標を画面内に制限(自分の座標)
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, screenMinPos.x + 0.5f, screenMaxPos.x - 0.5f);
        pos.y = Mathf.Clamp(pos.y, screenMinPos.y + 0.5f, screenMaxPos.y - 0.5f);
        transform.position = pos;

        backPos.x = Mathf.Clamp(backPos.x, screenMinPos.x + size.x / 2, screenMaxPos.x - size.x / 2);
        backPos.y = Mathf.Clamp(backPos.y, screenMinPos.y + size.y / 2, screenMaxPos.y - size.y / 2);
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        x_axis = Input.GetAxisRaw("Horizontal");
        y_axis = Input.GetAxisRaw("Vertical");
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
            transform.rotation = Quaternion.identity;
        }
        if (x_axis < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    /// <summary>
    /// ダメージ演出
    /// </summary>
    private void DamageEffect()
    {
        if (!isDamage) return;

        if (!damageEffect.activeSelf)
        {
            mainCamera.SetShake();
            ControllerShake.Shake(1.0f, 1.0f);
            damageEffect.SetActive(true);
        }
        else
        {
            if (mainCamera.IsShakeFinish)
            {
                ControllerShake.Shake(0.0f, 0.0f);
                damageEffect.SetActive(false);
                isDamage = false;
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
    /// 体力回復
    /// </summary>
    public void ChangeHp(int h)
    {
        //体力を上限まで回復
        parameter.hp += h;
        parameter.hp = Mathf.Clamp(parameter.hp, 0, parameter.maxHP);
    }

    /// <summary>
    /// スマッシュポイント変動
    /// </summary>
    public void AddSP(int value)
    {
        if (smashGage.IsMax) return;

        if (value > 0)
        {
			value *= spRate.spRates[parameter.hp - 1];
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
    /// パラメータ設定
    /// </summary>
    public void SetParam(float value, int i)
    {
        switch (i)
        {
            case 0:
                parameter.hp = (int)value;
                break;
            case 1:
                parameter.maxHP = (int)value;
                break;
            case 2:
                parameter.maxSP = (int)value;
                break;
            case 3:
                parameter.speed = value;
                break;
            case 4:
                parameter.spDifTime = value;
                break;
            case 5:
                parameter.spDifSpeed = value;
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
        //SpriteRenderer texture = GetComponent<SpriteRenderer>();
        //Color color = texture.color;

        if (parameter.hp > 0 && !isDamage)
        {
            ChangeHp(-1);
            isDamage = true;
        }
        if (parameter.hp <= 0 && state != State.DEAD)
        {
            parameter.hp = 0;
            //color.a = 0.0f;
            //texture.color = color;
            state = State.DEAD;
        }
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay2D(Collision2D col)
    {
        //敵に当たったらダメージ
        if (col.transform.tag == "Enemy" || col.transform.tag == "Boss")
        {
            Damage();
        }
    }
}
