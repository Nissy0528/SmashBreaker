using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;//敵
    public Sprite[] sprites;//切り替える画像
    public float spawnTime;//敵生成時間（設定用）
    public int enemyRange;//敵生成の最大数

    private List<GameObject> enemys = new List<GameObject>();//現在ゲーム上にいる敵
    private GameObject[] enemyBorns;//現在ゲーム上にある敵登場エフェクト
    private Vector3 spawnPos;//生成位置
    //private PlayerHP playerHP;//プレイヤー体力UI
    private Sprite sprite;//画像
    private float spawnDelay;//敵生成時間
    private float animCnt;
    private bool animStop;//アニメーション停止フラグ

    // Use this for initialization
    void Start()
    {
        spawnDelay = spawnTime;
        spawnPos = transform.GetChild(0).position;
        //playerHP = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
        sprite = transform.Find("Mouth").GetComponent<SpriteRenderer>().sprite;
        sprite = sprites[1];
        transform.Find("Mouth").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();//敵生成
        Anim();

        enemys.RemoveAll(x => x == null);//敵リストの空の要素を削除
    }

    /// <summary>
    /// 敵生成
    /// </summary>
    private void Spawn()
    {
        //敵の数が指定数以上になったら生成しない
        if (enemys.Count >= enemyRange) return;

        if (spawnDelay > 0.0f)
        {
            spawnDelay -= Time.deltaTime;
        }

        if (spawnDelay <= 0.0f)
        {
            //敵を生成位置に生成
            GameObject enemyObj = Instantiate(enemy);
            enemyObj.transform.position = spawnPos;
            enemys.Add(enemyObj);
            sprite = sprites[0];
            spawnDelay = spawnTime;
        }
    }

    /// <summary>
    /// アニメーション開始
    /// </summary>
    private void Anim()
    {
        if (sprite == sprites[0])
        {
            animCnt += Time.deltaTime;
            if (animCnt >= spawnTime / 2f)
            {
                sprite = sprites[1];
                animCnt = 0.0f;
            }
        }

        transform.Find("Mouth").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.transform.tag == "Attack")
    //    {
    //        //Destroy(gameObject);
    //    }
    //}
}
