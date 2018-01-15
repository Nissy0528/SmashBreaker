using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class SceneWarpZone : MonoBehaviour
{

	/// <summary>
	/// 移動先のscene
	/// </summary>
	[SerializeField]
	private string nextScene = "Main";
    private bool isNext;//trueなら次のステージへ
    private Animator anim;//アニメーション

    /// <summary>
    /// 初期化
    /// </summary>
	private void Start()
	{
        anim = GetComponent<Animator>();
	}

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //アニメションが終了したら次のステージへ行ける
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        if(animState.normalizedTime>=1.0f)
        {
            isNext = true;
            GetComponent<Collider2D>().enabled = true;
        }
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<PlayerDamage>() != null)
		{
			Warp();
		}
	}
	/// <summary>
	/// 移動処理
	/// </summary>
	private void Warp()
	{
        GameManager.stageNum++;
		SceneManager.LoadScene(nextScene);
	}

    /// <summary>
    /// 次のステージへ
    /// </summary>
    public bool IsNext
    {
        get { return isNext; }
    }

    /// <summary>
    /// アニメーション開始
    /// </summary>
    public void StartAnimation()
    {
        anim.enabled = true;
    }
}
