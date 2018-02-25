using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    public SmashGage smashGage;//スマッシュゲージ

    private Vector3 maxSize;//SP最大時の大きさ
    private Vector3 currentSize;//現在のSPに合わせた大きさ
    private ParticleSystem particle;//パーティクル
    private float sp;//スマシュポイント

    // Use this for initialization
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        maxSize = transform.localScale;
        sp = 0.0f;
        ChangeSize();
    }

    // Update is called once per frame
    void Update()
    {
        sp = smashGage.SpRate;
        ChangeSize();
    }

    /// <summary>
    /// 現在のSPに合わせて大きさを変化
    /// </summary>
    private void ChangeSize()
    {
        if (Time.timeScale == 0.0f) return;

        currentSize = maxSize * sp;
        transform.localScale = currentSize;
        var p_main = particle.main;

        //最大時は赤色に通常時は青色に変更
        if (!smashGage.IsMax)
        {
            p_main.startColor = Color.blue;
        }
        else
        {
            p_main.startColor = Color.red;
        }
    }
}
