using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSun : Boss
{
    public CircleBulletShooter enemyBulletClass;
    public float enemySetTime;//雑魚敵を発射位置に向かわせる時間
    public float length;//プレイヤーとの距離

    private FollowPlayer followClass;
    private Dash dashClass;
    private RazerShooter razerClass;
    private CircleBulletShooter bulletClass;
    private EnemySpawner spawner;//雑魚敵生成クラス
    private GameObject[] enemys;//フィールド上の敵
    private GameObject[] shootPoints;//雑魚敵発射位置
    private GameObject point;
    private float enemySetCount;
    private int enemyCount;
    private int enemyNum;
    private bool isShoot;//雑魚敵発射フラグ
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
        dashClass = GetComponent<Dash>();
        dashClass.dashInterval = 0f;
        razerClass = GetComponent<RazerShooter>();
        bulletClass = GetComponent<CircleBulletShooter>();
        spawner = GetComponent<EnemySpawner>();

        stanDelay = stanTime;

        ShadowSet();
        state = State.move;

        spawner.enemyRange = enemyBulletClass.bulletCount;
        enemySetCount = enemySetTime;
        enemyNum = 0;
        point = new GameObject();
        point.tag = "Barrier";
        isShoot = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void BossUpdate()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        shootPoints = GameObject.FindGameObjectsWithTag("ShootPoint");
        point.transform.position = transform.position;

        if (HP <= 0 || isStan)
        {
            followClass.Stop();
            dashClass.Stop();
            razerClass.Stop();
            bulletClass.Stop();
            enemyBulletClass.Initialize();
            state = State.move;
        }
        else
        {
            Move();
            Attack();

            isShoot = true;
            foreach (var e in enemys)
            {
                if (!e.GetComponent<SunEnemy>().IsShoot)
                {
                    isShoot = false;
                }
            }
            EnemyShootSet();
            EnemyShoot();
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
            spawner.Stop();
            return;
        }

        if (!smashGage.IsMax)
        {
            followClass.enabled = true;
            spawner.enabled = true;
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
            }
            if (enemyBulletClass.IsActive)
            {
                if (razerClass.IsEnd())
                {
                    razerClass.Stop();
                }
                if (enemys.Length == 0)
                {
                    ClearShootPoint();
                    enemyBulletClass.Initialize();
                    state = State.move;
                }
            }
            return;
        }
        else if (!Check(State.attack))
        {
            dashClass.Stop();
            razerClass.Stop();
            bulletClass.Stop();
            enemyBulletClass.Initialize();
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
        if (smashGage.IsMax)
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
                    bulletClass.enabled = true;
                    break;
                case Distance.far:
                    razerClass.enabled = true;
                    if (!enemyBulletClass.IsActive)
                    {
                        enemyBulletClass.CreateBullets();
                    }
                    bulletClass.Stop();
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

        if (distance < length)
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
        if (!enemyBulletClass.IsActive || isShoot)
        {
            enemySetCount = enemySetTime;
            enemyNum = 0;
            return;
        }

        enemySetCount -= Time.deltaTime;
        if (enemySetCount <= 0.0f && enemyNum < enemys.Length)
        {
            SunEnemy sunEnemy = enemys[enemyNum].GetComponent<SunEnemy>();
            Vector2 point = shootPoints[enemyNum].transform.position;
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
        if (!isShoot) return;

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


