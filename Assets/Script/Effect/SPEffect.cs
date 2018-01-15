using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPEffect : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float peak;//最高点の位置
    public float speed;//速度
    public float radius;//最高点までの幅
    public GameObject center;//2点間の線分上の座標オブジェクト
    public GameObject spObj;//スマッシュポイントオブジェクト
    public GameObject chargeObj;

    private ParticleSystem particle;//パーティクル
    private Vector2 endPoint;//移動先の座標
    private Vector2[] p = new Vector2[3];//曲線用の三点
    private Vector2[] b = new Vector2[3];//三点の線分上の座標
    private float t;

    // Use this for initialization
    void Start()
    {
        SetPoints();
        t = 0.0f;
        particle = GetComponentInChildren<ParticleSystem>();
        chargeObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetPoints();
        Lerp();
        ParticleSimulate();
    }

    /// <summary>
    /// 三点を設定
    /// </summary>
    private void SetPoints()
    {
        p[0] = transform.position;
        p[1] = endPoint;
        Vector2 centerPos = center.transform.position;

        //p0、p1の線分上の座標は常にp1を向いている
        Vector2 lookPos = p[1];
        Vector2 vec = (lookPos - centerPos).normalized;
        float angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        center.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        Vector2 add = center.transform.right * radius;

        //p0、p1の線分上の座標を設定
        centerPos.x = (1 - peak) * p[0].x + peak * p[1].x;
        centerPos.y = (1 - peak) * p[0].y + peak * p[1].y;
        center.transform.position = centerPos;

        p[2] = centerPos + add;//上の座標から指定した幅分平行移動させた座標
    }

    /// <summary>
    /// 線形補間
    /// </summary>
    private void Lerp()
    {
        t += Time.unscaledDeltaTime * speed;


        b[0] = (1 - t) * p[0] + t * p[2];//p0、p2の線分上の座標
        b[1] = (1 - t) * p[2] + t * p[1];//p2、p1の線分上の座標

        b[2] = (1 - t) * b[0] + t * b[1];//b0、b1線分上の座標

        spObj.transform.position = b[2];

        if (t >= 1.0f)
        {
            FinishParticle();
            if (t >= 1.7f)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 終了パーティクル
    /// </summary>
    private void FinishParticle()
    {
        if (chargeObj.activeSelf) return;

        spObj.SetActive(false);
        chargeObj.SetActive(true);
        chargeObj.transform.position = endPoint;
    }

    /// <summary>
    /// TimeScaleを無視してパーティクル再生
    /// </summary>
    private void ParticleSimulate()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Simulate(Time.unscaledDeltaTime, true, false);
    }

    /// <summary>
    /// 移動先の座標設定
    /// </summary>
    /// <param name="endPoint"></param>
    public void SetEndPoint(Vector2 endPoint)
    {
        this.endPoint = endPoint;
    }
}
