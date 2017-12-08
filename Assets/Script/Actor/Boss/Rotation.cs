using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotation : MonoBehaviour
{
    /// <summary>
    /// 回転のモード
    /// </summary>
    public enum Mode
    {
        ROUND,//一回転
        PENDULUM,//振り子
        CHANGE//切り替え
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

    private List<GameObject> muzzle = new List<GameObject>();
    private Enemy enemyClass;//エネミークラス
    private float changeCount;
    private float[] iniAngles;//初期角度

    // Use this for initialization
    void Start()
    {
        enemyClass = GetComponent<Enemy>();
        GameObject[] muzzles = GameObject.FindGameObjectsWithTag("Muzzle");
        iniAngles = new float[muzzles.Length];
        for (int i = 0; i < muzzles.Length; i++)
        {
            if (muzzles[i].transform.parent == transform)
            {
                muzzle.Add(muzzles[i]);
            }
            iniAngles[i] = muzzles[i].transform.eulerAngles.z;
        }

        changeCount = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyClass.IsStan) return;

        Move();
        AngleChange();
    }

    /// <summary>
    /// 一回転
    /// </summary>
    private void Move()
    {
        if (mode == Mode.CHANGE) return;

        Vector3 rotation = Vector3.forward * rotSpeed * Time.deltaTime;
        var riv = rivers ? -1f : 1f;

        for (int i = 0; i < muzzle.Count; i++)
        {
            muzzle[i].transform.Rotate(rotation * riv);
            MovePendulum(i);
        }
    }
    /// <summary>
    /// 振り子
    /// </summary>
    private void MovePendulum(int i)
    {
        if (mode != Mode.PENDULUM) return;

        float angleZ = muzzle[i].transform.eulerAngles.z;
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
        for (int i = 0; i < muzzle.Count; i++)
        {
            SetAngle(i);
        }
    }
    /// <summary>
    /// 角度設定
    /// </summary>
    /// <param name="i"></param>
    private void SetAngle(int i)
    {
        Vector3 angle = muzzle[i].transform.eulerAngles;
        Vector3 rotate = Vector3.zero;
        if (angle.z > 180)
        {
            rotate.z = angle.z - 360;
        }
        else
        {
            rotate.z = angle.z;
        }

        if (rotate.z == iniAngles[i])
        {
            IniToLR(rotate, i);
            return;
        }
        else
        {
            LRToIni(rotate, i);
        }
    }
    /// <summary>
    /// 初期角度から右か左の角度に
    /// </summary>
    /// <param name="rotate"></param>
    private void IniToLR(Vector3 rotate, int i)
    {
        if (!rivers)
        {
            rotate.z = rightAngle;
        }
        else
        {
            rotate.z = leftAngle;
        }

        muzzle[i].transform.eulerAngles = rotate;
    }
    /// <summary>
    /// 右か左から初期角度に
    /// </summary>
    /// <param name="rotate"></param>
    private void LRToIni(Vector3 rotate, int i)
    {
        if (rotate.z == rightAngle)
        {
            rivers = true;
        }
        if (rotate.z == leftAngle)
        {
            rivers = false;
        }
        rotate.z = iniAngles[i];
        muzzle[i].transform.eulerAngles = rotate;
    }
}
