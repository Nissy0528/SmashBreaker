using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float stanTime;//硬直時間

    private Animator anim;//アニメーション
    private NormalEnemy followClass;
    private Dash dashClass;
    private RazerShooter razerClass;
    private GameManager gameManager;
    private float stanDelay;//硬直カウント

    // Use this for initialization
    void Start()
    {
        anim = transform.Find("Chara").GetComponent<Animator>();
        followClass = GetComponent<NormalEnemy>();
        dashClass = GetComponent<Dash>();
        razerClass = GetComponent<RazerShooter>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	//影
        ShadowSet();
        Initialize();
        stanDelay = stanTime;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyUpdate();
        AI();
        Stan();//硬直
        BossDead();//消滅
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

        //if (isStan)
        //{
        //    followClass.Start();
        //    dashClass.Start();
        //    razerClass.Start();
        //}

        if (HP <= 0)
        {
            followClass.enabled = false;
            dashClass.enabled = false;
            razerClass.enabled = false;
        }
    }

    /// <summary>
    /// 硬直
    /// </summary>
    private void Stan()
    {
        if (!isStan) return;

        stanDelay -= Time.deltaTime;
        if (stanDelay <= 0.0f)
        {
            isStan = false;
            stanDelay = stanTime;
        }
    }

    /// <summary>
    /// 消滅
    /// </summary>
    private void BossDead()
    {
        if (HP > 0) return;
        isStan = true;
        GameObject effect = Instantiate(dead_effect);
        effect.transform.position = transform.position;
        gameManager.Slow(10.0f, 0.25f);
        Destroy(gameObject);
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
