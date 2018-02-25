using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private AnimState state;//アニメーション状態
        private Transform canvas;//キャンバス
        private Transform titles;//タイトル
        private Transform backPlayer;//後ろ向きのプレイヤー
        private Transform pyramid;//ピラミッド
        private GameObject fade;//フェード
        private TitlePlayer player;
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
            backPlayer = canvas.Find("TitlePlayerBack");
            pyramid = canvas.Find("Pyramid");
            fade = canvas.Find("Fade").gameObject;
            player = GameObject.Find("TitlePlayer").GetComponent<TitlePlayer>();
            OpenSet();
            StartSet();
        }

        /// <summary>
        /// scene開始時
        /// </summary>
        private void OpenSet()
        {
            smashSprite = titles.Find("smash").gameObject;
            //smashSprite.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 1f);

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
                //mainCamera.SetShake(0.3f);

                //titles.Find("caption1").gameObject.SetActive(false);
                //titles.Find("caption2").gameObject.SetActive(true);

                //smashSprite.SetActive(false);

                State = AnimState.openEnd;

                return;
            }
            //var rect = smashSprite.GetComponent<RectTransform>();
            //rect.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1f, 1f), count);
            Image fadeImage = fade.GetComponent<Image>();
            Color fadeColor = fadeImage.color;
            fadeColor.a = 1.0f - count;
            fadeImage.color = fadeColor;
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
                State = AnimState.startEnd;
                return;
            }

            SmashSprite();
            InPyramid();
        }

        /// <summary>
        /// スマッシュ画像
        /// </summary>
        private void SmashSprite()
        {
            if (!smashSprite.activeSelf) return;

            if (player.DashEnd())
            {
                smashSprite.GetComponent<Animator>().enabled = true;
            }

            Animator smash_anim = smashSprite.GetComponent<Animator>();
            AnimatorStateInfo smash_animState = smash_anim.GetCurrentAnimatorStateInfo(0);
            if (smash_animState.normalizedTime >= 1.0f)
            {
                player.transform.Find("Smash").gameObject.SetActive(true);
                smashSprite.SetActive(false);
            }
        }

        /// <summary>
        /// ピラミッドに入る
        /// </summary>
        private void InPyramid()
        {
            if (player != null) return;

            backPlayer.gameObject.SetActive(true);

            Animator backPlayer_anim = backPlayer.GetComponent<Animator>();
            AnimatorStateInfo backPlayer_animState = backPlayer_anim.GetCurrentAnimatorStateInfo(0);
            Animator pyramid_anim = pyramid.GetComponent<Animator>();
            AnimatorStateInfo pyramid_animState = pyramid_anim.GetCurrentAnimatorStateInfo(0);
            if (!isPlayerWalk)
            {
                backPlayer_anim.SetBool("Start", true);
                isPlayerWalk = true;
            }
            if (backPlayer_animState.IsName("TitlePlayer_Walk"))
            {
                if (backPlayer_animState.normalizedTime >= 1.0f)
                {
                    pyramid_anim.enabled = true;
                    count = pyramid_animState.normalizedTime;
                }
            }
        }
    }
}