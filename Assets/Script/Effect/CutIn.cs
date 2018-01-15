using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutIn : MonoBehaviour
{
    public Image[] sprites;
    public float time;
    public float speed;

    private GameObject bossDeadUI;
    private RectTransform rect;
    private float cnt;
    private bool isDead;

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<GameManager>().SetTimeScale(10, 0.0f);
        ControllerShake.Shake(0.0f);
        rect = GetComponent<RectTransform>();
        isDead = false;
        cnt = 0.0f;
        for (int i = 0; i < 2; i++)
        {
            sprites[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        TimeCount();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (cnt > 0.0f) return;

        Vector2 pos = rect.anchoredPosition;
        pos.x += speed;
        rect.anchoredPosition = pos;

        if (pos.x >= Screen.width / 2 && !isDead)
        {
            cnt = time;
            isDead = true;
        }

        if (isDead && pos.x >= (Screen.width + Screen.width / 2))
        {
            bossDeadUI.GetComponent<BossDown>().enabled = true;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 待機時間カウント
    /// </summary>
    private void TimeCount()
    {
        if (cnt < 0.0f) return;

        cnt -= Time.unscaledDeltaTime;
    }

    /// <summary>
    /// ボス死亡時の演出設定
    /// </summary>
    /// <param name="obj"></param>
    public void SetBossDeadUI(GameObject obj)
    {
        bossDeadUI = obj;
    }
}
