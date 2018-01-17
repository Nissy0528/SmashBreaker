using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : Enemy
{
    public float spawnSpeed;//生成時の移動速度
    public float startTime;//行動開始時間

    private Rigidbody2D rigid;
    private FollowPlayer followClass;//プレイヤー追従クラス
    private Vector2 spawnDirec;//生成時に移動する方向
    private bool isStart;//行動開始フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        followClass = GetComponent<FollowPlayer>();
        rigid = GetComponent<Rigidbody2D>();

        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        spawnDirec = new Vector2(x, y);
        rigid.AddForce(spawnDirec * spawnSpeed, ForceMode2D.Impulse);

        isStart = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void EnemyUpdate()
    {
        startTime -= Time.deltaTime;
        if (!isStart && startTime <= 0.0f)
        {
            rigid.velocity = Vector2.zero;
            followClass.enabled = true;
            isStart = true;
        }
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    public override void AI()
    {
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
