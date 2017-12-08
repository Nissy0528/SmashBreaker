using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bullet;
    public float shootTime;

    private float shootCount;

    // Use this for initialization
    void Start()
    {
        shootCount = shootTime;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    /// <summary>
    /// 弾発射
    /// </summary>
    private void Shoot()
    {
        if (shootCount > 0.0f)
        {
            shootCount -= Time.deltaTime;
            return;
        }

        GameObject bulletObj = Instantiate(bullet);
        bulletObj.transform.position = transform.position;
        Quaternion rotate = transform.rotation;
        rotate.z += 180f;
        bulletObj.transform.rotation = rotate;
        shootCount = shootTime;
    }
}
