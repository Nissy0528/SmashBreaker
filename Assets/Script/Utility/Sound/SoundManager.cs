using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class SoundManager
{
	/// <summary>
	/// 音があるパス
	/// </summary>
	private static string audioPass = @"Sound\";

	/// <summary>
	/// オーディオ
	/// </summary>
	private static List<AudioSource> audioPlayers;

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
		var bgm = Resources.LoadAll<AudioClip>(audioPass + "BGM");

		bgms = new Dictionary<string, AudioClip>();

		foreach (var b in bgm)
		{
			bgms.Add(b.name, b);
		}


		var se = Resources.LoadAll<AudioClip>(audioPass + "SE");

		ses = new Dictionary<string, AudioClip>();

		foreach (AudioClip s in se)
		{
			ses.Add(s.name, s);
		}

		isLoad = true;
	}

	/// <summary>
	/// se再生
	/// </summary>
	/// <param name="name"></param>
	/// <param name="isLoop"></param>
	/// <returns>オーディオ</returns>
	public static AudioSource PlaySE(string name, bool isLoop = false)
	{
		if (!isLoad)
		{
			Load();
		}

		var se = new GameObject("se");

		se.hideFlags = HideFlags.HideInHierarchy;

		var source = se.AddComponent<AudioSource>();
		var play = se.AddComponent<SEPlayer>();
		play.SetLoop(isLoop);

		source.clip = ses[name];

		return source;
	}

	/// <summary>
	/// bgm再生
	/// </summary>
	public static void PlayBGM(string name)
	{
		if (!isLoad)
		{
			Load();
		}

		var bgm = new GameObject("bgm");
	}


}
