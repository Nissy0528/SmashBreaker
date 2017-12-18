using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public Material[] mat;//マテリアルの配列

    private GameObject player;//プレイヤー
    private GameObject child;//プレイヤーと比較する座標
    private SpriteRenderer sprite;//スプライト

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");//プレイヤー取得
        child = transform.GetChild(0).gameObject;//比較する座標取得
        sprite = GetComponent<SpriteRenderer>();//スプライト取得
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが上にいたら、プレイヤーが後ろにいるように描画
        if (child.transform.position.y < player.transform.position.y)
        {
            sprite.sortingLayerName = "Middle";//プレイヤーより前に描画
            sprite.material = mat[0];//ステンシルバッファ用マテリアルに
        }
        //プレイヤーが下にいたら、プレイヤーが前にいるように描画
        else
        {
            sprite.sortingLayerName = "Default";//プレイヤーより後ろに描画
            sprite.material = mat[1];//通常マテリアルに
        }
    }
}
