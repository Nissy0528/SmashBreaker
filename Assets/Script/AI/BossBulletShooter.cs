using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletShooter : AI
{
    public GameObject bullet;
    public GameObject[] shotPos;
    public float t_rate = 2f;

    private float t_count = 0f;

    // Use this for initialization
    public override void Initialize()
    {
    }

    protected override void AIUpdate()
    {
        //テスト用
        Test();
    }

    /// <summary>
    /// テスト用
    /// </summary>
    private void Test()
    {
        t_count += Time.deltaTime;

        if (t_rate < t_count)
        {
            t_count = 0;

            //var rot = 0f;
            //for(var i = 0; i < bulletCount; i++)
            //{
            //	rot = i * (360f/bulletCount);

            //	Shot(rot);
            //}

            for (int i = 0; i < shotPos.Length; i++)
            {
                Instantiate(bullet, shotPos[i].transform.position, shotPos[i].transform.rotation);
            }

        }

    }


    ///// <summary>
    ///// 発射処理
    ///// </summary>
    ///// <param name="rotation">発射角(実数)</param>

    //public void Shot(float rotation)
    //{
    //	Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * rotation);
    //	Vector3 popPoint = transform.position + rot * (Vector3.up);
    //	var bInstant = Instantiate<GameObject>(bullet[0], popPoint, Quaternion.identity);
    //	bInstant.transform.rotation = rot;
    //}

    ///// <summary>
    ///// 発射処理
    ///// </summary>
    ///// <param name="index">発射する弾丸</param>
    /////<param name="rotation">発射角(実数)</param> 
    //public void Shot( int index , float rotation)
    //{
    //	Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * rotation);
    //	Vector3 popPoint = transform.position + rot * ( Vector3.up);
    //	var bInstant = Instantiate<GameObject>(bullet[index], popPoint, Quaternion.identity);
    //	bInstant.transform.rotation = rot;
    //}
}
