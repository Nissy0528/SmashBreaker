using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float stopTime;//ゲームストップの長さ
    public GameObject gameover;

    private float stopDelay;
    private Player player;

    // Use this for initialization
    void Start()
    {
        stopDelay = stopTime;
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        GameStart();
        ShowGameOver();
    }

    /// <summary>
    /// ゲームストップ
    /// </summary>
    public static void GameStop()
    {
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// ゲーム再開
    /// </summary>
    private void GameStart()
    {
        if (Time.timeScale >= 1.0f) return;

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
        if (Input.GetButtonDown("Decision"))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
