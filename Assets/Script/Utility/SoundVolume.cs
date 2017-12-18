using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 音量構造体
/// </summary>
public struct SoundVolume
{
	private float bgmVolume;
	private float seVolume;

	public float BgmVolume
	{
		get
		{
			return bgmVolume;
		}

		set
		{
			bgmVolume = value;
		}
	}

	public float SeVolume
	{
		get
		{
			return seVolume;
		}

		set
		{
			seVolume = value;
		}
	}

	public SoundVolume(float bgm, float se)
	{
		bgmVolume = bgm;
		seVolume = se;
	}

	public SoundVolume Zero
	{
		get
		{
			return new SoundVolume(0, 0);
		}
	}
}
