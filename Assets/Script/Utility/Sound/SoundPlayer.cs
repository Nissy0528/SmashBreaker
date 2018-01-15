using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sound
{
	class SoundPlayer:MonoBehaviour
	{
		/// <summary>
		/// ループ設定
		/// </summary>
		protected bool isLoop;
		protected AudioClip clip;
		protected AudioSource source;

		/// <summary>
		/// ループ設定
		/// </summary>
		public bool IsLoop
		{
			get
			{
				return isLoop;
			}

			set
			{
				isLoop = value;
			}
		}

		private void Start()
		{
			PlayerStart();
		}

		private void Update()
		{
			PlayerUpdate();
		}

		protected virtual void PlayerStart()
		{
			source = gameObject.AddComponent<AudioSource>();
		}

		protected virtual void PlayerUpdate() {}

		/// <summary>
		/// クリップの設定
		/// </summary>
		/// <param name="clip"></param>
		public void SetClip(AudioClip clip)
		{
			this.clip = clip;
		}
	}
}
