using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurorialBoss : Boss
{
    private RazerShooter razerClass;//レーザークラス
    private EnemySpawner spawner;//雑魚敵生成クラス
    private FollowPlayer followClass;//プレイヤー追従クラス（プレイヤーの方向に向かせる用）

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
}
