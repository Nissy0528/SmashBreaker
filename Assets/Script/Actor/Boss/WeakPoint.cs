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
        if (col.tag == "PlayerBullet")
        {
            //boss_class.Shoot(0, true);
            Destroy(gameObject);
        }
    }
}
