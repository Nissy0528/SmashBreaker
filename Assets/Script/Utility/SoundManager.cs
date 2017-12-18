using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SoundManager
{
	/// <summary>
	/// 消音
	/// </summary>
	private bool isMute;

	/// <summary>
	/// 音量
	/// </summary>
	private static SoundVolume soundVolume;

	public static SoundVolume SoundVolume
	{
		get
		{
			return soundVolume;
		}

		set
		{
			soundVolume = value;
		}
	}

	private void Mute()
	{
		isMute = true;
	}
}
