using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBulletShooter : MonoBehaviour
{
    public GameObject bullet;//弾オブジェクト
    public int bulletCount;//弾の数
    public float radius;//生成する円状の広さ
    public float shootTime;//発射時間（設定用）

    private List<GameObject> bulletList = new List<GameObject>();//弾のリスト
    private float shootCount;//発射時間

    void Start()
    {
        shootCount = shootTime;
    }


    void Update()
    {
        Shoot();//弾を発射

        bulletList.RemoveAll(b => b == null);
    }

    /// <summary>
    /// 弾を円状に配置
    /// </summary>
    private void CreateBullets()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            bulletList.Add(Instantiate(bullet));
        }

        float angleDiff = 360f / (float)bulletList.Count;//オブジェクト間の角度差

        //円状に配置
        for (int i = 0; i < bulletList.Count; i++)
        {
            Vector2 bulletPos = transform.position;

            float angle = (90 - angleDiff * i) * Mathf.Deg2Rad;
            bulletPos.x += radius * Mathf.Cos(angle);
            bulletPos.y += radius * Mathf.Sin(angle);

            if (bulletList[i] == null) continue;

            bulletList[i].transform.Rotate(0, 0, -(angleDiff * i));
            bulletList[i].transform.position = bulletPos;
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
}
