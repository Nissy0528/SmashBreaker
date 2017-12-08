using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public GameObject parent;//ボスオブジェクト

    private Enemy boss_class;

    private void Start()
    {
        boss_class = parent.GetComponent<Enemy>();
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        //プレイヤーに攻撃されたらプレイヤーが向いてる方向に吹き飛ぶ
        if (col.transform.tag == "Player" && col.gameObject.GetComponent<Player>().IsState(Player.State.DASH))
        {
            boss_class.Shoot();
        }
    }


    public void Dead()
    {
        boss_class.Shoot();
    }
}
