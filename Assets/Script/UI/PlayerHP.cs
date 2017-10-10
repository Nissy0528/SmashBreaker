using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public GameObject[] hpTextures;

    private Player player;//プレイヤー
    private int playerHP;//プレイヤーの体力

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
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
        playerHP = player.hp;

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
}
