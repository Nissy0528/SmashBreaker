﻿using System.Collections;
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

	/// <summary>
	/// 音量情報の一時変数
	/// </summary>
	private SoundVolume volumeSave;

	private bool isSave;

	/// <summary>
	/// ボタン一覧
	/// </summary>
	private Dictionary<BType, Button> buttons;


	// Use this for initialization
	void Start()
	{
		ButtonSet();
		SliderSet();
		gameObject.SetActive(false);
	}

	/// <summary>
	/// ボタンの設定
	/// </summary>
	private void ButtonSet()
	{
		buttons = new Dictionary<BType, Button>();
		foreach (BType t in Enum.GetValues(typeof(BType)))
		{
			var name = Enum.GetName(typeof(BType), t);
			GameObject obj = GameObject.Find(name);
			if (obj != null)
			{
				var b = obj.GetComponent<Button>();
				b.onClick.AddListener(() => ButtonPush(t));
				buttons.Add(t, b);
			}
		}
	}

	/// <summary>
	/// Sliderの設定
	/// </summary>
	private void SliderSet()
	{
		Transform sliders = transform.Find("SoundVolume").Find("Sliders");
		Slider bgm = sliders.Find("bgm").GetComponent<Slider>();
		Slider se = sliders.Find("se").GetComponent<Slider>();

		bgm.onValueChanged.AddListener(BGMChange);
		se.onValueChanged.AddListener(SEChange);
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
		else if (type == BType.save)
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
		volumeSave = SoundManager.SoundVolume;
	}

	/// <summary>
	/// 終了
	/// </summary>
	private void Close()
	{
		SoundManager.SoundVolume = volumeSave;
		gameObject.SetActive(false);
	}

	/// <summary>
	/// 設定保存
	/// </summary>
	private void Save()
	{
		volumeSave = SoundManager.SoundVolume;
	}

	/// <summary>
	/// BGM音量変更
	/// </summary>
	private void BGMChange(float value)
	{
		SoundManager.SoundVolume = new SoundVolume(value, SoundManager.SoundVolume.SeVolume);
	}

	/// <summary>
	/// 効果音量変更
	/// </summary>
	private void SEChange(float value)
	{
		SoundManager.SoundVolume = new SoundVolume(value, SoundManager.SoundVolume.SeVolume);

	}
}