using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject pointObj;
    public int pointCount;
    public float radius;

    private List<GameObject> pointList = new List<GameObject>();
    private GameObject boss;

    // Use this for initialization
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        CreatePoint();
        SetCollision();
    }

    // Update is called once per frame
    void Update()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = boss.transform.position;
        CreateBarrier();
        SetCollision();
    }

    /// <summary>
    /// バリアの繋ぎ目作成
    /// </summary>
    private void CreatePoint()
    {
        pointList = new List<GameObject>();

        for (int i = 0; i < pointCount; i++)
        {
            pointList.Add(Instantiate(pointObj));
            pointList[i].transform.parent = transform;
        }

        float angleDiff = 360f / (float)pointList.Count;

        for (int i = 0; i < pointList.Count; i++)
        {
            Vector2 pointPos = transform.position;

            float angle = (90 - angleDiff * i) * Mathf.Deg2Rad;
            pointPos.x += radius * Mathf.Cos(angle);
            pointPos.y += radius * Mathf.Sin(angle);

            if (pointList[i] == null) continue;

            pointList[i].transform.position = pointPos;
        }
    }

    /// <summary>
    /// バリア生成
    /// </summary>
    private void CreateBarrier()
    {
        for (int i = 0; i < pointCount; i++)
        {
            if (pointList[i] == null) continue;
            LineRenderer lineRenderer = pointList[i].GetComponent<LineRenderer>();
            if (i < pointCount - 1)
            {
                if (pointList[i + 1] != null)
                {
                    lineRenderer.SetPosition(0, pointList[i].transform.position);
                    lineRenderer.SetPosition(1, pointList[i + 1].transform.position);
                }
                else
                {
                    lineRenderer.SetPosition(0, pointList[i].transform.position);
                    lineRenderer.SetPosition(1, pointList[i].transform.position);
                }
            }
            else
            {
                if (pointList[0] != null)
                {
                    lineRenderer.SetPosition(0, pointList[i].transform.position);
                    lineRenderer.SetPosition(1, pointList[0].transform.position);
                }
                else
                {
                    lineRenderer.SetPosition(0, pointList[i].transform.position);
                    lineRenderer.SetPosition(1, pointList[i].transform.position);
                }
            }
        }
    }

    /// <summary>
    /// あたり判定設定
    /// </summary>
    private void SetCollision()
    {
        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();
        Vector2[] points = new Vector2[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            if (pointList[i] == null) continue;
            points[i] = pointList[i].transform.localPosition;
        }

        polygon.SetPath(0, points);
    }
}
