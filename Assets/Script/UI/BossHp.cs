using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    private Slider slider;
    private Boss boss_class;
    private GameObject boss;

    // Use this for initialization
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            boss_class = boss.GetComponent<Boss>();
        }
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null) return;
        ShowHP();
    }

    /// <summary>
    /// ボスの体力UI表示
    /// </summary>
    private void ShowHP()
    {
        int maxHp = boss_class.MaxHP;//最大体力
        int currentHp = boss_class.HP;//現在の体力
        float hp = (float)currentHp / maxHp;//体力の割合

        slider.value = hp;//スライダーに体力を反映
    }
}
