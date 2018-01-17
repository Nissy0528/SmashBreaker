using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer
{
    public Ray2D shotRay;

    /// <summary>
    /// 原点の空オブジェクト
    /// </summary>
    private GameObject origin;
    private GameObject effect;//エフェクトオブジェクト

    /// <summary>
    /// 線の描画
    /// </summary>
    private LineRenderer lineRenderer;
    private RaycastHit2D shotHit;
    private int layerMask;

    private float length;
    private float time;
    private float count;

    /// <summary>
    /// 遮蔽物レイヤー
    /// </summary>
    private string wallLayer;

    public Razer(Transform shooter, Vector3 velocity, string wallLayer, Material mat, float time, params string[] targetLayers)
    {
        //空オブジェクトの生成
        origin = new GameObject("shooter");
        origin.tag = "Razer";
        var transform = origin.transform;
        transform.SetParent(shooter);
        transform.localPosition = Vector3.zero;

        this.time = time;

        ///線の作成
        lineRenderer = origin.AddComponent<LineRenderer>();
        //当たり判定付けたい分だけ追加
        layerMask = LayerMask.GetMask(targetLayers);
        //ビームの表示
        lineRenderer.enabled = true;
        //原点の設定
        lineRenderer.SetPosition(0, transform.position);
        //レイの設定
        shotRay.origin = transform.position;
        shotRay.direction = velocity.normalized;
        //マテリアル設定
        lineRenderer.material = mat;

        this.wallLayer = wallLayer;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        LineUpdate();

    }

    private void LineUpdate()
    {
        WallCheck();

        length = WallCheck();
        shotRay.origin = origin.transform.position;
        Vector3 kz = shotRay.origin + (Vector2)(origin.transform.rotation * shotRay.direction * length);


        lineRenderer.SetPosition(0, origin.transform.position);
        lineRenderer.SetPosition(1, kz);

        RazerPrepare();
    }

    /// <summary>
    /// 壁判定
    /// </summary>
    /// <returns></returns>
    private float WallCheck()
    {
        Vector2 rotdir = (origin.transform.rotation * shotRay.direction);
        const float range = 150f;
        Vector3 kz = shotRay.origin + rotdir * range;
        //当たったobj取得
        float st = Vector2.Distance(origin.transform.position, kz);
        shotHit = Physics2D.Raycast(origin.transform.position, rotdir, st, LayerMask.GetMask(wallLayer));
        if (shotHit && shotHit.transform.parent != origin.transform.parent)
        {
            return Vector2.Distance(origin.transform.position, shotHit.point);
        }
        return range;
    }

    /// <summary>
    /// レーザー発射準備
    /// </summary>
    private void RazerPrepare()
    {
        if (lineRenderer.enabled == false) return;

        count += Time.deltaTime;

        if (count >= time)
        {
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            Color color = lineRenderer.material.color;
            color.a = 1.0f;
            lineRenderer.material.color = color;
            GameObject.Destroy(effect);
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <returns></returns>
    public Collider2D GetHit()
    {
        if (count < time || lineRenderer.enabled == false) return null;

        Vector2 rotdir = (origin.transform.rotation * shotRay.direction);
        Vector3 kz = lineRenderer.GetPosition(1);
        //当たったobj取得
        float st = Vector2.Distance(origin.transform.position, kz);
        shotHit = Physics2D.Raycast(origin.transform.position, rotdir, st, layerMask);

        return shotHit.collider;

    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop(bool isEnabled)
    {
        //幅設定
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.enabled = isEnabled;
        count = 0.0f;
        //マテリアル設定
        Color color = lineRenderer.material.color;
        if (!isEnabled)
        {
            color.a = 0.0f;
        }
        else
        {
            CreateEffect();
            color.a = 0.25f;
        }
        lineRenderer.material.color = color;
    }

    /// <summary>
    /// エフェクト生成
    /// </summary>
    private void CreateEffect()
    {
        effect = GameObject.Instantiate(Resources.Load("Prefab/Effect/RazerEffect")) as GameObject;
        var e_transform = effect.transform;
        e_transform.SetParent(origin.transform.parent);
        e_transform.localPosition = Vector3.zero;
        e_transform.localRotation = Quaternion.identity;
        e_transform.GetComponent<RazerEffect>().SetRazer = this;
    }

    /// <summary>
    /// レーザーインターバル判定
    /// </summary>
    public bool IsRazer
    {
        get { return count >= time || lineRenderer.enabled == false; }
    }

    /// <summary>
    /// 長さ取得
    /// </summary>
    public float Length
    {
        get { return length; }
    }

    /// <summary>
    /// 発射方向
    /// </summary>
    /// <returns></returns>
    public Vector2 Direction
    {
        get { return origin.transform.parent.up; }
    }


    public GameObject GetOrigin
    {
        get { return origin; }
    }

    /// <summary>
    /// レーザー発射フラグ
    /// </summary>
    /// <returns></returns>
    public bool IsFire()
    {
       return count >= time;
    }
}

