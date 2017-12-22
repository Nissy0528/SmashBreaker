using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public struct SoundVolume
#pragma warning restore CS0660 // 型は演算子 == または演算子 != を定義しますが、Object.Equals(object o) をオーバーライドしません
#pragma warning restore CS0661 // 型は演算子 == または演算子 != を定義しますが、Object.GetHashCode() をオーバーライドしません
{
	//音量
	private float bgmVolume;
	private float seVolume;

	/// <summary>
	/// BGM音量
	/// </summary>
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
	/// <summary>
	/// SE音量
	/// </summary>
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

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="bgm"></param>
	/// <param name="se"></param>
	public SoundVolume(float bgm, float se)
	{
		bgmVolume = bgm;
		seVolume = se;
	}


	/// <summary>
	/// 消音
	/// </summary>
	public SoundVolume Mute
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

	/// <summary>
	/// 無音フラグ
	/// </summary>
	public bool IsMute
	{
		get
		{
			return this == Mute;
		}
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


public enum SoundType
{
	bgm,
	se,
}
