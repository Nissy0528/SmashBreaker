using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound;

public static class SoundUtility
{
	/// <summary>
	/// 消音前音量
	/// </summary>
	private static SoundVolume preVolume;
	/// <summary>
	/// 音量
	/// </summary>
	private static SoundVolume soundVolume;

	/// <summary>
	/// 音量
	/// </summary>
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
	/// <summary>
	/// 消音
	/// </summary>
	public static bool IsMute
	{
		get
		{
			return soundVolume.IsMute;
		}

		set
		{
			///消音に変更
			if (value)
			{
				preVolume = soundVolume;
				soundVolume = SoundVolume.Mute;
			}
			///消音だったときのみ
			else if (IsMute)
			{
				soundVolume = preVolume;
			}
		}
	}

	/// <summary>
	/// BGM音量
	/// </summary>
	public static float BGMVolume
	{
		get
		{
			return soundVolume.BgmVolume;
		}
	}

	/// <summary>
	/// BGM音量
	/// </summary>
	public static float SEVolume
	{
		get
		{
			return soundVolume.SeVolume;
		}
	}

}
