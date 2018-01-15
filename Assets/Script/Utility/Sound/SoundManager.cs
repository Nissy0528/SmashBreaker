using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;
using System;

public class SoundManager
{
	/// <summary>
	/// BGMリスト
	/// </summary>
	private static Dictionary<string, AudioClip> bgms;
	/// <summary>
	/// SEリスト
	/// </summary>
	private static Dictionary<string, AudioClip> ses;

	/// <summary>
	/// 読み込みしたか
	/// </summary>
	private static bool isLoad;

	/// <summary>
	/// 読み込み
	/// </summary>
	public static void Load()
	{
		var load = Resources.LoadAll<AudioClip>(SoundUtility.GetPath(SoundType.BGM));

		bgms = new Dictionary<string, AudioClip>();

		foreach (var b in load)
		{
			bgms.Add(b.name, b);
		}

		load = Resources.LoadAll<AudioClip>(SoundUtility.GetPath(SoundType.SE));

		ses = new Dictionary<string, AudioClip>();

		foreach (AudioClip s in load)
		{
			ses.Add(s.name, s);
		}

		isLoad = true;
	}


	public static void PlaySound(SoundType type, string name)
	{
		if (!isLoad)
		{
			Load();
		}

		if(type == SoundType.BGM)
		{

		}
		else if(type == SoundType.SE)
		{
			PlaySE(name);
		}
	}

	/// <summary>
	/// se再生
	/// </summary>
	/// <param name="name"></param>
	/// <param name="isLoop"></param>
	/// <returns>オーディオ</returns>
	private static void PlaySE(string name, bool isLoop = false)
	{
		var se = new GameObject("se");

		se.hideFlags = HideFlags.HideInHierarchy;

		var source = se.AddComponent<AudioSource>();
		var play = se.AddComponent<SoundPlayer>();

		source.clip = ses[name];

	}

	/// <summary>
	/// bgm再生
	/// </summary>
	public static void PlayBGM(string name)
	{
		var bgm = new GameObject("bgm");
	}
}
