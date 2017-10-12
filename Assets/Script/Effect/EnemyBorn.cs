using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBorn : MonoBehaviour
{
    public GameObject enemy;//敵

    private Animator anim;//アニメーション
    private bool isBorn;//falseなら生成できる

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        isBorn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーションが終わったら敵生成
        var animState = anim.GetCurrentAnimatorStateInfo(0);
        if (animState.normalizedTime >= 0.5f && !isBorn)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            isBorn = true;
        }
        if (animState.normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
