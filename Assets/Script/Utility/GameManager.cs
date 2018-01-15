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
    public GameObject pauseText;//ポーズUI
    public GameObject[] stages;//ステージの配列
    public AudioSource BGM;

    private float stopDelay;//ゲーム停止時間
    private float controllerShakeTime;
    private bool isPause;//ポーズフラグ
    private bool isDebug;//デバックフラグ
    private Player player;//プレイヤー
    private GameObject bossObj;//ボス
    private MainCamera mainCamera;//カメラ
    private Vector2 stageMinPos;//ステージの左下
    private Vector2 stageMaxPos;//ステージの右上

    /// <summary>
    /// ワープゾーン
    /// </summary>
    private SceneWarpZone warpZone;

    // Use this for initialization
    void Awake()
    {
        if (!isDebug)
        {
            Instantiate(stages[stageNum]);
        }

        stopDelay = stopTime;
        player = FindObjectOfType<Player>();
        bossObj = GameObject.FindGameObjectWithTag("Boss");
        mainCamera = FindObjectOfType<MainCamera>();
        if (stageNum >= 1 || isDebug)
        {
            mainCamera.followSpeed = 0.0f;
            mainCamera.gameObject.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
            BGM.gameObject.SetActive(true);
        }

        warpZone = FindObjectOfType<SceneWarpZone>();

        Time.timeScale = 1.0f;

        SetStageRange();
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();//ゲーム再開
        ShowGameOver();//ゲームオーバー表示
        ShowGameClear();//ゲームクリア表示
        Pause();//ポーズ
        C_Shake();//コントローラー振動

        if (!warpZone.IsNext && !gameclear.activeSelf)
        {
            time += Time.deltaTime;
        }
    }

    /// <summary>
    /// ゲーム再開
    /// </summary>
    private void GameStart()
    {
        if (Time.timeScale >= 1.0f) return;

        //指定した時間まで達したらゲーム再開
        stopDelay -= Time.unscaledDeltaTime;
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

        BGM.Stop();
        Time.timeScale = 0.0f;
        Animator playerAnim = player.GetComponentInChildren<Animator>();
        AnimatorStateInfo playerAnimState = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (playerAnimState.normalizedTime >= 1.0f && playerAnimState.IsName("Player_Dead"))
        {
            playerAnim.enabled = false;
            gameover.SetActive(true);
            //リトライボタンが押されたらMainシーンを再読み込み
            if (Input.GetButtonDown("Decision"))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("Main");
            }
        }
    }

    /// <summary>
    /// ゲームクリア表示
    /// </summary>
    private void ShowGameClear()
    {
        if (bossObj != null || !mainCamera.IsShakeFinish) return;

        GameObject bossDeadEffect = GameObject.FindGameObjectWithTag("DeadEffect");

        if (bossDeadEffect == null)
        {
            warpZone.StartAnimation();
        }
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
                ControllerShake.Shake(0.0f);
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

    /// <summary>
    /// コントローラー振動時間カウント
    /// </summary>
    private void C_Shake()
    {
        if (controllerShakeTime <= 0.0f)
        {
            ControllerShake.Shake(0.0f);
            return;
        }

        controllerShakeTime -= Time.unscaledDeltaTime;
    }

    /// <summary>
    /// ステージの範囲設定
    /// </summary>
    private void SetStageRange()
    {
        if (stageNum < 1&&!isDebug) return;

        GameObject under = GameObject.Find("Under");
        GameObject left = GameObject.Find("Left");
        GameObject top = GameObject.Find("Top");
        GameObject right = GameObject.Find("Right");

        Vector2 underleft = new Vector2(left.transform.position.x, under.transform.position.y);
        Vector2 topright = new Vector2(right.transform.position.x, top.transform.position.y);

        stageMinPos = new Vector2(underleft.x, underleft.y);
        stageMaxPos = new Vector2(topright.x, topright.y);
    }

    /// <summary>
    /// TimeScale変更
    /// </summary>
    public void SetTimeScale(float delay, float scale)
    {
        stopDelay = delay;
        Time.timeScale = scale;
    }

    /// <summary>
    /// コントローラー振動
    /// </summary>
    /// <param name="value"></param>
    /// <param name="time"></param>
    public void ShakeController(float value, float time)
    {
        ControllerShake.Shake(value);
        controllerShakeTime = time;
    }

    /// <summary>
    /// レイヤー同士ののあたり判定
    /// </summary>
    /// <param name="name"></param>
    /// <param name="otherName"></param>
    /// <param name="isCollision"></param>
    public void LayerCollision(string name, string otherName, bool isCollision)
    {
        int layer = LayerMask.NameToLayer(name);
        int otherLayer = LayerMask.NameToLayer(otherName);

        Physics2D.IgnoreLayerCollision(layer, otherLayer, isCollision);
    }

    /// <summary>
    /// ステージの左下
    /// </summary>
    public Vector2 StageMinPos
    {
        get { return stageMinPos; }
    }

    /// <summary>
    /// ステージの右上
    /// </summary>
    public Vector2 StageMaxPos
    {
        get { return stageMaxPos; }
    }

    /// <summary>
    /// デバッグフラグ
    /// </summary>
    public bool IsDebug
    {
        get { return isDebug; }
        set { isDebug = value; }
    }
}
