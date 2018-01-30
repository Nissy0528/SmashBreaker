using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    public GameObject chara;//キャラ画像
    public GameObject sound;//サウンド
    public AudioClip[] se;//効果音
    public TitleSmash smash;//スマッシュ
    public float speed;//速度
    public float dashSpeed;//ダッシュ速度
    public float moveInterval;//移動インターバル
    public float destroyInterval;//消滅インターバル

    private Animator anim;//アニメーション
    private Rigidbody2D rigid;
    private Vector3 vec;//ダッシュ方向
    private bool isDash;

    /// <summary>
    /// 状態を表す列挙型
    /// </summary>
    public enum State
    {
        IDEL,//待機
        MOVE,//移動
        DASH,//突撃
        ATTACK,//攻撃
        DEAD,//消滅
    }
    private State state;//状態

    // Use this for initialization
    void Start()
    {
        anim = chara.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        state = State.IDEL;//最初は待機状態
        isDash = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DashEnd())
        {
            state = State.IDEL;
        }
        if (smash.SmashEnd())
        {
            moveInterval -= Time.deltaTime;
            if (moveInterval <= 0.0f && state != State.DEAD)
            {
                state = State.MOVE;
            }
        }

        Move();
        Dash();
        Dead();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (state != State.MOVE) return;

        anim.SetBool("Walk", true);

        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);

        if (transform.position.x >= 0.0f)
        {
            Vector2 pos = transform.position;
            pos.x = 0.0f;
            transform.position = pos;
            state = State.DEAD;
        }
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    private void Dash()
    {
        if (state != State.DASH) return;

        if (rigid.velocity == Vector2.zero)
        {
            rigid.AddForce(transform.right * dashSpeed, ForceMode2D.Impulse);
            GameObject soundObj = Instantiate(sound);
            soundObj.GetComponent<SE>().SetClip(se[0]);
            isDash = true;
        }
    }

    /// <summary>
    /// 消滅
    /// </summary>
    private void Dead()
    {
        if (state != State.DEAD) return;

        destroyInterval -= Time.deltaTime;
        if (destroyInterval <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 状態設定
    /// </summary>
    public State PlayerState
    {
        set { state = value; }
    }

    /// <summary>
    /// ダッシュ終了判定
    /// </summary>
    /// <returns></returns>
    public bool DashEnd()
    {
        return isDash && rigid.velocity.magnitude <= 0.01f;
    }
}
