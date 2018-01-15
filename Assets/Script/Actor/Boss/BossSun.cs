using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSun : Enemy
{
	public float stanTime;//硬直時間
	public GameObject cutIn;
	public GameObject deadUI;//死亡時の演出UI

	private Animator anim;//アニメーション
	private NormalEnemy followClass;
	private Dash dashClass;
	private RazerShooter razerClass;

	private List<AI> move;
	private List<AI> attack;

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
	private readonly float attackTime = 10f;


	// Use this for initialization
	void Start()
	{
		gameObject.tag = "Boss";
		anim = transform.Find("Chara").GetComponent<Animator>();

		state = State.wait;
		move = new List<AI>()
		{
			GetComponent<NormalEnemy>()
		};

		attack = new List<AI>
		{
			GetComponent<Dash>(),
			GetComponent<RazerShooter>(),
		};

		gameManager = FindObjectOfType<GameManager>();
		//影
		ShadowSet();
		Initialize();
		stanDelay = stanTime;

		gage = player.GetComponent<Player>().smashGage;
	}
	
	protected override void EnemyUpdate()
	{
		Move();
		Attack();

		//Stan();//硬直
		BossDead();//消滅
	}


	/// <summary>
	/// 移動
	/// </summary>
	private void Move()
	{
		if (!Check(State.move))
		{
			foreach (var m in move)
			{
				m.Stop();
			}
			return;
		}

		if (!gage.IsMax)
		{
			move[0].enabled = true;
		}
		else
		{

		}

		attackCount += Time.deltaTime;
		if(attackCount >= attackTime)
		{
			attackCount = 0;
			StateChange(State.attack);
		}
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	private void Attack()
	{
		if (!Check(State.attack))
		{
			foreach (var a in attack)
			{
				a.Stop();
			}
			return;
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
	/// 影の設定を有効にする
	/// </summary>
	private void ShadowSet()
	{
		var child = transform.GetChild(0);
		var sr = child.GetComponent<SpriteRenderer>();
		sr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}

	/// <summary>
	/// 状態
	/// </summary>
	public enum State
	{
		dead = 0,
		wait = 1,
		move = 2,
		attack = 4,
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

	/// <summary>
	/// ステートを変える
	/// </summary>
	/// <param name="value"></param>
	private void StateChange(State value)
	{
		state = value;
	}

}


