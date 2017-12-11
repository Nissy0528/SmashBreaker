using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    private Slider slider;
    private Enemy enemy;

    // Use this for initialization
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Boss").GetComponent<Enemy>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowHP();
    }

    /// <summary>
    /// ボスの体力UI表示
    /// </summary>
    private void ShowHP()
    {
        int maxHp = enemy.MaxHP;//最大体力
        int currentHp = enemy.HP;//現在の体力
        float hp = (float)currentHp / maxHp;//体力の割合

        slider.value = hp;//スライダーに体力を反映
    }
}
