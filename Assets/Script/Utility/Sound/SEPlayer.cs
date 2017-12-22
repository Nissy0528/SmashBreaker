using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sound
{
	/// <summary>
	/// SEの再生コンポーネント
	/// </summary>
	class SEPlayer:MonoBehaviour
	{
		private bool loop;
		private AudioSource source;

		private void Start()
		{
			source = GetComponent<AudioSource>();
		}

		private void Update()
		{
			source.volume = SoundUtility.SEVolume;
			source.loop = loop;

			if (source.isPlaying && !loop)
			{
				Destroy(gameObject);
			}
		}

		/// <summary>
		/// ループの設定
		/// </summary>
		/// <param name="value"></param>
		public void SetLoop(bool value)
		{
			loop = value;
		}
	}
}
