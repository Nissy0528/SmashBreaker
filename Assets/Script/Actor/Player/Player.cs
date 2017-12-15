using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
	//パラメータの構造体
	public struct Parameter
	{
		//public int hp;//体力
		//public int maxHP;//最大体力
		public int maxSP;//最大スマッシュポイント
		public float speed;//移動速度
		public float sp;//スマシュポイント
		public float spDifTime;//スマッシュポイントが減るまでの時間（設定用）
		public float spDifSpeed;//スマッシュポイントが減る速度
		public float dashInterval;//ダッシュのインターバル
		public float dashSpeed;//ダッシュ速度
							   //public float shootInterval;
							   //public float bulletGrawSpeed;
							   //public float bulletMaxSize;
	}

	//public GameObject damageEffect;//ダメージエフェクト
	//public GameObject warp;//ワープゾーン
	public SmashGage smashGage;
	//public GameObject bullet;
	public GameObject muzzle;
	public GameObject spMaxEffect;

	private Parameter parameter;//パラメータ
	private MainCamera mainCamera;//カメラ
	private Animator anim;//アニメーション
	private Rigidbody2D rigid;
	private GameObject bulletObj;
	private Vector3 size;//大きさ
	private Vector3 dashPos;//後ろに下がる座標
	private Vector3 vec;//ダッシュの方向
	private float x_axis;//横の入力値
	private float y_axis;//縦の入力値
	private float spDifCount;//スマッシュポイントが減るまでの時間
	private float dashCount;
	//private float shootCount;
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
		anim = transform.Find("body").GetComponent<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		size = transform.localScale;//大きさ取得
		state = State.IDEL;//最初は待機状態

		isDamage = false;
		parameter.sp = 0.0f;
		spDifCount = 0.0f;
		dashCount = 0.0f;
		//shootCount = 0.0f;
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
					 //DamageEffect();//ダメージ演出
			SmashPoint();
			Dash();//ダッシュ
				   //BulletShoot();
				   //CreateWarp();
			BrownOff();//吹き飛び
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
			transform.rotation = Quaternion.identity;
		}
		if (x_axis < 0)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
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
	/// ダメージ演出
	/// </summary>
	//private void DamageEffect()
	//{
	//    if (!isDamage) return;

	//    if (!damageEffect.activeSelf)
	//    {
	//        mainCamera.SetShake(true, 0.0f);
	//        ControllerShake.Shake(1.0f, 1.0f);
	//        damageEffect.SetActive(true);
	//    }
	//    else
	//    {
	//        if (mainCamera.IsShakeFinish)
	//        {
	//            ControllerShake.Shake(0.0f, 0.0f);
	//            damageEffect.SetActive(false);
	//            isDamage = false;
	//        }
	//    }
	//}

	/// <summary>
	/// 弾発射
	/// </summary>
	//private void BulletShoot()
	//{
	//    SpawnBullt();//弾生成
	//    GrowBullet();//弾を大きくする

	//    if (Input.GetButtonUp("Decision") && bulletObj != null)
	//    {
	//        bulletObj.transform.parent = null;
	//        bulletObj.AddComponent<CircleCollider2D>();
	//        bulletObj.GetComponent<CircleCollider2D>().radius = 0.16f;
	//        bulletObj.GetComponent<CircleCollider2D>().isTrigger = true;
	//        bulletObj.AddComponent<CircleBullet>();
	//        bulletObj.GetComponent<CircleBullet>().Speed = 20;
	//        bulletObj.GetComponent<CircleBullet>().IsPlayer = true;
	//        muzzle.transform.localScale = Vector3.one;
	//        bulletObj = null;
	//        state = State.IDEL;
	//    }
	//}
	/// <summary>
	/// 弾生成
	/// </summary>
	//private void SpawnBullt()
	//{
	//    if (shootCount > 0.0f)
	//    {
	//        shootCount -= Time.deltaTime;
	//        return;
	//    }

	//    if (Input.GetButtonDown("Decision"))
	//    {
	//        bulletObj = Instantiate(bullet, muzzle.transform);
	//        bullet.transform.localPosition = new Vector3(0, 0.18f, 0);
	//        shootCount = parameter.shootInterval;
	//    }
	//}
	/// <summary>
	/// 弾を大きくする
	/// </summary>
	//private void GrowBullet()
	//{
	//    if (bulletObj == null) return;

	//    if (Input.GetButton("Decision"))
	//    {
	//        state = State.ATTACK;
	//        Vector3 bulletSize = muzzle.transform.localScale;
	//        bulletSize += new Vector3(parameter.bulletGrawSpeed, parameter.bulletGrawSpeed, 1.0f);
	//        bulletSize.x = Mathf.Clamp(bulletSize.x, 0.0f, parameter.bulletMaxSize);
	//        bulletSize.y = Mathf.Clamp(bulletSize.y, 0.0f, parameter.bulletMaxSize);
	//        muzzle.transform.localScale = bulletSize;
	//    }
	//}

	/// <summary>
	/// ワープゾーン生成
	/// </summary>
	//private void CreateWarp()
	//{
	//    if (Input.GetButtonDown("Decision"))
	//    {
	//        GameObject warpObj = Instantiate(warp);
	//        warpObj.transform.position = transform.position;
	//    }
	//}

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

		spMaxEffect.SetActive(smashGage.IsMax);
	}

	/// <summary>
	/// 体力回復
	/// </summary>
	//public void ChangeHp(int h)
	//{
	//    //体力を上限まで回復
	//    parameter.hp += h;
	//    parameter.hp = Mathf.Clamp(parameter.hp, 0, parameter.maxHP);
	//}

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
		GameObject boss = GameObject.FindGameObjectWithTag("Boss");
		if (state == State.DASH || boss == null) return;

		//if (parameter.hp > 0 && !isDamage)
		//{
		//    ChangeHp(-1);
		//    isDamage = true;
		//}
		//if (parameter.hp <= 0 && state != State.DEAD)
		//{
		//    parameter.hp = 0;
		state = State.DEAD;
		//}
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
		if (rigid.velocity.magnitude <= 25)
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
		state = State.BROWN;
	}
}
