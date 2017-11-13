using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotation : MonoBehaviour
{
    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField]
    private int speed = 2;

    /// <summary>
    ///耐力
    /// </summary>
    [SerializeField]
    private float bytal = 10;

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    private float rotSpeed = 30f;

    /// <summary>
    /// 反転
    /// </summary>
    public bool rivers;

    private List<GameObject> muzzle = new List<GameObject>();
    private Enemy enemyClass;//エネミークラス

    // Use this for initialization
    void Start()
    {
        enemyClass = GetComponent<Enemy>();
        GameObject[] muzzles = GameObject.FindGameObjectsWithTag("Muzzle");
        for (int i = 0; i < muzzles.Length; i++)
        {
            if (muzzles[i].transform.parent == transform)
            {
                muzzle.Add(muzzles[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (bytal <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (enemyClass.IsStan) return;

        Vector3 rotation = Vector3.forward * rotSpeed * Time.deltaTime;
        var riv = rivers ? -1f : 1f;

        for (int i = 0; i < muzzle.Count; i++)
        {
            muzzle[i].transform.Rotate(rotation * riv);
            muzzle[i].GetComponent<Rigidbody2D>().MovePosition(muzzle[i].transform.position + ((muzzle[i].transform.rotation * Vector3.forward) * speed));
        }
    }
}
