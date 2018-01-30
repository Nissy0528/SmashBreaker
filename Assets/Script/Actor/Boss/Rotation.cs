using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotation : AI
{
    /// <summary>
    /// 回転のモード
    /// </summary>
    public enum Mode
    {
        ROUND,//一回転
        PENDULUM,//振り子
        CHANGE,//切り替え
        TARGET,//軸回転
    }

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    private float rotSpeed = 30f;

    public Mode mode;//回転のモード
    public bool rivers;//反転
    public float rightAngle;//右の最大角度
    public float leftAngle;//左の最大角度
    public float changeTime;//角度切り替え時間
    public string targetTag;//回転軸にするオブジェクトのタグ

    private Vector2 targetPos;//回転軸の座標
    private Transform target;
    private float changeCount;//回転切り替えカウント
    private float iniAngle;//初期角度

    // Use this for initialization
    public override void Initialize()
    {
        iniAngle = transform.eulerAngles.z;

        changeCount = changeTime;
        //if (GetComponent<Collider2D>() != null && tag != "Boss")
        //{
        //    GetComponent<Collider2D>().enabled = false;
        //}
    }

    // Update is called once per frame
    protected override void AIUpdate()
    {
        Move();
        AngleChange();
        TargetRotate();
    }

    /// <summary>
    /// 一回転
    /// </summary>
    private void Move()
    {
        if (mode == Mode.CHANGE) return;

        Vector3 rotation = Vector3.forward * rotSpeed * Time.deltaTime;
        var riv = rivers ? -1f : 1f;
        transform.Rotate(rotation * riv);
        MovePendulum();
    }
    /// <summary>
    /// 振り子
    /// </summary>
    private void MovePendulum()
    {
        if (mode != Mode.PENDULUM) return;

        float angleZ = transform.eulerAngles.z;
        float rotateZ = 0.0f;
        //ラジアン単位に変換
        if (angleZ > 180)
        {
            rotateZ = angleZ - 360;
            //右の最大角度まで行ったら時計周り
            if (rotateZ > rightAngle)
            {
                rivers = true;
            }

        }
        else
        {
            rotateZ = angleZ;
            //左の最大角度まで行ったら反時計回り
            if (rotateZ < leftAngle)
            {
                rivers = false;
            }
        }
    }

    /// <summary>
    /// 角度切り替え
    /// </summary>
    private void AngleChange()
    {
        if (mode != Mode.CHANGE) return;

        changeCount -= Time.deltaTime;

        if (changeCount < 0.0f)
        {
            Change();
            changeCount = changeTime;
        }
    }
    /// <summary>
    /// 切り替え
    /// </summary>
    private void Change()
    {
        Vector3 angle = transform.eulerAngles;
        Vector3 rotate = Vector3.zero;
        if (angle.z > 180)
        {
            rotate.z = angle.z - 360;
        }
        else
        {
            rotate.z = angle.z;
        }

        if (rotate.z == iniAngle)
        {
            IniToLR(rotate);
            return;
        }
        else
        {
            LRToIni(rotate);
        }
    }
    /// <summary>
    /// 初期角度から右か左の角度に
    /// </summary>
    /// <param name="rotate"></param>
    private void IniToLR(Vector3 rotate)
    {
        if (!rivers)
        {
            rotate.z = rightAngle;
        }
        else
        {
            rotate.z = leftAngle;
        }

        transform.eulerAngles = rotate;
    }
    /// <summary>
    /// 右か左から初期角度に
    /// </summary>
    /// <param name="rotate"></param>
    private void LRToIni(Vector3 rotate)
    {
        if (rotate.z == rightAngle)
        {
            rivers = true;
        }
        if (rotate.z == leftAngle)
        {
            rivers = false;
        }
        rotate.z = iniAngle;
        transform.eulerAngles = rotate;
    }

    /// <summary>
    /// 軸回転
    /// </summary>
    private void TargetRotate()
    {
        if (mode != Mode.TARGET) return;

        Transform target = GameObject.FindGameObjectWithTag(targetTag).transform;
        targetPos = target.position;
        transform.parent = target;

        Vector3 axis = transform.TransformDirection(Vector3.forward);
        transform.RotateAround(targetPos, axis, rotSpeed * Time.deltaTime);

        Vector3 rotate = transform.eulerAngles;
        rotate.z = 0.0f;
        transform.eulerAngles = rotate;
    }
}
