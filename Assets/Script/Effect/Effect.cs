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
    private AnimatorStateInfo animState;

    // Use this for initialization
    void Start()
    {
        Initialized();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialized()
    {
        anim = GetComponent<Animator>();
        animState = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(animState.shortNameHash, 0, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーションが終わったら削除
        animState = anim.GetCurrentAnimatorStateInfo(0);
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
            anim.enabled = false;
        }
    }
}
