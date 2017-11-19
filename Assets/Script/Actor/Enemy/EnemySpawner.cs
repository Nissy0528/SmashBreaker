using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;//敵
    public float spawnTime;//敵生成時間（設定用）
    public int enemyRange;//敵生成の最大数

    private List<GameObject> enemys = new List<GameObject>();//現在ゲーム上にいる敵
    private GameObject[] enemyBorns;//現在ゲーム上にある敵登場エフェクト
    private Vector3 spawnPos;//生成位置
    private PlayerHP playerHP;//プレイヤー体力UI
    private TextureAnimator animation_chid;//アニメーションするオブジェクト
    private float spawnDelay;//敵生成時間
    private bool animStop;//アニメーション停止フラグ

    // Use this for initialization
    void Start()
    {
        spawnDelay = spawnTime;
        spawnPos = transform.GetChild(0).position;
        playerHP = GameObject.Find("PlayerHP").GetComponent<PlayerHP>();
        animation_chid = transform.GetChild(1).GetComponent<TextureAnimator>();
        animation_chid.CurrentTex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        StopAnim();
        Spawn();//敵生成

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
            spawnDelay = spawnTime;
            SetAnim();
        }
    }

    /// <summary>
    /// アニメーション開始
    /// </summary>
    private void SetAnim()
    {
        if (animation_chid.IsPlay) return;

        animation_chid.SetTime = spawnTime / 2f;
        animation_chid.IsPlay = true;
        animation_chid.IsLoop = true;
        animation_chid.NextTex();
        animStop = false;
    }

    /// <summary>
    /// アニメーション停止
    /// </summary>
    private void StopAnim()
    {
        if (enemys.Count < enemyRange) return;

        if (animation_chid.CurrentTex == 0)
        {
            animStop = true;
        }
        if (animStop && animation_chid.CurrentTex == 1)
        {
            animation_chid.Stop();
        }
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Attack" && playerHP.IsHPMax)
        {
            Destroy(gameObject);
        }
    }
}
