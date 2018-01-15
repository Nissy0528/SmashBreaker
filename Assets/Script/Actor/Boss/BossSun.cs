using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSun : Boss
{
	//public float stanTime;//硬直時間
	//public GameObject cutIn;
	//public GameObject deadUI;//死亡時の演出UI

	private Animator anim;//アニメーション
	private NormalEnemy followClass;
	private Dash dashClass;
	private RazerShooter razerClass;

	private State state;
	private SmashGage gage;


	private GameManager gameManager;
	private float stanDelay;//硬直カウント

	/// <summary>
	/// 攻撃へのカウント
	/// </summary>
	private float attackCount;

	/// <summary>
	/// 攻撃の時間
	/// </summary>
	[SerializeField]
	private float attackTime = 10f;


	// Use this for initialization
	void Start()
	{
		gameObject.tag = "Boss";
		anim = transform.Find("Chara").GetComponent<Animator>();

		followClass = GetComponent<NormalEnemy>();
		dashClass = GetComponent<Dash>();
		dashClass.dashInterval = 0f;
		razerClass = GetComponent<RazerShooter>();

		gameManager = FindObjectOfType<GameManager>();
		//影
		ShadowSet();
		Initialize();
		stanDelay = stanTime;

		gage = player.GetComponent<Player>().smashGage;
		state = State.move;
	}

	private void Update()
	{
		EnemyUpdate();
	}

	protected override void EnemyUpdate()
	{
		Move();
		Attack();
		Stan();//硬直
		BossDead();//消滅
	}

	/// <summary>
	/// 移動
	/// </summary>
	private void Move()
	{
		if (!Check(State.move))
		{
			followClass.Stop();
			return;
		}

		if (!gage.IsMax)
		{
			followClass.enabled = true;
		}
		else
		{

		}

		AttackCount();
	}

	/// <summary>
	/// 攻撃までの時間
	/// </summary>
	private void AttackCount()
	{
		attackCount += Time.deltaTime;
		if (attackCount >= attackTime)
		{
			attackCount = 0;
			state = State.attack;
		}
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	private void Attack()
	{
		if (Check(State.attackWait))
		{
			if (dashClass.enabled & dashClass.IsEnd())
			{
				dashClass.Stop();
				state = State.move;
				return;
			}
			if (razerClass.enabled & razerClass.IsEnd())
			{
				state = State.move;
				razerClass.Stop();
				return;
			}
		}
		else if (!Check(State.attack))
		{
			dashClass.Stop();
			razerClass.Stop();
			return;
		}

		AttackSelect();

		state = State.attackWait;
	}

	/// <summary>
	/// 攻撃の決定
	/// </summary>
	private void AttackSelect()
	{
		//ゲージ満タン時
		if (gage.IsMax)
		{
			switch (DistanceCheck())
			{
				case Distance.near:
					break;
				case Distance.far:
					break;
				case Distance.middle:
					break;
			}
		}
		//それ以外
		else
		{
			switch (DistanceCheck())
			{
				case Distance.near:
					dashClass.enabled = true;
					break;
				case Distance.far:
					razerClass.enabled = true;
					break;
				case Distance.middle:
					dashClass.enabled = true;
					break;
			}
		}


	}

	/// <summary>
	/// 硬直
	/// </summary>
	private void Stan()
	{
		if (!isStan) return;

		if (stanDelay == stanTime)
		{
			AnimBool("Razer", false);
			AnimBool("Hit", true);
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
	/// 消滅
	/// </summary>
	private void BossDead()
	{
		if (HP > 0) return;

		state = State.dead;

		if (cutIn != null)
		{
			cutIn.SetActive(true);
			cutIn.GetComponent<CutIn>().SetBossDeadUI(deadUI);
		}

		if (deadUI == null)
		{
			GameObject effect = Instantiate(dead_effect);
			effect.transform.position = transform.position;
			gameManager.Slow(2.0f, 0.25f);
			gameManager.ShakeController(1.0f, 0.5f);
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// アニメーション切り替え
	/// </summary>
	/// <param name="name">切り替えるフラグの名前</param>
	public override void AnimBool(string name, bool frag)
	{
		if (!anim.enabled) return;

		anim.SetBool(name, frag);
	}

	/// <summary>
	/// アニメーション終了判定
	/// </summary>
	/// <returns></returns>
	public override bool AnimFinish(string name)
	{
		if (!anim.enabled) return false;

		AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
		return animState.IsName(name) && animState.normalizedTime >= 1;
	}

	/// <summary>
	/// 影の設定を有効にする
	/// </summary>
	private void ShadowSet()
	{
		var child = transform.GetChild(0);
		var sr = child.GetComponent<SpriteRenderer>();
		sr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}

	/// <summary>
	/// 距離
	/// </summary>
	private enum Distance
	{
		near,
		middle,
		far,
	}

	/// <summary>
	/// 距離の確認
	/// </summary>
	/// <returns></returns>
	private Distance DistanceCheck()
	{
		var pPos = player.transform.position;
		var mPos = transform.position;

		var distance = Vector2.Distance(pPos, mPos);

		if (distance < 5f)
		{
			return Distance.near;
		}
		else if (distance < 10f)
		{
			return Distance.middle;
		}
		else
		{
			return Distance.far;
		}



	}

	/// <summary>
	/// 状態
	/// </summary>
	private enum State
	{
		dead = 0,
		wait = 1,
		move = 2,
		attack = 4,
		attackWait = 5,
		escape = 8,
	}

	/// <summary>
	/// 状態確認
	/// </summary>
	/// <param name="check"></param>
	private bool Check(State check)
	{
		return state == check;
	}

}


