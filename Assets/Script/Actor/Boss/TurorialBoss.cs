using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurorialBoss : Boss
{
    private RazerShooter razerClass;//レーザークラス
    private EnemySpawner spawner;//雑魚敵生成クラス
    private FollowPlayer followClass;//プレイヤー追従クラス（プレイヤーの方向に向かせる用）
    private GameObject[] enemys;//フィールド上の敵

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        razerClass = GetComponent<RazerShooter>();
        spawner = GetComponent<EnemySpawner>();

        Transform child = transform.GetChild(0);
        followClass = child.Find("Muzzle").GetComponent<FollowPlayer>();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public override void BossUpdate()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (HP <= 0 || isStan)
        {
            razerClass.Stop();
            spawner.Stop();
            followClass.Stop();
        }
        else
        {
            razerClass.enabled = true;
            spawner.enabled = true;
            followClass.enabled = true;

            if (razerClass.IsFire())
            {
                followClass.Stop();
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
