using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBoss : Boss
{
    public GameObject enemy;//生成する雑魚敵
    public float enemySpawnTime;//雑魚敵生成時間
    public float enemySetTime;//雑魚敵を発射位置に向かわせる時間
    public float enemyShootLength;//雑魚敵を飛ばし始める距離

    private GameObject[] enemys;//フィールド上の敵
    private GameObject[] shootPoints;//雑魚敵発射位置
    private GameObject point;
    private float enemySpawnCount;//雑魚敵生成カウント
    private float enemySetCount;
    private float length;//プレイヤーとの距離
    private int enemyCount;
    private int enemyNum;
    private bool isShoot;//雑魚敵発射フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        ai_classes[1].Stop();
        enemySpawnCount = enemySpawnTime;
        enemyCount = ai_classes[1].GetComponent<CircleBulletShooter>().bulletCount;
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
        length = Vector2.Distance(transform.position, player.transform.position);
        point.transform.position = transform.position;

    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
        if (HP <= 0 || isStan)
        {
            foreach (var ai in ai_classes)
            {
                ai.Stop();
            }
            ai_classes[1].Initialize();
        }
        else
        {
            if (length >= enemyShootLength && enemys.Length > 0)
            {
                GetComponent<FollowPlayer>().MoveStop();
                if (!GetComponent<CircleBulletShooter>().IsActive)
                {
                    GetComponent<CircleBulletShooter>().CreateBullets();
                }
            }
            else if (enemys.Length == 0)
            {
                GetComponent<FollowPlayer>().ReMove();
                ai_classes[1].Initialize();
            }

            if (!GetComponent<CircleBulletShooter>().IsActive && length < enemyShootLength)
            {
                SpawnEnemy();
            }

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
            foreach (var ai in ai_classes)
            {
                ai.enabled = true;
            }
            ai_classes[1].Stop();
        }
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
    /// 雑魚敵生成
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemySpawnCount > 0.0f)
        {
            enemySpawnCount -= Time.deltaTime;
            return;
        }

        if (enemys.Length < enemyCount)
        {
            Vector2 min = gameManager.StageMinPos;
            Vector2 max = gameManager.StageMaxPos;
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            GameObject enemyObj = Instantiate(enemy);
            enemyObj.transform.position = new Vector2(x, y);
            enemySpawnCount = enemySpawnTime;
        }
    }

    /// <summary>
    /// 雑魚敵発射準備
    /// </summary>
    private void EnemyShootSet()
    {
        if (!GetComponent<CircleBulletShooter>().IsActive || isShoot)
        {
            enemySetCount = enemySetTime;
            enemyNum = 0;
            return;
        }

        enemySetCount -= Time.deltaTime;
        if (enemySetCount <= 0.0f)
        {
            SunEnemy sunEnemy = enemys[enemyNum].GetComponent<SunEnemy>();
            Vector2 point = shootPoints[enemyNum].transform.position;
            sunEnemy.SetShoot(point);
            if (enemyNum < enemys.Length - 1)
            {
                enemyNum++;
            }
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
}
