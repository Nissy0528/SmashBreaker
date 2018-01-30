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
    public FollowPlayer lookClass;//プレイヤーを見るクラス

    private Rigidbody2D rigid;
    private Vector2 currentRV;
    private CircleBulletShooter enemyBulletClass;//雑魚敵発射クラス
    private BossBulletShooter bulletClass;//弾発射クラス
    private Dash dashClass;//突撃クラス
    private Rotation rotateClass;//回転クラス
    private int dashCount;//突撃した回数
    private float bulletCount;//弾発射時間
    private bool isStartMove;//初期加速フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        StartMove();

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
        AI();
        StartMove();
        for(int i=0;i<enemys.Length;i++)
        {
            enemys[i].GetComponent<BulletEnemy>().SetNumber(i);
        }
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
            if (dashCount <= 2)
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
            lookClass.Stop();
            dashCount = 0;
            bulletCount = bulletTime;
            currentRV = Vector2.zero;
            isStartMove = false;
        }
        else
        {
            rigid.velocity = currentRV;
            lookClass.enabled = true;
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
    /// 初期加速
    /// </summary>
    private void StartMove()
    {
        if (isStartMove) return;

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

        isStartMove = true;
    }
}
