using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBulletShooter : AI
{
    public GameObject bullet;//弾オブジェクト
    public int bulletCount;//弾の数
    public float radius;//生成する円状の広さ
    public float shootTime;//発射時間（設定用）
    public bool setRotate;//角度設定フラグ

    private List<GameObject> bulletList = new List<GameObject>();//弾のリスト
    private float shootCount;//発射時間
    private bool isCreate;//弾生成完了フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize()
    {
        ResetBullet();
        shootCount = shootTime;
        isCreate = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void AIUpdate()
    {
        Shoot();//弾を発射

        bulletList.RemoveAll(b => b == null);
    }

    /// <summary>
    /// 弾を円状に配置
    /// </summary>
    public void CreateBullets()
    {
        bulletList = new List<GameObject>();

        for (int i = 0; i < bulletCount; i++)
        {
            bulletList.Add(Instantiate(bullet));
        }

        float angleDiff = 360f / (float)bulletList.Count;//オブジェクト間の角度差

        //円状に配置
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] == null) continue;

            SetTransfrom(i, angleDiff);
        }

        isCreate = true;
    }

    /// <summary>
    /// 角度設定
    /// </summary>
    private void SetTransfrom(int i, float angleDiff)
    {
        float angle = (90 - angleDiff * i) * Mathf.Deg2Rad;
        Vector2 bulletPos = transform.position;

        bulletPos.x += radius * Mathf.Cos(angle);
        bulletPos.y += radius * Mathf.Sin(angle);

        bulletList[i].transform.position = bulletPos;

        if (setRotate)
        {
            bulletList[i].transform.Rotate(0, 0, -(angleDiff * i));
        }
    }

    /// <summary>
    /// 発射時間が経過したら円状に弾を発射
    /// </summary>
    private void Shoot()
    {
        if (shootCount > 0.0f)
        {
            shootCount -= Time.deltaTime;
            return;
        }

        CreateBullets();
        shootCount = shootTime;
    }

    /// <summary>
    /// 弾リストクリア
    /// </summary>
    private void ResetBullet()
    {
        foreach (var b in bulletList)
        {
            Destroy(b);
        }
        bulletList.Clear();
    }

    /// <summary>
    /// 完了フラグ
    /// </summary>
    public override bool IsActive
    {
        get
        {
            return isCreate;
        }
    }
}
