using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleUtility
{
	public class TitleAnim
	{
		/// <summary>
		/// 演出カウント
		/// </summary>
		private float count = 0f;
		private bool isAnim = false;

		/// <summary>
		/// 水晶玉スプライト
		/// </summary>
		private GameObject smashSprite;

		/// <summary>
		/// 拳
		/// </summary>
		private GameObject knuckleSprite;

		private AnimState state;
		private Transform canvas;
		private Transform titles;
        private bool isPlayerWalk;//プレイヤー歩きアニメーション開始

		/// <summary>
		/// 演出の進行状況
		/// </summary>
		public AnimState State
		{
			get
			{
				return state;
			}

			set
			{
				//カウントをリセット
				count = 0f;
				state = value;
			}
		}

		// Use this for initialization
		public TitleAnim()
		{
			isAnim = false;
			canvas = GameObject.Find("Canvas").transform;
			titles = GameObject.Find("titles").transform;
			OpenSet();
			StartSet();
		}

		/// <summary>
		/// scene開始時
		/// </summary>
		private void OpenSet()
		{
			smashSprite = titles.Find("smash").gameObject;
			smashSprite.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 1f);

			titles.Find("caption1").gameObject.SetActive(true);
			titles.Find("caption2").gameObject.SetActive(false);
		}

		/// <summary>
		/// ゲーム開始時
		/// </summary>
		private void StartSet()
		{
			knuckleSprite = canvas.Find("knuckle").gameObject;
			knuckleSprite.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 1f);

			knuckleSprite.SetActive(false);
		}

		// Update is called once per frame
		public void Update()
		{
			OpenAnime();
			StartAnime();
		}

		/// <summary>
		/// シーンを開いた時のアニメ
		/// </summary>
		private void OpenAnime()
		{
			if (State != AnimState.open)
			{
				return;
			}
			else if (count > 1f || Input.GetButton("Smash"))
			{
				var mainCamera = GameObject.Find("Main Camera").GetComponent<TitleCamera>();
				mainCamera.SetShake(0.3f);

				titles.Find("caption1").gameObject.SetActive(false);
				titles.Find("caption2").gameObject.SetActive(true);

				smashSprite.SetActive(false);

				State = AnimState.openEnd;

				return;
			}
			var rect = smashSprite.GetComponent<RectTransform>();
			rect.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1f, 1f), count);
			count += Time.deltaTime;
		}


		/// <summary>
		/// scene開始時のアニメ
		/// </summary>
		private void StartAnime()
		{
			if (State != AnimState.start)
			{
				return;
			}
			else if (count >= 1f)
			{
				//var mainCamera = GameObject.Find("Main Camera").GetComponent<TitleCamera>();
				//mainCamera.SetShake(0.3f);

				State = AnimState.startEnd;
				return;
			}
            //var rect = knuckleSprite.GetComponent<RectTransform>();
            //rect.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1f, 1f), count);
            //float speed = 3f;
            //count += Time.deltaTime * speed;
            Animator anim = canvas.Find("Player").GetComponent<Animator>();
            AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
            if(!isPlayerWalk)
            {
                anim.SetBool("Start", true);
                isPlayerWalk = true;
            }
            if (animState.IsName("TitlePlayer_Walk"))
            {
                count = animState.normalizedTime;
            }
		}
	}
}