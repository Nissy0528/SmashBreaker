using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBoss : Boss
{
    public GameObject barrier;//バリアオブジェクト

    private GameObject[] enemys;//フィールド上の敵
    private GameObject barrierObj;//バリアオブジェクト

    // Use this for initialization
    public override void Initialize()
    {
        barrierObj = Instantiate(barrier);
    }

    // Update is called once per frame
    public override void BossUpdate()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    ///// <summary>
    ///// 人工知能
    ///// </summary>
    //public override void AI()
    //{
    //    if (enemys.Length > 0)
    //    {
    //        GetComponent<FollowPlayer>().MoveStop();
    //        ai_classes[1].Stop();
    //        SetEnemyDash(enemys);
    //    }
    //    else
    //    {
    //        GetComponent<FollowPlayer>().ReMove();
    //        ai_classes[1].enabled = true;
    //    }

    //    if (HP <= 0 || isStan)
    //    {
    //        foreach (var ai in ai_classes)
    //        {
    //            ai.Stop();
    //        }
    //    }
    //}

    ///// <summary>
    ///// 硬直
    ///// </summary>
    //public override void Stan()
    //{
    //    if (!isStan) return;

    //    if (stanDelay == stanTime)
    //    {
    //        AnimBool("Razer", false);
    //        AnimBool("Hit", true);
    //        EnemyDead();
    //        Destroy(barrierObj);
    //        barrierObj = Instantiate(barrier);
    //    }
    //    stanDelay -= Time.deltaTime;

    //    if (stanDelay <= 0.0f)
    //    {
    //        AnimBool("Hit", false);
    //        isStan = false;
    //        stanDelay = stanTime;
    //        foreach (var ai in ai_classes)
    //        {
    //            ai.enabled = true;
    //        }
    //        GetComponent<FollowPlayer>().ReMove();
    //    }
    //}


    ///// <summary>
    ///// 雑魚敵の突撃パラメーター設定
    ///// </summary>
    //private void SetEnemyDash(GameObject[] enemys)
    //{
    //    for (int i = 0; i < enemys.Length; i++)
    //    {
    //        enemys[i].transform.parent = null;
    //        Dash dash = enemys[i].GetComponent<Dash>();
    //        if (!dash.isStart)
    //        {
    //            dash.chargeTime *= i + 1;
    //            dash.Initialize();
    //            dash.isStart = true;
    //        }
    //    }
    //}

    ///// <summary>
    ///// 全雑魚敵消滅
    ///// </summary>
    //private void EnemyDead()
    //{
    //    foreach (var e in enemys)
    //    {
    //        e.GetComponent<Enemy>().Dead();
    //    }
    //}
}
