using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public Player player;

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay2D(Collision2D col)
    {
        //敵に当たったらダメージ
        if (col.transform.tag == "Enemy" || col.transform.tag == "Boss")
        {
            Damage();
        }
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    public void Damage()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (player.IsState(Player.State.DASH) || boss == null) return;

        //if (parameter.hp > 0 && !isDamage)
        //{
        //    ChangeHp(-1);
        //    isDamage = true;
        //}
        //if (parameter.hp <= 0 && state != State.DEAD)
        //{
        //    parameter.hp = 0;
        player.SetState(Player.State.DEAD);
        //}
    }
}
