using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBoss : Boss
{
    public float moveLength;//逃げる距離
    public float drillCount;//ドリルの数

    private GameObject[] enemys;//フィールド上の雑魚敵
    private Object[] drills;//フィールド上のドリル
    private BossBulletShooter bulletClass;//弾発射クラス
    private EnemySpawner spawnClass;//雑魚敵生成クラス
    private Translation moveClass;//平行移動クラス
    private FollowPlayer lookClass;//プレイヤーの方向に向くクラス
    private float length;//プレイヤーとの距離

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        bulletClass = GetComponent<BossBulletShooter>();
        spawnClass = GetComponent<EnemySpawner>();
        moveClass = GetComponent<Translation>();
        lookClass = GetComponent<FollowPlayer>();

        moveClass.enabled = true;
        Vector2 screenMaxPos = gameManager.StageMaxPos;
        Vector2[] positions = new Vector2[] { screenMaxPos };
        moveClass.Positions = positions;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void BossUpdate()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        drills = FindObjectsOfType<GrowDrillEnemy>();
        length = Vector2.Distance(transform.position, player.transform.position);
        AI();
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    private void AI()
    {
        NormalAI();

        if (HP <= 0 || isStan)
        {
            bulletClass.Stop();
            spawnClass.Stop();
            moveClass.Stop();
        }
    }

    /// <summary>
    /// 通常時の人口知能
    /// </summary>
    private void NormalAI()
    {
        if (smashGage.IsMax) return;

        if (moveClass.IsActive && moveClass.enabled)
        {
            if (drills.Length < drillCount)
            {
                bulletClass.enabled = true;
            }
            else
            {
                bulletClass.Stop();
            }
            bulletClass.enabled = true;
            lookClass.enabled = true;
            moveClass.Stop();
        }

        if (length <= moveLength && !moveClass.enabled)
        {
            bulletClass.Stop();
            spawnClass.Stop();
            lookClass.Stop();
            moveClass.enabled = true;
            SetMovePos();
        }
    }

    /// <summary>
    /// スマシュゲージマックス時の人工知能
    /// </summary>
    private void MaxAI()
    {
        if (!smashGage.IsMax) return;
    }

    /// <summary>
    /// 逃走時の座標設定
    /// </summary>
    private void SetMovePos()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 position = transform.position;
        Vector2 firstPos = Vector2.zero;
        float x = Mathf.Abs(playerPos.x) / Mathf.Abs(position.x);
        float y = Mathf.Abs(playerPos.y) / Mathf.Abs(position.y);
        if (y <= x)
        {
            firstPos = new Vector2(position.x *= -1.0f, position.y);
        }
        if (y > x)
        {
            firstPos = new Vector2(position.x, position.y *= -1.0f);
        }
        Vector2[] positions = new Vector2[] { firstPos };
        moveClass.Positions = positions;
        moveClass.Initialize();
    }

    /// <summary>
    /// 硬直
    /// </summary>
    public override void Stan()
    {
        if (!isStan) return;

        if (stanDelay == stanTime)
        {
            EnemyDead();
        }
        stanDelay -= Time.deltaTime;

        if (stanDelay <= 0.0f)
        {
            isStan = false;
            stanDelay = stanTime;
            Initialize();
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
}
