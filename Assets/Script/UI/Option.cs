using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sound;

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
		volumeSave = SoundUtility.Volume;
	}

	/// <summary>
	/// 終了
	/// </summary>
	private void Close()
	{
		SoundUtility.Volume = volumeSave;

		GameObject.Find("Title").GetComponent<Title>().ButtonOn();

		foreach (var b in buttons.Values)
		{
			b.gameObject.SetActive(false);
		}
		Transform canvas = GameObject.Find("Canvas").transform;

		canvas.Find(Title.titleCaptions).gameObject.SetActive(true);

		gameObject.SetActive(false);
	}

	/// <summary>
	/// 設定保存
	/// </summary>
	private void Save()
	{
		volumeSave = SoundUtility.Volume;
	}

	/// <summary>
	/// BGM音量変更
	/// </summary>
	private void BGMChange(float value)
	{
		SoundUtility.Volume = new SoundVolume(value, SoundUtility.Volume.SeVolume);
	}

	/// <summary>
	/// 効果音量変更
	/// </summary>
	private void SEChange(float value)
	{
		SoundUtility.Volume = new SoundVolume(SoundUtility.Volume.BgmVolume, value);
	}
}
