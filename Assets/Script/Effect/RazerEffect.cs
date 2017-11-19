using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerEffect : MonoBehaviour
{
    public GameObject[] particles;

    private Razer razer;
    private float razer_length;
    private float length;

    // Update is called once per frame
    void Update()
    {
        LengthChange();
    }

    /// <summary>
    /// 長さ変更
    /// </summary>
    private void LengthChange()
    {
        razer_length = razer.Length;
        length = razer_length / 2f;

        foreach (var p in particles)
        {
            SetParameter(p);
        }
    }

    /// <summary>
    /// パーティクルと位置設定
    /// </summary>
    /// <param name="prticleObj"></param>
    private void SetParameter(GameObject prticleObj)
    {
        ParticleSystem particle = prticleObj.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule EmObj = particle.emission;
        EmObj.rateOverTime = new ParticleSystem.MinMaxCurve(length / 0.2f);
        ParticleSystem.ShapeModule ShObj = particle.shape;
        ShObj.radius = length;
        Vector2 pos = prticleObj.transform.localPosition;
        pos.y = length;
        prticleObj.transform.localPosition = pos;
    }

    /// <summary>
    /// レーザークラス設定
    /// </summary>
    public Razer SetRazer
    {
        set { razer = value; }
    }
}
