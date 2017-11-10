using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : MonoBehaviour
{
    public Vector2[] positions;//移動する座標
    public float speed;//速度

    private Vector3 target;//目指す座標
    private int posNum;//目指す座標の番号
    private Enemy enemyClass;

    // Use this for initialization
    void Start()
    {
        if (positions.Length > 0)
        {
            target = positions[0];
        }
        posNum = 0;
        enemyClass = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyClass.IsStan)
        {
            Move();
            TargetChange();
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    /// <summary>
    /// 向かう座標変更
    /// </summary>
    private void TargetChange()
    {
        if (transform.position != target) return;

        if (posNum < positions.Length - 1)
        {
            posNum++;
        }
        else
        {
            posNum = 0;
        }

        target = positions[posNum];
    }
}
