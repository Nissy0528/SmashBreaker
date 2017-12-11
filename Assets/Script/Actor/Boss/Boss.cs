using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //private GameObject smashText;//スマッシュUI
    //private SmashGage playerSP;//プレイヤー体力UI
    private Animator anim;//アニメーション
    private NormalEnemy followClass;
    private Dash dashClass;
    private RazerShooter razerClass;

    // Use this for initialization
    void Start()
    {
        //playerSP = GameObject.Find("SmashGage").GetComponent<SmashGage>();
        //smashText = GameObject.Find("SmashText");
        //smashText.SetActive(false);
        anim = transform.Find("Chara").GetComponent<Animator>();
        followClass = GetComponent<NormalEnemy>();
        dashClass = GetComponent<Dash>();
        razerClass = GetComponent<RazerShooter>();
		//影
		ShadowSet();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyUpdate();
        AI();
    }

    /// <summary>
    /// 人工知能
    /// </summary>
    private void AI()
    {
        if (razerClass.GetEnable)
        {
            followClass.enabled = false;
            dashClass.enabled = false;
        }
        else
        {
            dashClass.enabled = true;
            followClass.enabled = !dashClass.IsDash();
        }

        if (isStan)
        {
            followClass.enabled = false;
            dashClass.enabled = false;
            razerClass.enabled = false;
        }
    }

    /// <summary>
    /// アニメーション切り替え
    /// </summary>
    /// <param name="name">切り替えるフラグの名前</param>
    public void AnimBool(string name, bool frag)
    {
        if (!anim.enabled) return;

        anim.SetBool(name, frag);
    }

    /// <summary>
    /// アニメーション終了判定
    /// </summary>
    /// <returns></returns>
    public bool AnimFinish(string name)
    {
        if (!anim.enabled) return false;

        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(name) && animState.normalizedTime >= 1;
    }

	/// <summary>
	/// 影の設定を有効にする
	/// </summary>
	private void ShadowSet()
	{
		var child = transform.GetChild(0);
		var sr = child.GetComponent<SpriteRenderer>();
		sr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}
}
