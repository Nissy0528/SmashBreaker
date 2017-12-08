using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashGage : MonoBehaviour
{
    public Image backGround;

    private float sp;//スマッシュポイント
    private float max;//最大スマッシュポイント
    private bool isMax;//ワンパンフラグ
    private Slider slider;
    private Player player;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        player = GameObject.Find("Chara").GetComponent<Player>();
        //max = player.GetParam.maxSP;
        isMax = false;
    }

    // Update is called once per frame
    void Update()
    {
        //SmashPoint();
        GageColor();
    }

    /// <summary>
    /// プレイヤーのスマッシュポイントを表示
    /// </summary>
    //private void SmashPoint()
    //{
    //    sp = player.GetParam.sp;
    //    slider.value = sp / max;

    //    if (sp >= player.GetParam.maxSP)
    //    {
    //        isMax = true;
    //    }

    //    if (isMax && sp <= 0.0f)
    //    {
    //        isMax = false;
    //    }
    //}

    /// <summary>
    /// ゲージの色変更
    /// </summary>
    private void GageColor()
    {
        if (isMax)
        {
            backGround.color = Color.red;
        }
        else
        {
            backGround.color = Color.blue;
        }
    }

    /// <summary>
    /// ワンパンフラグ取得
    /// </summary>
    public bool IsMax
    {
        get { return isMax; }
        set { isMax = value; }
    }
}
