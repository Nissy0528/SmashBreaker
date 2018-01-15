using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound;

namespace Sound
{
	public static class SoundUtility
	{
		/// <summary>
		/// 音があるパス
		/// </summary>
		private static string audioPass = @"Sound\";

		/// <summary>
		/// 音量
		/// </summary>
		private static SoundVolume soundVolume;

		/// <summary>
		/// 音量
		/// </summary>
		public static SoundVolume Volume
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
		/// パスの取得
		/// </summary>
		/// <returns></returns>
		public static string GetPath(SoundType type)
		{
			return audioPass + type.ToString();
		}

		/// <summary>
		/// 消音設定の変更
		/// </summary>
		/// <param name="value"></param>
		public static void ChangeMute(SoundType type, bool value)
		{
			soundVolume.SetMute(type, value);
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

}