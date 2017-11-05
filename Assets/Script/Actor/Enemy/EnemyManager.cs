using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyBorn;//敵登場エフェクト
    public float spawnSpeed;//敵生成速度
    public int enemyRange;//敵生成の最大数

    private GameObject[] enemys;//現在ゲーム上にいる敵
    private GameObject[] enemyBorns;//現在ゲーム上にある敵登場エフェクト
    private MainCamera camera;//カメラ
    private Vector3 spawnPos;//生成位置
    private float spawnDelay;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        spawnDelay = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");//ゲーム上にいる敵取得
        enemyBorns = GameObject.FindGameObjectsWithTag("EnemyBorn");//ゲーム上にある敵登場エフェクト
        Spawn();//敵生成
    }

    /// <summary>
    /// 敵生成
    /// </summary>
    private void Spawn()
    {
        //敵の数が指定数以上になったら生成しない
        if (enemys.Length >= enemyRange || enemyBorns.Length >= enemyRange) return;

        if (spawnDelay > 0.0f)
        {
            spawnDelay -= Time.deltaTime;
        }

        if (spawnDelay <= 0.0f)
        {
            spawnPos.x = Random.Range(camera.ScreenMin.x, camera.ScreenMax.x);
            spawnPos.y = Random.Range(camera.ScreenMin.y, camera.ScreenMax.y);

            //敵を生成位置に生成
            GameObject enemyBornObj = Instantiate(enemyBorn);
            enemyBornObj.transform.position = spawnPos;
            spawnDelay = spawnSpeed;
        }
    }
}
