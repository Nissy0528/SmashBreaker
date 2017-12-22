using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

public class SoundManager
{
	/// <summary>
	/// オーディオ
	/// </summary>
	private static List<AudioSource> audioPlayers;



	/// <summary>
	/// se再生
	/// </summary>
	/// <param name="name"></param>
	/// <param name="isLoop"></param>
	/// <returns>オーディオ</returns>
	public static AudioSource PlaySE(string name, bool isLoop = false)
	{
		var se = new GameObject("se");

		var source = se.AddComponent<AudioSource>();
		var play = se.AddComponent<SEPlayer>();
		play.SetLoop(isLoop);

		return source;
	}

	/// <summary>
	/// bgm再生
	/// </summary>
	public static void PlaySound(string name)
	{

	}


}
