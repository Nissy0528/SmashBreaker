using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// オプション画面
/// </summary>
public class Option : MonoBehaviour
{
	/// <summary>
	/// タイプ一覧
	/// </summary>
	private enum BType
	{
		close,
		save,
	}


	[SerializeField]
	private string nextScene;

	/// <summary>
	/// 音量情報の一時変数
	/// </summary>
	private SoundVolume volumeLog;


	/// <summary>
	/// ボタン一覧
	/// </summary>
	private Dictionary<BType, Button> buttons;

	// Use this for initialization
	void Start()
	{
		ButtonSet();
		gameObject.SetActive(false);
	}

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
	}

	/// <summary>
	/// ボタンを押した際の処理
	/// </summary>
	/// <param name="type"></param>
	private void ButtonPush(BType type)
	{
		if (type == BType.close)
		{
			Close();
		}
		if (type == BType.save)
		{
			Save();
		}
		else
		{
			DebugCommand.DebugLog("挙動が設定されてません");
		}
	}

	private void OnEnable()
	{
		volumeLog = GameManager.SoundVolume;
	}
	
	private void Close()
	{

	}

	private void Save()
	{
		GameManager.SoundVolume = volumeLog;
	}
}
