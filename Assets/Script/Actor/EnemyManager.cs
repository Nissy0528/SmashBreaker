using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;//敵オブジェクト
    public float spawnSpeed;//敵生成速度
    public int enemyRange;//敵生成の最大数

    private GameObject[] enemys;//現在ゲーム上にいる敵
    private MainCamera camera;//カメラ
    private Vector3 spawnPos;//生成位置
    private float spawnDelay;
    private int spawnPosNum;

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
        Spawn();//敵生成
    }

    /// <summary>
    /// 敵生成
    /// </summary>
    private void Spawn()
    {
        //敵の数が指定数以上になったら生成しない
        if (enemys.Length >= enemyRange) return;

        if (spawnDelay > 0.0f)
        {
            spawnDelay -= Time.deltaTime;
        }

        if (spawnDelay <= 0.0f)
        {
            spawnPosNum = Random.Range(0, 4);
            switch(spawnPosNum)
            {
                //画面外の上に座標を指定
                case 0:
                    spawnPos.x = Random.Range(camera.ScreenMin().x, camera.ScreenMax().x);
                    spawnPos.y = camera.ScreenMax().y;
                    break;
                //画面外の右に座標を指定
                case 1:
                    spawnPos.x = camera.ScreenMax().x;
                    spawnPos.y = Random.Range(camera.ScreenMin().y, camera.ScreenMax().y);
                    break;
                //画面外の下に座標を指定
                case 2:
                    spawnPos.x = Random.Range(camera.ScreenMin().x, camera.ScreenMax().x);
                    spawnPos.y = camera.ScreenMin().y;
                    break;
                //画面外の左に座標を指定
                case 3:
                    spawnPos.x = camera.ScreenMin().x;
                    spawnPos.y = Random.Range(camera.ScreenMin().y, camera.ScreenMax().y);
                    break;
            }

            //敵を生成位置に生成
            GameObject enemyObj = Instantiate(enemy);
            enemyObj.transform.position = spawnPos;
            spawnDelay = spawnSpeed;
        }
    }
}
