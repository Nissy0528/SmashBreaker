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
	class SEPlayer:SoundPlayer
	{
		protected override void PlayerStart()
		{
			base.PlayerStart();
		}

		private void Update()
		{
			source.volume = SoundUtility.SEVolume;
			source.loop = isLoop;

			if (source.isPlaying && !isLoop)
			{
				Destroy(gameObject);
			}
		}

		
	}
}
