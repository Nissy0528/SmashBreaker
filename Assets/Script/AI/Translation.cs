using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : AI
{
    public float speed;//速度
    public bool isLoop;//移動を繰り返すか

    private Vector2[] positions;//移動する座標
    private Vector3 target;//目指す座標
    private int posNum;//目指す座標の番号
    private float length;//移動先までの距離
    private bool isEnd;//終了判定

    // Use this for initialization
    public override void Initialize()
    {
        if (positions.Length > 0)
        {
            target = positions[0];
        }
        posNum = 0;
        isEnd = false;
    }

    // Update is called once per frame
    protected override void AIUpdate()
    {
        Move();
        TargetChange();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        length = Vector2.Distance(transform.position, target);
        if (length <= 1.0f)
        {
            transform.position = target;
        }
        if (!isLoop && transform.position == (Vector3)positions[positions.Length - 1])
        {
            isEnd = true;
        }
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
        else if (isLoop)
        {
            posNum = 0;
        }

        target = positions[posNum];
    }

    /// <summary>
    /// 座標の配列
    /// </summary>
    public Vector2[] Positions
    {
        get { return positions; }
        set { positions = value; }
    }

    /// <summary>
    /// 移動速度
    /// </summary>
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    /// <summary>
    /// 終了判定
    /// </summary>
    public override bool IsActive
    {
        get { return isEnd; }
    }
}
