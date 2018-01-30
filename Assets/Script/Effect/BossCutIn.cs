using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCutIn : MonoBehaviour
{
    public GameObject[] sprites;
    public Vector3[] spritePos;
    public Vector3[] spriteSize;
    public float activeTime;

    private RectTransform rect;
    private int stageNum;

    // Use this for initialization
    void Start()
    {
        stageNum = GameManager.stageNum - 1;
        GameObject boss = Instantiate(sprites[stageNum]);
        rect = boss.GetComponent<RectTransform>();
        boss.transform.SetParent(transform);
        rect.localPosition = spritePos[stageNum];
        rect.localScale = spriteSize[stageNum];
        rect.localRotation = Quaternion.Euler(0.0f, 0.0f, -15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Active();
    }

    /// <summary>
    /// 指定した長さ表示
    /// </summary>
    private void Active()
    {
        activeTime -= Time.deltaTime;
        if (activeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
