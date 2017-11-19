using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザーの管理
/// </summary>
public class RazerShooter : MonoBehaviour
{
    /// <summary>
    /// 停止フラグ
    /// </summary>
    private bool isStop;

    /// <summary>
    /// 爆破
    /// </summary>
    [SerializeField]
    private GameObject particle;

    /// <summary>
    /// レーザー
    /// </summary>
    private List<Razer> razerList;

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
    public float razerTime = 2.5f;//レーザー切り替え時間
    public float time = 2.5f;//レーザー発射時間

    private float razerCount;
    private bool isEnable;//レーザーのアクティブフラグ
    private List<GameObject> muzzle = new List<GameObject>();//発射口のリスト

    public void Start()
    {
        GameObject[] muzzles = GameObject.FindGameObjectsWithTag("Muzzle");
        for (int i = 0; i < muzzles.Length; i++)
        {
            if (muzzles[i].transform.parent == transform)
            {
                muzzle.Add(muzzles[i]);
            }
        }
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
        Swich();
        MuzzleColor();
    }

    /// <summary>
    /// ビーム切り替え
    /// </summary>
    private void Swich()
    {
        if (!ShootCount()) return;

        if (razerCount < razerTime)
        {
            razerCount += Time.deltaTime;
            return;
        }

        isEnable = !isEnable;
        Stop();
        razerCount = 0.0f;
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
            fireColor.a = razerCount / razerTime;
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
            var posi = rCol.transform.position;
            //与ダメージ処理
            AddDamage(rCol.gameObject);
            //撃破パーティクル
            //var p = Instantiate(particle);
            //p.transform.position = posi;
            //Destroy(p, 5f);
        }
    }

    private void AddDamage(GameObject col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<Player>().Damage();
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
        isStop = true;
        foreach (var r in razerList)
        {
            r.Stop(isEnable);
        }
    }
}