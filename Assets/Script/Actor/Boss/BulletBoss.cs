using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : Boss
{
    public float speed;//初期速度
    public int spawnCount;//敵を生成する数（通常時）
    public int spawnCountMax;//敵を生成する数（スマッシュゲージマックス時）
    public float bulletTime;//弾発射時間
    public PhysicsMaterial2D p_mat;//フィジックマテリアル

    private Rigidbody2D rigid;
    private Vector2 currentRV;
    private GameObject[] enemys;//フィールド上の雑魚敵
    private CircleBulletShooter enemyBulletClass;//雑魚敵発射クラス
    private BossBulletShooter bulletClass;//弾発射クラス
    private Dash dashClass;//突撃クラス
    private Rotation rotateClass;//回転クラス
    private int dashCount;//突撃した回数
    private float bulletCount;//弾発射時間

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        //ランダム方向に加速
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        Vector2 v = new Vector2(x, y).normalized;

        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        rigid.sharedMaterial = p_mat;
        rigid.drag = 0.0f;
        rigid.AddForce(v * speed, ForceMode2D.Impulse);
        currentRV = rigid.velocity;

        enemyBulletClass = GetComponent<CircleBulletShooter>();
        bulletClass = GetComponent<BossBulletShooter>();
        dashClass = GetComponent<Dash>();
        rotateClass = GetComponent<Rotation>();

        dashCount = 0;
        bulletCount = bulletTime;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void BossUpdate()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        AI();
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    private void AI()
    {
        if (!smashGage.IsMax)
        {
            enemyBulletClass.bulletCount = spawnCount;
            bulletClass.Stop();
            dashClass.Stop();
            rotateClass.Stop();
            dashCount = 0;
            bulletCount = bulletTime;
            rigid.sharedMaterial = p_mat;
            rigid.drag = 0.0f;
        }
        else
        {
            rigid.sharedMaterial = null;
            rigid.drag = 2.0f;
            if (enemyBulletClass.bulletCount != spawnCountMax)
            {
                currentRV = Vector2.zero;
                rigid.velocity = currentRV;
                enemyBulletClass.bulletCount = spawnCountMax;
            }
            if (dashCount <= 3)
            {
                bulletCount = bulletTime;
                dashClass.enabled = true;
                currentRV = rigid.velocity;
                if (dashClass.IsEnd())
                {
                    dashClass.Stop();
                    dashCount++;
                }
            }
            else
            {
                bulletClass.enabled = true;
                rotateClass.enabled = true;
                bulletCount -= Time.deltaTime;
                if (bulletCount <= 0.0f)
                {
                    bulletClass.Stop();
                    rotateClass.Stop();
                    dashCount = 0;
                }
            }
        }

        if (enemys.Length > 0)
        {
            enemyBulletClass.Stop();
        }
        else
        {
            enemyBulletClass.enabled = true;
        }

        if (HP <= 0 || isStan)
        {
            enemyBulletClass.Stop();
            bulletClass.Stop();
            dashClass.Stop();
            rotateClass.Stop();
            dashCount = 0;
            bulletCount = bulletTime;
            currentRV = Vector2.zero;
        }
        else
        {
            rigid.velocity = currentRV;
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
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    public override void Collision(Collision2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Pillar")
        {
            currentRV = rigid.velocity;
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
