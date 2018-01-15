using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemy : Enemy
{
    private GameManager gameManager;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.LayerCollision("Enemy", "Barrier", true);
        gameManager.LayerCollision("Enemy", "Enemy", true);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void EnemyUpdate()
    {
        base.EnemyUpdate();
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
        if (ai_classes[0].IsActive)
        {
            GetComponent<Collider2D>().enabled = true;
            ai_classes[1].Stop();
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
}
