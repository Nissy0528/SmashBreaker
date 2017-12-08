using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public float rotateSpeed;//回転速度

    private GameObject[] warpObjects;//ゲーム上のワープゾーン
    private GameObject warpPosObj;//転送先のオブジェクト
    private GameObject sprite;//画像オブジェクト
    private Vector3 warpPos;//転送先の座標
    private bool isWarp;//ワープフラグ
    private int num;//認識番号

    // Use this for initialization
    void Start()
    {
        warpObjects = GameObject.FindGameObjectsWithTag("Warp");//ゲーム上のワープゾーン検索
        if (transform.childCount > 0)
        {
            sprite = transform.Find("WarpSprite").gameObject;
        }
        num = warpObjects.Length - 1;
        //ワープゾーンが3つ以上なら古いワープゾーン削除
        if (warpObjects.Length >= 3)
        {
            SetNum(warpObjects[0]);
        }
        isWarp = false;
    }

    /// <summary>
    /// 認識番号設定
    /// </summary>
    private void SetNum(GameObject oldWarp)
    {
        num = oldWarp.GetComponent<Warp>().GetNum;
        Destroy(oldWarp);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        SetWarpPos();
    }

    /// <summary>
    /// 転送先の座標設定
    /// </summary>
    private void SetWarpPos()
    {
        warpObjects = GameObject.FindGameObjectsWithTag("Warp");//ゲーム上のワープゾーン検索
        warpPos = Vector3.zero;

        if (warpObjects.Length < 2) return;

        foreach (var w in warpObjects)
        {
            if (w.GetComponent<Warp>().GetNum != num)
            {
                warpPos = w.transform.position;
                warpPosObj = w;
            }
        }
    }

    /// <summary>
    /// 転送
    /// </summary>
    private void Transport(GameObject player)
    {
        if (warpPos == Vector3.zero) return;

        player.transform.position = warpPos;

        AllWarpDestroy();
    }

    /// <summary>
    /// 全てのワープゾーン削除
    /// </summary>
    private void AllWarpDestroy()
    {
        foreach (var w in warpObjects)
        {
            Destroy(w);
        }
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Rotate()
    {
        if (sprite == null) return;

        sprite.transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }

    /// <summary>
    /// あたり判定（トリガー）
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && isWarp)
        {
            Transport(col.gameObject);
        }
    }

    /// <summary>
    /// 離れた判定（トリガー）
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && !isWarp)
        {
            isWarp = true;
        }
    }

    /// <summary>
    /// 認識番号取得
    /// </summary>
    public int GetNum
    {
        get { return num; }
    }

    /// <summary>
    /// 転送先のワープ
    /// </summary>
    public GameObject GetWarpPosObj
    {
        get { return warpPosObj; }
    }
}
