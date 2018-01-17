using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public Player player;
    public GameObject audio;
    public AudioClip se;

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
        if (player.IsState(Player.State.DASH) || player.IsState(Player.State.DEAD)) return;

        //if (parameter.hp > 0 && !isDamage)
        //{
        //    ChangeHp(-1);
        //    isDamage = true;
        //}
        //if (parameter.hp <= 0 && state != State.DEAD)
        //{
        //    parameter.hp = 0;
        player.Damage();
        GameObject audioObj = Instantiate(audio);
        audioObj.GetComponent<SE>().SetClip(se);
        //}
    }
}
