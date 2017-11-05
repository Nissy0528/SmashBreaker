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
    /// 飛ばす方向
    /// </summary>
    [SerializeField]
    private Vector3[] velocitys;

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

    public void Start()
    {
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        razerList = new List<Razer>();
        for (int i = 0; i < velocitys.Length; i++)
        {
            razerList.Add(new Razer(transform, velocitys[i], speed, shieldLayer, mat, targetLayers));
        }
        razerCount = 0.0f;
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
    }

    /// <summary>
    /// ビーム切り替え
    /// </summary>
    private void Swich()
    {
        if(razerCount>0.0f)
        {
            razerCount -= Time.deltaTime;
            return;
        }

        isEnable = !isEnable;
        Stop();
        razerCount = razerTime;
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