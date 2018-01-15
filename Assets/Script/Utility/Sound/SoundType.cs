using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum SoundType
{
	None = 0x0,
	BGM = 0x1,
	SE = 0x2,
	ALL = 0x3,
}
static class SoundTypeEx
{
	/// <summary>
	/// 文字列変換
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static string ToString(this SoundType type)
	{
		if (type == SoundType.ALL)
		{
			return "error";
		}
		return type.ToString("G");
	}

	/// <summary>
	/// フラグのチェック
	/// </summary>
	/// <param name="type"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public static bool Check(this SoundType type, SoundType value)
	{
		return (type & value) == value;
	}
}
