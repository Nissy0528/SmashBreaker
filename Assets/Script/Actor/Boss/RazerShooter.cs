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
    /// 速度
    /// </summary>
    [SerializeField]
    private float speed = 10f;

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

    public Material mat;
    public float razerTime;

    private float razerCount;
    private bool isEnable;
    private List<GameObject> muzzle = new List<GameObject>();

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
            razerList.Add(new Razer(muzzle[i].transform.Find("FirePos"), muzzle[i].transform.up, speed, shieldLayer, mat, targetLayers));
        }
        razerCount = razerTime;
        isEnable = true;
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
            //AddDamage(rCol.gameObject);
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