using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer
{
    private const float limit = 200f;

    /// <summary>
    /// 原点の空オブジェクト
    /// </summary>
    private GameObject origin;

    /// <summary>
    /// 距離
    /// </summary>
    [SerializeField]
    private float distance = 0.0f;
    public Ray2D shotRay;

    /// <summary>
    /// 線の描画
    /// </summary>
    private LineRenderer lineRenderer;
    private RaycastHit2D shotHit;
    private int layerMask;

    private float time;
    private float count;

    /// <summary>
    /// 遮蔽物レイヤー
    /// </summary>
    private string wallLayer;

    public Razer(Transform shooter, Vector3 velocity, string wallLayer, Material mat, params string[] targetLayers)
	{
        //空オブジェクトの生成
        origin = new GameObject("shooter");
        var transform = origin.transform;
        transform.SetParent(shooter);
        transform.localPosition = Vector3.zero;

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
        //幅設定
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

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
        //長さの加算
        distance = limit;


		WallCheck();

        float length = WallCheck();
        shotRay.origin = origin.transform.position;
        Vector3 kz = shotRay.origin + (Vector2)(origin.transform.rotation * shotRay.direction * length);


        lineRenderer.SetPosition(0, origin.transform.position);
        lineRenderer.SetPosition(1, kz);

        //RazerPrepare();
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
        if (shotHit)
        {
            return Vector2.Distance(origin.transform.position, shotHit.transform.position);
        }

		return range;
    }

    /// <summary>
    /// レーザー発射準備
    /// </summary>
    private void RazerPrepare()
    {
        count += Time.deltaTime;

        lineRenderer.startWidth = Mathf.Min(lineRenderer.startWidth + Time.deltaTime, 0.25f);
        lineRenderer.endWidth = Mathf.Min(lineRenderer.endWidth + Time.deltaTime, 0.25f);

        if (count >= time)
        {
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            Color color = lineRenderer.material.color;
            color.a = 1.0f;
            lineRenderer.material.color = color;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <returns></returns>
    public Collider2D GetHit()
    {
        if (lineRenderer.enabled == false) return null;

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
        lineRenderer.enabled = isEnabled;
        distance = 0;
        ////マテリアル設定
        //Color color = lineRenderer.material.color;
        //color.a = 0.0f;
        //lineRenderer.material.color = color;
        ////幅設定
        //lineRenderer.startWidth = 0.0f;
        //lineRenderer.endWidth = 0.0f;
        //time = 0.0f;
    }
}

