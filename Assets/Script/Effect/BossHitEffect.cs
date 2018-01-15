using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitEffect : MonoBehaviour
{
    private Animator anim;
    private Boss boss;//ボスクラス
    private int bossHP;//ボスの体力
    private int iniHP;//前回のボスの体力

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
        if (bossObj == null)
        {
            GetComponent<BossHitEffect>().enabled = false;
            return;
        }
        boss = bossObj.GetComponent<Boss>();
        bossHP = boss.HP;
        iniHP = bossHP;
    }

    // Update is called once per frame
    void Update()
    {
        bossHP = boss.HP;
        Anim();
    }

    /// <summary>
    /// アニメション
    /// </summary>
    private void Anim()
    {
        if (bossHP == iniHP) return;

        anim.enabled = true;
        if (bossHP <= 0)
        {
            anim.SetBool("BossDead", true);
        }
        iniHP = bossHP;
    }
}
