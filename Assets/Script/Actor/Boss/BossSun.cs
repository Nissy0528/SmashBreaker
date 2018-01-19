using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSun : Boss
{
	[SerializeField]
	private CircleBulletShooter enemyBulletClass;
	public float enemySetTime;//雑魚敵を発射位置に向かわせる時間
	public float farlength;//プレイヤーとの距離

	private FollowPlayer followClass;
	private DistansePlayer distanseClass;
	private Dash dashClass;
	private CircleBulletShooter bulletClass;

	private RazerShooter razerClass;
	private EnemySpawner spawner;//雑魚敵生成クラス
	private GameObject[] enemys;//フィールド上の敵
	private GameObject[] shootPoints;//雑魚敵発射位置
	private GameObject point;
	private float enemySetCount;
	private int enemyCount;
	private int enemyNum;
	private bool isEnemyShootSet;//雑魚敵発射準備
	private bool isEnemyShoot;//雑魚敵発射フラグ
	private bool isShootEnd;
	private State state;

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
	public override void Initialize()
	{
		gameObject.tag = "Boss";

		followClass = GetComponent<FollowPlayer>();
		distanseClass = GetComponent<DistansePlayer>();
		distanseClass.keepDistanse = farlength;

		dashClass = GetComponent<Dash>();
		dashClass.dashInterval = 0f;
		razerClass = GetComponent<RazerShooter>();
		bulletClass = GetComponent<CircleBulletShooter>();
		spawner = GetComponent<EnemySpawner>();
		shootPoints = new GameObject[0];
		stanDelay = stanTime;

		state = State.move;

		spawner.enemyRange = enemyBulletClass.bulletCount;
		enemySetCount = enemySetTime;
		enemyNum = 0;
		point = new GameObject();
		point.tag = "Barrier";

		isEnemyShoot = false;
		isEnemyShootSet = false;

	}

	/// <summary>
	/// 更新
	/// </summary>
	public override void BossUpdate()
	{ 
		enemys = GameObject.FindGameObjectsWithTag("Enemy");

		

		ClearShootPoint();
		enemyBulletClass.CreateBullets();
		shootPoints = GameObject.FindGameObjectsWithTag("ShootPoint");

		point.transform.position = transform.position;

		if (HP <= 0 || isStan)
		{
			followClass.Stop();
			dashClass.Stop();
			razerClass.Stop();
			enemyBulletClass.Initialize();
			state = State.move;
		}
		else
		{
			Move();
			Attack();
		}
	}

	/// <summary>
	/// 移動
	/// </summary>
	private void Move()
	{
		if (!Check(State.move))
		{
			followClass.Stop();
			distanseClass.Stop();
			spawner.Stop();
			return;
		}
		ResetEnemyShoot();

		followClass.enabled = !smashGage.IsMax;
		distanseClass.enabled = smashGage.IsMax;

		spawner.enabled = true;
		isEnemyShoot = false;

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
			state = State.attackStart;
		}
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	private void Attack()
	{
		///攻撃中
		if (Check(State.attackWait))
		{
			AttackWait();
			return;
		}
		//それ以外
		else if (!Check(State.attackStart))
		{
			dashClass.Stop();
			razerClass.Stop();
			bulletClass.Stop();
			enemyBulletClass.Initialize();
			enemyNum = 0;

			return;
		}
		//攻撃開始
		AttackSelect();

		state = State.attackWait;
	}

	private void AttackWait()
	{
		EnemyShootSet();

		var check = true;
		foreach (var e in enemys)
		{
			check = check & e.GetComponent<SunEnemy>().IsShoot;
		}

		if (dashClass.enabled & dashClass.IsEnd())
		{
			dashClass.Stop();
			state = State.move;
		}
		if (razerClass.enabled & razerClass.IsEnd())
		{
			if (check)
			{
				razerClass.Stop();
				state = State.move;
			}
		}

		if(bulletClass.enabled & bulletClass.IsShoot)
		{
			bulletClass.Stop();
			state = State.move;
		}


	}

	/// <summary>
	/// 攻撃の決定
	/// </summary>
	private void AttackSelect()
	{

		//ゲージ満タン時
		if (smashGage.IsMax)
		{
			switch (DistanceCheck())
			{
				case Distance.near:
					DebugCommand.DebugLog("maxnear");
					bulletClass.enabled = true;
					if (!enemyBulletClass.IsActive)
					{
						ClearShootPoint();
						enemyBulletClass.CreateBullets();
					}

					break;
				case Distance.far:
					razerClass.enabled = true;
					if (!enemyBulletClass.IsActive)
					{
						ClearShootPoint();
						enemyBulletClass.CreateBullets();
					}
					isEnemyShootSet = true;
					isEnemyShoot = true;

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
					if (!enemyBulletClass.IsActive)
					{
						ClearShootPoint();
						enemyBulletClass.CreateBullets();
					}
					isEnemyShootSet = true;
					break;
			}
		}


	}

	/// <summary>
	/// 硬直
	/// </summary>
	public override void Stan()
	{
		if (!isStan) return;

		if (stanDelay == stanTime)
		{
			AnimBool("Razer", false);
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

		if (distance < farlength)
		{
			return Distance.near;
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
		attackStart = 4,
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

	/// <summary>
	/// 全雑魚敵消滅
	/// </summary>
	private void EnemyDead()
	{
		foreach (var e in enemys)
		{
			e.GetComponent<Enemy>().Dead();
		}
	}

	/// <summary>
	/// 雑魚敵発射準備
	/// </summary>
	private void EnemyShootSet()
	{
		if (!isEnemyShootSet)
		{
			enemySetCount = enemySetTime;
			enemyNum = 0;
			return;
		}
		else if(enemyNum >= enemys.Length && isEnemyShoot)
		{
			EnemyShoot();
			return;
		}


		enemySetCount -= Time.deltaTime;
		if (enemySetCount <= 0.0f && enemyNum < enemys.Length)
		{
			SunEnemy sunEnemy = enemys[enemyNum].GetComponent<SunEnemy>();
			Vector2 point = shootPoints[enemyNum].transform.position;
			//DebugCommand.DebugLog(sunEnemy.ToString() + "" + enemyNum);

			sunEnemy.SetShoot(point);
			enemyNum++;
			enemySetCount = enemySetTime;
		}
	}

	/// <summary>
	/// 雑魚敵発射
	/// </summary>
	private void EnemyShoot()
	{
		var check = true;
		foreach (var e in enemys)
		{
			check = check & e.GetComponent<SunEnemy>().IsShoot;
		}
		if (!check)
		{
			return;
		}

		foreach (var e in enemys)
		{
			if (!e.GetComponent<Dash>().enabled)
			{
				e.GetComponent<Rotation>().enabled = true;
				e.GetComponent<Dash>().enabled = true;
			}
		}
		SetEnemyDash();
	}

	/// <summary>
	/// 雑魚敵の突撃パラメーター設定
	/// </summary>
	private void SetEnemyDash()
	{
		for (int i = 0; i < enemys.Length; i++)
		{
			enemys[i].transform.parent = null;
			Dash dash = enemys[i].GetComponent<Dash>();
			if (!dash.isStart)
			{
				dash.chargeTime *= i + 1;
				dash.Initialize();
				dash.isStart = true;
			}
		}

	}

	/// <summary>
	/// 敵発射をリセット
	/// </summary>
	private void ResetEnemyShoot()
	{
		if(enemys.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < enemys.Length; i++)
		{
			var e = enemys[i].GetComponent<SunEnemy>();
			if (e != null && e.IsShootSet)
			{
				e.SetShootReset();
			}
		}
		isEnemyShoot = false;
		isEnemyShootSet = false;
	}

	/// <summary>
	/// 雑魚敵発射座標をクリア
	/// </summary>
	private void ClearShootPoint()
	{
		foreach (var s in shootPoints)
		{
			Destroy(s);
		}
	}

}


