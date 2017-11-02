using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//移動速度
    //public float backSpeed;//殴ったときに後ろに下がる速度
    public int hp;//体力

    private GameObject smash;//攻撃のあたり判定
    //private GameObject backPosObj;//後ろに下がる座標オブジェクト
    private GameObject smashGage;//スマッシュゲージ
    private MainCamera camera;//カメラ
    //private Animator anim;//アニメーション
    private Vector3 size;//大きさ
    private Vector3 attackColSize;//攻撃あたり判定の大きさ
    private Vector3 iniAttackPos;//攻撃あたり判定の初期位置
    private Vector3 backPos;//後ろに下がる座標
    private float x_axis;//横の入力値
    private float y_axis;//縦の入力値
    private float sp;//スマッシュポイント
    private bool isDamage;//ダメージ
    //private bool isBack;//tureなら後ろに下がり続ける

    //↓仮変数（後で使わなくなるかも）
    private int flashCnt;//点滅カウント

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
        camera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        smash = GameObject.Find("Smash").gameObject;
        //backPosObj = transform.Find("BackPos").gameObject;
        size = transform.localScale;//大きさ取得
        state = State.IDEL;//最初は待機状態

        //あたり判定の大きさを体力に合わせて変える
        attackColSize = smash.transform.localScale;
        iniAttackPos = smash.transform.localPosition;
        ChangeHp(0);

        //各フラグをfalseに
        isDamage = false;
        //isBack = false;

        sp = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.DEAD)
        {
            Move();//移動
            Rotate();//向き変更
            Damage();//ダメージ演出
            //Back();//後ろに下がる
        }
        //Clamp();//移動制限
    }

    ///// <summary>
    ///// 移動制限
    ///// </summary>
    //private void Clamp()
    //{
    //    Vector3 screenMinPos = camera.ScreenMin;//画面の左下の座標
    //    Vector3 screenMaxPos = camera.ScreenMax;//画面の右下の座標

    //    //座標を画面内に制限(自分の座標)
    //    Vector3 pos = transform.position;
    //    pos.x = Mathf.Clamp(pos.x, screenMinPos.x + size.x / 2, screenMaxPos.x - size.x / 2);
    //    pos.y = Mathf.Clamp(pos.y, screenMinPos.y + size.y / 2, screenMaxPos.y - size.y / 2);
    //    transform.position = pos;

    //    backPos.x = Mathf.Clamp(backPos.x, screenMinPos.x + size.x / 2, screenMaxPos.x - size.x / 2);
    //    backPos.y = Mathf.Clamp(backPos.y, screenMinPos.y + size.y / 2, screenMaxPos.y - size.y / 2);
    //}

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        //if (isBack) return;

        x_axis = Input.GetAxisRaw("Horizontal");
        y_axis = Input.GetAxisRaw("Vertical");
        Vector2 axis = Vector2.zero;

        state = State.IDEL;

        if (x_axis >= 0.5f || x_axis <= -0.5f
            || y_axis >= 0.5f || y_axis <= -0.5f)
        {
            axis = new Vector2(x_axis, y_axis);
            state = State.MOVE;
        }

        if (axis.magnitude != 0.0f)
        {
            axis.Normalize();
        }

        transform.Translate(axis * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// 向きを変更
    /// </summary>
    private void Rotate()
    {
        if (state != State.MOVE) return;

        Vector3 lookPos = new Vector3(transform.position.x + x_axis * -1, transform.position.y + y_axis * -1, 0);//向く方向の座標
        Vector3 vec = (lookPos - transform.position).normalized;//向く方向を正規化
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);//入力された方向に向く
    }

    /// <summary>
    /// ダメージ演出
    /// </summary>
    private void Damage()
    {
        if (!isDamage) return;

        SpriteRenderer texture = GetComponent<SpriteRenderer>();
        Color color = texture.color;
        flashCnt += 1;
        color.a = (flashCnt / 5) % 2;
        if (flashCnt >= 60)
        {
            color.a = 1;
            flashCnt = 0;
            isDamage = false;
        }
        texture.color = color;
    }

    /// <summary>
    /// 後ろに下がる
    /// </summary>
    //private void Back()
    //{
    //    if (!isBack) return;

    //    //指定した距離分下がる
    //    transform.position = Vector3.MoveTowards(transform.position, backPos, backSpeed * Time.deltaTime);
    //    if (transform.position == backPos)
    //    {
    //        isBack = false;
    //    }
    //}

    /// <summary>
    /// 体力回復
    /// </summary>
    public void ChangeHp(int h)
    {
        //体力を上限まで回復
        hp += h;
        hp = Mathf.Clamp(hp, 0, 5);

        //体力に合わせて拳のサイズを変える
        smash.transform.localScale = new Vector3(attackColSize.x * hp, attackColSize.y * hp, 1);

        if (h > 0)
        {
            sp = 0.0f;
        }
    }

    /// <summary>
    /// スマッシュポイント加算
    /// </summary>
    public void AddSP()
    {
        sp = Mathf.Min(sp + 1.0f, 30.0f);
    }

    /// <summary>
    /// 後退開始
    /// </summary>
    //public void SetBack()
    //{
    //    //後ろに下がるように
    //    if (!isBack)
    //    {
    //        backPos = backPosObj.transform.position;
    //        isBack = true;
    //    }
    //}

    /// <summary>
    /// スマッシュポイント取得
    /// </summary>
    public float GetSP
    {
        get { return sp; }
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
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay2D(Collision2D col)
    {
        SpriteRenderer texture = GetComponent<SpriteRenderer>();
        Color color = texture.color;

        //敵に当たったらダメージ
        if (col.transform.tag == "Enemy")
        {
            if (hp > 0 && !isDamage)
            {
                ChangeHp(-1);
                isDamage = true;
            }
            if (hp <= 0 && state != State.DEAD)
            {
                hp = 0;
                color.a = 0.0f;
                texture.color = color;
                state = State.DEAD;
            }
        }
    }
}
