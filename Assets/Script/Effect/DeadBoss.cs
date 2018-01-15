using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoss : MonoBehaviour
{
    public GameObject explosion;//爆発エフェクト
    public float time;//演出時間
    public float spawnTime;//爆発エフェクト生成間隔
    public float range = 0.2f;//ランダムの範囲

    private Vector2 spawnPos;//爆発エフェクト生成座標
    private float spawnCount;//爆発エフェクト生成カウント

    // Use this for initialization
    void Start()
    {
        spawnCount = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        SpawnExplosion();
        if (time <= 0.0f)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 爆発エフェクト生成
    /// </summary>
    private void SpawnExplosion()
    {
        if (time <= 0.0f) return;

        spawnCount -= Time.deltaTime;
        if (spawnCount <= 0.0f)
        {
            float x = Random.Range(-range, range);
            float y = Random.Range(-range, range);
            spawnPos = new Vector2(x, y);
            transform.localPosition = spawnPos;
            GameObject obj = Instantiate(explosion);
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
            spawnCount = spawnTime;
        }
    }
}
