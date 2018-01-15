using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunEnemy : Enemy
{
    public float setSpeed;//発射位置に向かう速度

    private GameManager gameManager;
    private Vector2 shootPos;//発射位置
    private bool isShootSet;//発射準備フラグ
    private bool isShoot;//発射フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        isShootSet = false;
        isShoot = false;
        gameManager = FindObjectOfType<GameManager>();
        gameManager.LayerCollision("Enemy", "Enemy", true);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void EnemyUpdate()
    {
        MoveToPoint();
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
        if (isShootSet)
        {
            GetComponent<FollowPlayer>().MoveStop();
            GetComponent<Collider2D>().enabled = false;
        }
        if (GetComponent<Dash>().IsActive && ai_classes[0].enabled)
        {
            GetComponent<Collider2D>().enabled = true;
            ai_classes[1].Stop();
            ai_classes[2].Stop();
        }

        if (isStan)
        {
            gameObject.layer = 0;
            foreach (var ai in ai_classes)
            {
                ai.Stop();
            }
        }
    }

    /// <summary>
    /// 発射位置に向かう
    /// </summary>
    private void MoveToPoint()
    {
        if (!isShootSet || isShoot) return;

        transform.position = Vector2.Lerp(transform.position, shootPos, setSpeed * Time.deltaTime);
        float distance = Vector2.Distance(transform.position, shootPos);
        if (distance <= 0.5f)
        {
            transform.position = shootPos;
            isShoot = true;
        }
    }

    /// <summary>
    /// 発射準備
    /// </summary>
    public void SetShoot(Vector2 shootPos)
    {
        this.shootPos = shootPos;
        isShootSet = true;
    }

    /// <summary>
    /// 発射準備フラグ
    /// </summary>
    public bool IsShootSet
    {
        get { return isShootSet; }
    }

    /// <summary>
    /// 発射フラグ
    /// </summary>
    public bool IsShoot
    {
        get { return isShoot; }
    }
}
