﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザーの管理
/// </summary>
public class RazerShooter : MonoBehaviour
{
    /// <summary>
    /// 爆破
    /// </summary>
    [SerializeField]
    private GameObject particle;

    /// <summary>
    /// 標的のレイヤー
    /// </summary>
    [SerializeField]
    private string[] targetLayers;
    /// <summary>
    /// 遮蔽物のレイヤー
    /// </summary>
    [SerializeField]
    private string shieldLayer;

    public Material mat;//レーザーの色

    public float razerOnTime;//レーザーオン時間
    public float razerOffTime;//レーザーオフ時間
    public float time;//レーザー発射時間
    private List<Razer> razerList;//レーザーのリスト
    private Razer warpRazer;//ワープしたレーザーのリスト
    private List<GameObject> muzzle;//発射口のリスト
    private Boss boss_class;//ボスクラス
    private GameObject boss_muzzle;//レーザー口
    private GameObject warpRazerObj;
    private MainCamera mainCamera;
    private float razerCount;
    private bool isEnable;//レーザーのアクティブフラグ

    public void Start()
    {
        GameObject[] muzzles = GameObject.FindGameObjectsWithTag("Muzzle");
        muzzle = new List<GameObject>();
        for (int i = 0; i < muzzles.Length; i++)
        {
            if (muzzles[i].transform.parent == transform)
            {
                muzzle.Add(muzzles[i]);
            }
        }

        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();

        boss_class = GetComponent<Boss>();
        boss_muzzle = transform.Find("Muzzle").Find("Boss2_Muzzle").gameObject;
        boss_muzzle.SetActive(false);

        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        razerList = new List<Razer>();
        for (int i = 0; i < muzzle.Count; i++)
        {
            razerList.Add(new Razer(muzzle[i].transform.Find("FirePos"), muzzle[i].transform.up, shieldLayer, mat, time, targetLayers));
        }
        razerCount = 0.0f;
        isEnable = false;
        Stop();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        foreach (var r in razerList)
        {
            r.Update();
            HitCheck(r);
        }
        if (warpRazer != null)
        {
            warpRazer.Update();
            HitCheck(warpRazer);
        }
        Swich();
        //MuzzleColor();
    }

    /// <summary>
    /// ビーム切り替え
    /// </summary>
    private void Swich()
    {
        if (!ShootCount()) return;

        if (isEnable)
        {
            mainCamera.SetShake(false, razerOffTime);
        }

        if (razerCount < razerOnTime && !isEnable)
        {
            razerCount += Time.deltaTime;
            return;
        }
        if (razerCount < razerOffTime && isEnable)
        {
            razerCount += Time.deltaTime;
            return;
        }

        boss_class.AnimBool("Razer", !isEnable);

        if (isEnable)
        {
            warpRazer = null;
            Destroy(warpRazerObj);
        }

        if (boss_class.AnimFinish("Boss_Razer"))
        {
            isEnable = !isEnable;
            Stop();
            razerCount = 0.0f;
            boss_muzzle.SetActive(isEnable);
        }
    }

    /// <summary>
    /// 発射口の透明度を徐々に濃くする
    /// </summary>
    private void MuzzleColor()
    {
        if (isEnable) return;

        for (int i = 0; i < muzzle.Count; i++)
        {
            GameObject fireObj = muzzle[i].transform.Find("Fire").gameObject;
            Color fireColor = fireObj.GetComponent<SpriteRenderer>().color;
            fireColor.a = razerCount / razerOnTime;
            fireObj.GetComponent<SpriteRenderer>().color = fireColor;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="r"></param>
    private void HitCheck(Razer r)
    {
        var rCol = r.GetHit();

        if (rCol != null)
        {
            AddDamage(rCol.gameObject);//プレイヤーにダメージ
            WarpRazer(rCol.gameObject, r);//ワープしたレーザー生成
        }
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="col"></param>
    private void AddDamage(GameObject col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<Player>().Damage();
        }
    }

    /// <summary>
    /// ワープしたレーザー生成
    /// </summary>
    /// <param name="col"></param>
    private void WarpRazer(GameObject col, Razer razer)
    {
        if (col.gameObject.tag == "Warp" && warpRazer == null)
        {
            Warp warp = col.GetComponent<Warp>();
            string[] target = new string[] { "Player", "Weak" };
            if (warp.GetWarpPosObj != null)
            {
                warpRazer = new Razer(warp.GetWarpPosObj.transform, razer.Direction, shieldLayer, mat, 0.0f, target);
                warpRazerObj = warpRazer.GetOrigin;
            }
        }
    }

    /// <summary>
    /// レーザー切り替えカウント判定
    /// </summary>
    /// <returns></returns>
    private bool ShootCount()
    {
        foreach (var r in razerList)
        {
            if (!r.IsRazer)
                return false;
            else
                return true;
        }
        return false;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        foreach (var r in razerList)
        {
            r.Stop(isEnable);
        }
    }

    /// <summary>
    /// 各時間設定
    /// </summary>
    /// <param name="razerTime">予兆時間</param>
    /// <param name="time">発射時間</param>
    public void SetSpeed(float razerTime, float time)
    {
        this.razerOnTime = razerTime;
        this.time = time;
    }

    /// <summary>
    /// レーザーフラグ
    /// </summary>
    public bool GetEnable
    {
        get { return isEnable; }
    }
}