using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 音量構造体
/// </summary>
public struct SoundVolume
{
	public float bgmVolume;
	public float seVolume;

	public SoundVolume(float bgm, float se)
	{
		bgmVolume = bgm;
		seVolume = se;
	}
}
