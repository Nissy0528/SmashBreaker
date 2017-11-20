using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float stopTime;//ゲームストップの長さ
    public GameObject gameover;//ゲームオーバーUI
    public GameObject gameclear;//ゲームクリアUI
    public GameObject smashText;//スマッシュUI
    public GameObject enemyManager;//エネミーマネージャー

    private float stopDelay;//ゲーム停止時間
    private Player player;//プレイヤー
    private GameObject bossObj;//ボス
    private MainCamera camera;//カメラ

	/// <summary>
	/// ワープゾーン
	/// </summary>
	private SceneWarpZone warpZone;

    // Use this for initialization
    void Start()
    {
        stopDelay = stopTime;
        player = GameObject.Find("Chara").GetComponent<Player>();
        bossObj = GameObject.FindGameObjectWithTag("Boss");
        camera = GameObject.Find("Main Camera").GetComponent<MainCamera>();

		warpZone = FindObjectOfType<SceneWarpZone>();
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();//ゲーム再開
        ShowGameOver();//ゲームオーバー表示
        ShowGameClear();//ゲームクリア表示
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
        if (player.GetParam.hp > 0) return;

        gameover.SetActive(true);
        Time.timeScale = 0.0f;
        //リトライボタンが押されたらMainシーンを再読み込み
        if (Input.GetButtonDown("Decision"))
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Main");
        }
    }

    /// <summary>
    /// ゲームクリア表示
    /// </summary>
    private void ShowGameClear()
    {
        if (bossObj != null || !camera.IsShakeFinish) return;

		warpZone.gameObject.SetActive(true);
        //gameclear.SetActive(true);
        //Time.timeScale = 0.0f;
        ////リトライボタンが押されたらMainシーンを再読み込み
        //if (Input.GetButtonDown("Decision"))
        //{
        //    Time.timeScale = 1.0f;
        //    SceneManager.LoadScene("Main");
        //}
    }
}
