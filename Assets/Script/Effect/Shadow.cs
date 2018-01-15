using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Material[] mat;//マテリアルの配列

    private GameObject[] pillars;//ゲーム上にある柱オブジェクト
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start()
    {
        pillars = GameObject.FindGameObjectsWithTag("Pillar");
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// マテリアル変更
    /// </summary>
    /// <param name="i"></param>
    private void ChangeMat(int i)
    {
        Vector3 pillarPos = nearPillar().transform.GetChild(0).position;

        if (transform.position.y > pillarPos.y)
        {
            sprite.material = mat[0];
        }
        if (transform.position.y < pillarPos.y)
        {
            sprite.material = mat[1];
        }
    }

    /// <summary>
    /// 一番近い柱
    /// </summary>
    /// <returns></returns>
    private GameObject nearPillar()
    {
        float tmpDis = 0;//距離用一時変数
        float nearDis = 0;//最も近い柱の距離
        GameObject pillar = null;//柱

        foreach (GameObject p in pillars)
        {
            //柱との距離取得
            tmpDis = Vector3.Distance(p.transform.position, transform.position);

            //柱の距離が近いか、0であれば柱取得
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                pillar = p;
            }
        }

        return pillar;
    }
}
