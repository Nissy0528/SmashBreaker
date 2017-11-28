using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public GameObject texture;

    private GameObject[] hpTextures;
    private Player player;//プレイヤー
    private int playerHP;//プレイヤーの体力

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Chara").GetComponent<Player>();
        playerHP = player.GetParam.hp;
        hpTextures = new GameObject[player.GetParam.maxHP];
        CreateTextures();
    }

    /// <summary>
    /// HP画像配置
    /// </summary>
    private void CreateTextures()
    {
        for (int i = 0; i < hpTextures.Length; i++)
        {
            hpTextures[i] = Instantiate(texture, transform);
            RectTransform rectTransform = hpTextures[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-100 * i, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHP();
    }

    /// <summary>
    /// プレイヤーの体力表示
    /// </summary>
    private void ChangeHP()
    {
        //プレイヤーの体力に合わせて表示する画像の個数を変更
        playerHP = player.GetParam.hp;

        for (int i = 0; i < hpTextures.Length; i++)
        {
            if (i < playerHP)
            {
                hpTextures[i].SetActive(true);
            }
            else
            {
                hpTextures[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 体力が最大か
    /// </summary>
    public bool IsHPMax
    {
        get { return playerHP >= hpTextures.Length; }
    }

    /// <summary>
    /// 体力取得
    /// </summary>
    public float HP_Dif
    {
        get { return (float)hpTextures.Length / (float)playerHP; }
    }
}
