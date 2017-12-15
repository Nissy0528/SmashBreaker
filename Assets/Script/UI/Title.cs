using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Title : MonoBehaviour
{
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
			var b = GameObject.Find(name).GetComponent<Button>();
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
		if(type == BType.start)
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
		SceneManager.LoadSceneAsync(nextScene);
	}

	/// <summary>
	/// オプション
	/// </summary>
	private void OptionPush()
	{
		foreach(var b in buttons.Values)
		{
			b.gameObject.SetActive(false);
		}
		GameObject.Find("titleCaption").SetActive(false);

		GameObject canvas = GameObject.Find("Canvas");
		var optionGroup = canvas.transform.Find("optiongroup").gameObject;
		optionGroup.SetActive(true);
	}
}
