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
	}

	private Parameter parameter;//パラメータ
	private GameObject smash;//攻撃のあたり判定
	private GameObject smashGage;//スマッシュゲージ
	private MainCamera camera;//カメラ
	private Vector3 size;//大きさ
	private Vector3 attackColSize;//攻撃あたり判定の大きさ
	private Vector3 backPos;//後ろに下がる座標
	private float x_axis;//横の入力値
	private float y_axis;//縦の入力値
	private bool isDamage;//ダメージ

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
		size = transform.localScale;//大きさ取得
		state = State.IDEL;//最初は待機状態

		//あたり判定の大きさを体力に合わせて変える
		attackColSize = smash.transform.localScale;
		ChangeHp(0);

		//各フラグをfalseに
		isDamage = false;

		parameter.sp = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (state != State.DEAD)
		{
			Move();//移動
			Rotate();//向き変更
			DamageEffect();//ダメージ演出
						   //Back();//後ろに下がる
		}
		//Clamp();//移動制限
	}

	/// <summary>
	/// 移動制限
	/// </summary>
	private void Clamp()
	{
		Vector3 screenMinPos = camera.ScreenMin;//画面の左下の座標
		Vector3 screenMaxPos = camera.ScreenMax;//画面の右下の座標

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

		transform.Translate(axis * parameter.speed * Time.deltaTime, Space.World);
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
	private void DamageEffect()
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
	/// 体力回復
	/// </summary>
	public void ChangeHp(int h)
	{
		//体力を上限まで回復
		parameter.hp += h;
		parameter.hp = Mathf.Clamp(parameter.hp, 0, parameter.maxHP);

		//体力に合わせて拳のサイズを変える
		smash.transform.localScale = new Vector3(attackColSize.x * parameter.hp, attackColSize.y * parameter.hp, 1);

		if (h > 0)
		{
			parameter.sp = 0.0f;
		}
	}

	/// <summary>
	/// スマッシュポイント変動
	/// </summary>
	public void AddSP()
	{
		var value = 1;
		if (value > 0)
		{
			parameter.sp = Mathf.Min(parameter.sp + value, parameter.maxSP);
		}
		
	}

	/// <summary>
	/// スマッシュポイント変動
	/// </summary>
	public void AddSP(int value)
	{
		if (value > 0)
		{
			parameter.sp = Mathf.Min(parameter.sp + value, parameter.maxSP);
		}
		if (value < 0)
		{
			if (parameter.sp > 0.0f)
			{
				parameter.sp = Mathf.Max(parameter.sp - parameter.maxSP / 2, 0);
			}
			else
			{
				ChangeHp(value);
			}
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
		SpriteRenderer texture = GetComponent<SpriteRenderer>();
		Color color = texture.color;

		if (parameter.hp > 0 && !isDamage)
		{
			ChangeHp(-1);
			isDamage = true;
		}
		if (parameter.hp <= 0 && state != State.DEAD)
		{
			parameter.hp = 0;
			color.a = 0.0f;
			texture.color = color;
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
