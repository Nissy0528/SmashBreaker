using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    //アニメーション終了時の処理の種類
    public enum Mode
    {
        DESTROY,//消滅
        ACTIVE,//非表示
    }
    public Mode mode;//アニメーション終了時の処理の種類

    private Animator anim;//アニメーション

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーションが終わったら削除
        var animState = anim.GetCurrentAnimatorStateInfo(0);
        Finish(animState.normalizedTime);
    }

    /// <summary>
    /// アニメーション終了時の処理
    /// </summary>
    /// <param name="animTime"></param>
    private void Finish(float animTime)
    {
        if (animTime < 1f) return;

        if (mode == Mode.DESTROY)
        {
            Destroy(gameObject);
        }
        if (mode == Mode.ACTIVE)
        {
            gameObject.SetActive(false);
        }
    }
}
