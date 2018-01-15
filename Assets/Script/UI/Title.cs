using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TitleUtility;

public class Title : MonoBehaviour
{
	/// <summary>
	/// タイトル文字
	/// </summary>
	public static string titleCaptions = "titles";

	/// <summary>
	/// アニメ
	/// </summary>
	private TitleAnim anime;

	/// <summary>
	/// タイプ一覧
	/// </summary>
	private enum BType
	{
		start,
		option,
	}

	/// <summary>
	/// 移動先のscene
	/// </summary>
	[SerializeField]
	private string nextScene;

	/// <summary>
	/// ボタン一覧
	/// </summary>
	private Dictionary<BType, Button> buttons;

	// Use this for initialization
	void Start()
	{
		ButtonSet();
		GameManager.stageNum = 0;
		GameManager.time = 0.0f;
		anime = new TitleAnim();
	}

	/// <summary>
	/// ボタンを設定
	/// </summary>
	private void ButtonSet()
	{
		buttons = new Dictionary<BType, Button>();
		foreach (BType t in Enum.GetValues(typeof(BType)))
		{
			var name = Enum.GetName(typeof(BType), t);
			var canvas = GameObject.Find("Canvas").transform;
			var b = canvas.Find(name).gameObject.GetComponent<Button>();
			b.onClick.AddListener(() => ButtonPush(t));
			buttons.Add(t, b);
		}
		buttons[BType.start].Select();

	}

	/// <summary>
	/// ボタンを押した際の処理
	/// </summary>
	/// <param name="type"></param>
	private void ButtonPush(BType type)
	{
		SoundManager.PlaySound(SoundType.SE, "title_selectSE");

		if (type == BType.start)
		{
			StartPush();
		}
		else if (type == BType.option)
		{
			OptionPush();
		}
		else
		{
			DebugCommand.DebugLog("挙動が設定されてません");
		}
	}

	/// <summary>
	/// ゲーム開始処理
	/// </summary>
	private void StartPush()
	{
		AnimeChange(AnimState.start);
	}

	/// <summary>
	/// オプション
	/// </summary>
	private void OptionPush()
	{
		foreach (var b in buttons.Values)
		{
			b.gameObject.SetActive(false);
		}
		GameObject.Find(titleCaptions).SetActive(false);

		GameObject canvas = GameObject.Find("Canvas");
		var optionGroup = canvas.transform.Find("optiongroup").gameObject;
		optionGroup.SetActive(true);
	}

	private void Update()
	{
		Anime();
	}

	/// <summary>
	/// 演出実行フラグ
	/// </summary>
	private bool isAnime = true;

	/// <summary>
	/// 演出
	/// </summary>
	private void Anime()
	{
		if (!isAnime)
		{
			return;
		}

		anime.Update();
		StateCheck();
	}

	/// <summary>
	/// 進行状況に応じた処理
	/// </summary>
	private void StateCheck()
	{

		if (anime.State == AnimState.open)
		{
			foreach (var b in buttons.Values)
			{
				b.gameObject.SetActive(false);
			}
		}
		else if (anime.State == AnimState.openEnd)
		{
			ButtonOn();
			isAnime = false;
		}
		else if (anime.State == AnimState.start)
		{
			foreach (var b in buttons.Values)
			{
				Transform canvas = GameObject.Find("Canvas").transform;

				for (var c = 0; c < canvas.childCount; c++)
				{
					var g = canvas.GetChild(c);
					if(g.name != "knuckle" && g.name != "Back")
					{
						g.gameObject.SetActive(false);
					}
					else
					{
						g.gameObject.SetActive(true);
					}
					
				}

			}
		}
		else if (anime.State == AnimState.startEnd)
		{
			Invoke("SceneChange", 0.3f);
		}

	}

	/// <summary>
	/// ボタンの有効化
	/// </summary>
	public void ButtonOn()
	{
		foreach (var b in buttons.Values)
		{
			b.gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// scene変更
	/// </summary>
	private void SceneChange()
	{
		SceneManager.LoadSceneAsync(nextScene);
	}

	/// <summary>
	/// 演出変更
	/// </summary>
	/// <param name="state"></param>
	private void AnimeChange(AnimState state)
	{
		isAnime = true;

		anime.State = state;
	}
}


namespace TitleUtility
{
	public enum AnimState
	{
		open,
		openEnd,
		none,
		start,
		startEnd,
	}
}
