using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDown : MonoBehaviour
{
    public Image[] sprites;
    public GameObject boss;//ボス画像オブジェクト
    public GameObject smash;//拳画像オブジェクト
    public Sprite[] bossImage;//切り替えるボス画像の配列
    public float changeTime;//ボスと拳の画像を切り替える時間（設定用）
    public float scaleSpeed;//拳の画像が大きくなる速度

    private float changeCnt;//ボスと拳の画像を切り替える時間
    private int bossImageNum;//表示するボス画像の配列番号
    private bool isDead;//消滅フラグ

    // Use this for initialization
    void Start()
    {
        changeCnt = changeTime;
        bossImageNum = 0;
        boss.SetActive(false);
        smash.SetActive(true);
        isDead = false;
        for (int i = 0; i < 3; i++)
        {
            sprites[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Change();
        SmashScale();
        Dead();
    }

    /// <summary>
    /// 画像切り替え
    /// </summary>
    private void Change()
    {
        if (changeCnt > 0.0f)
        {
            changeCnt -= Time.unscaledDeltaTime;
            return;
        }

        BossImage();

        smash.SetActive(!smash.activeSelf);

        if (!isDead)
        {
            changeCnt = changeTime;
        }
    }

    /// <summary>
    /// ボス画像更新
    /// </summary>
    private void BossImage()
    {
        boss.SetActive(!boss.activeSelf);

        if (bossImageNum >= bossImage.Length)
        {
            isDead = true;
            return;
        }

        boss.GetComponent<Image>().sprite = bossImage[bossImageNum];

        if (boss.activeSelf)
        {
            bossImageNum += 1;
        }
    }

    /// <summary>
    /// 拳の画像を大きくする
    /// </summary>
    private void SmashScale()
    {
        if (!smash.activeSelf) return;

        RectTransform rect = smash.GetComponent<RectTransform>();
        rect.sizeDelta += new Vector2(scaleSpeed, scaleSpeed);
    }

    /// <summary>
    /// 消滅
    /// </summary>
    private void Dead()
    {
        if (!isDead || changeCnt > 0.0f) return;

        Destroy(gameObject);
    }
}
