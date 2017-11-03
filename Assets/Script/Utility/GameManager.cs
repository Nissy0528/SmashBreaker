using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float stopTime;//ゲームストップの長さ
    public GameObject gameover;//ゲームオーバーUI
    public GameObject smashText;//スマッシュUI
    public GameObject enemyManager;//エネミーマネージャー

    private float stopDelay;//ゲーム停止時間
    private Player player;//プレイヤー
    private GameObject[] bossObjs;//ボス
    private GameObject[] enemys;//敵

    // Use this for initialization
    void Start()
    {
        stopDelay = stopTime;
        player = GameObject.Find("Chara").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();//ゲーム再開
        ShowGameOver();//ゲームオーバー表示
    }

    /// <summary>
    /// ゲーム再開
    /// </summary>
    private void GameStart()
    {
        if (Time.timeScale >= 1.0f || smashText.activeSelf) return;

        //指定した時間まで達したらゲーム再開
        stopDelay -= 0.1f;
        if (stopDelay <= 0.0f)
        {
            stopDelay = stopTime;
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    private void ShowGameOver()
    {
        if (player.hp > 0) return;

        gameover.SetActive(true);
        //リトライボタンが押されたらMainシーンを再読み込み
        if (Input.GetButtonDown("Decision"))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
