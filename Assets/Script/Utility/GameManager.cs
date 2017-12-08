using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int stageNum;//ステージ番号
    public static float time;//経過時間

    public float stopTime;//ゲームストップの長さ
    public GameObject gameover;//ゲームオーバーUI
    public GameObject gameclear;//ゲームクリアUI
    public GameObject smashText;//スマッシュUI
    public GameObject pauseText;//ポーズUI
    public GameObject[] stages;//ステージの配列

    private float stopDelay;//ゲーム停止時間
    private bool isPause;//ポーズフラグ
    private Player player;//プレイヤー
    private GameObject bossObj;//ボス
    private MainCamera mainCamera;//カメラ

    /// <summary>
    /// ワープゾーン
    /// </summary>
    private SceneWarpZone warpZone;

    // Use this for initialization
    void Awake()
    {
        //Instantiate(stages[stageNum]);

        stopDelay = stopTime;
        player = GameObject.Find("Chara").GetComponent<Player>();
        bossObj = GameObject.FindGameObjectWithTag("Boss");
        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();

        warpZone = FindObjectOfType<SceneWarpZone>();
        warpZone.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();//ゲーム再開
        ShowGameOver();//ゲームオーバー表示
        ShowGameClear();//ゲームクリア表示
        Pause();//ポーズ

        if (!warpZone.gameObject.activeSelf && !gameclear.activeSelf)
        {
            time += Time.deltaTime;
        }
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
        if (!player.IsState(Player.State.DEAD)) return;

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
        if (bossObj != null || !mainCamera.IsShakeFinish) return;

        //if (stageNum < stages.Length - 1)
        //{
            warpZone.gameObject.SetActive(true);
        //}
        //else if (!warpZone.gameObject.activeSelf)
        //{
        //    gameclear.SetActive(true);
        //    Time.timeScale = 0.0f;
        //    //リトライボタンが押されたらMainシーンを再読み込み
        //    if (Input.GetButtonDown("Decision"))
        //    {
        //        Time.timeScale = 1.0f;
        //        SceneManager.LoadScene("Title");
        //    }
        //}
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    private void Pause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!isPause)
            {
                ControllerShake.Shake(0.0f, 0.0f);
                isPause = true;
            }
            else
            {
                Time.timeScale = 1.0f;
                isPause = false;
            }
        }

        if (isPause)
        {
            Time.timeScale = 0.0f;
        }

        pauseText.SetActive(isPause);
    }
}
