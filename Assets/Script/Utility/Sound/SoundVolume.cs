using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound;

/// <summary>
/// 音量
/// </summary>
public struct SoundVolume
{
	//音量
	private float bgmVolume;
	private float seVolume;

	// 消音フラグ
	private SoundType muteFlag;

	/// <summary>
	/// BGM音量
	/// </summary>
	public float BgmVolume
	{
		get
		{
			if (IsMute(SoundType.BGM))
			{
				return 0f;
			}

			return bgmVolume;
		}

		set
		{
			
			bgmVolume = value;
		}
	}

	/// <summary>
	/// SE音量
	/// </summary>
	public float SeVolume
	{
		get
		{
			if (IsMute(SoundType.SE))
			{
				return 0f;
			}
			return seVolume;
		}

		set
		{
			seVolume = value;
		}
	}

	/// <summary>
	/// 消音フラグ
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public bool IsMute(SoundType type)
	{
		return muteFlag.Check(type);
	}


	/// <summary>
	/// 消音設定
	/// </summary>
	/// <param name="type"></param>
	/// <param name="value">ON OFF</param>
	public void SetMute( SoundType type, bool value)
	{
		if (value)
		{
			//指定ビットOn
			muteFlag = muteFlag | type; 
		}
		else
		{
			//指定ビットOFF
			muteFlag = muteFlag & ~type;
		}
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="bgm"></param>
	/// <param name="se"></param>
	public SoundVolume(float bgm, float se)  
	{
		bgmVolume = bgm;
		seVolume = se;
		muteFlag = SoundType.None;
	}


	/// <summary>
	/// 消音
	/// </summary>
	public SoundVolume Zero
	{
		get
		{
			return new SoundVolume(0, 0);
		}
	}

	public static bool operator== (SoundVolume value1, SoundVolume value2)
	{
		bool bgm = value1.bgmVolume == value2.bgmVolume;
		bool se = value1.seVolume == value2.seVolume;
		return bgm && se;
	}

	public static bool operator !=(SoundVolume value1, SoundVolume value2)
	{
		bool bgm = value1.bgmVolume != value2.bgmVolume;
		bool se = value1.seVolume != value2.seVolume;
		return bgm || se;
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

}