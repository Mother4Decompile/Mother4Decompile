using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Data;

namespace Mother4.Battle.UI
{
	// Token: 0x0200007B RID: 123
	internal class LevelUpJingler : IDisposable
	{
		// Token: 0x06000290 RID: 656 RVA: 0x000101C8 File Offset: 0x0000E3C8
		public LevelUpJingler(CharacterType[] characters, bool useOutro)
		{
			this.useOutro = useOutro;
			this.baseJingle = AudioManager.Instance.Use(Paths.AUDIO + "jingleBase.wav", AudioType.Stream);
			this.baseJingle.LoopCount = -1;
			if (this.useOutro)
			{
				this.groupOutro = AudioManager.Instance.Use(Paths.AUDIO + "groupOutro.wav", AudioType.Sound);
			}
			this.characterJingles = new Dictionary<CharacterType, CarbineSound>();
			foreach (CharacterType characterType in characters)
			{
				string filename = string.Format("{0}jingle{1}.{2}", Paths.AUDIO, CharacterNames.GetName(characterType), "wav");
				CarbineSound carbineSound = AudioManager.Instance.Use(filename, AudioType.Stream);
				carbineSound.LoopCount = -1;
				this.characterJingles.Add(characterType, carbineSound);
			}
			this.state = LevelUpJingler.State.Stopped;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000102A8 File Offset: 0x0000E4A8
		~LevelUpJingler()
		{
			this.Dispose(false);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000102D8 File Offset: 0x0000E4D8
		public void Play()
		{
			if (this.state == LevelUpJingler.State.Stopped)
			{
				this.baseJingle.Play();
				foreach (CarbineSound carbineSound in this.characterJingles.Values)
				{
					carbineSound.Volume = 0f;
					carbineSound.Play();
				}
				this.state = LevelUpJingler.State.Playing;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00010358 File Offset: 0x0000E558
		public void Play(CharacterType character)
		{
			this.Play();
			if (this.characterJingles.ContainsKey(character))
			{
				AudioManager.Instance.FadeIn(this.characterJingles[character], 800U);
				this.state = LevelUpJingler.State.Playing;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00010390 File Offset: 0x0000E590
		public void End()
		{
			if (this.useOutro)
			{
				foreach (CarbineSound sound in this.characterJingles.Values)
				{
					AudioManager.Instance.FadeOut(sound, 400U);
				}
				AudioManager.Instance.FadeOut(this.baseJingle, 400U);
				this.groupOutro.Play();
			}
			else
			{
				foreach (CarbineSound sound2 in this.characterJingles.Values)
				{
					AudioManager.Instance.FadeOut(sound2, 3000U);
				}
				AudioManager.Instance.FadeOut(this.baseJingle, 3000U);
			}
			this.state = LevelUpJingler.State.Ending;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00010488 File Offset: 0x0000E688
		public void Stop()
		{
			this.baseJingle.Stop();
			this.groupOutro.Stop();
			foreach (CarbineSound carbineSound in this.characterJingles.Values)
			{
				carbineSound.Stop();
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000104F8 File Offset: 0x0000E6F8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00010508 File Offset: 0x0000E708
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				AudioManager.Instance.Unuse(this.baseJingle);
				if (this.useOutro)
				{
					AudioManager.Instance.Unuse(this.groupOutro);
				}
				foreach (CarbineSound sound in this.characterJingles.Values)
				{
					AudioManager.Instance.Unuse(sound);
				}
				this.disposed = true;
			}
		}

		// Token: 0x040003F3 RID: 1011
		private const string AUDIO_EXT = "wav";

		// Token: 0x040003F4 RID: 1012
		private const uint FADE_IN_DURATION = 800U;

		// Token: 0x040003F5 RID: 1013
		private const uint FADE_OUT_DURATION = 3000U;

		// Token: 0x040003F6 RID: 1014
		private const uint FADE_OUT_QUICK_DURATION = 400U;

		// Token: 0x040003F7 RID: 1015
		private bool disposed;

		// Token: 0x040003F8 RID: 1016
		private CarbineSound baseJingle;

		// Token: 0x040003F9 RID: 1017
		private CarbineSound groupOutro;

		// Token: 0x040003FA RID: 1018
		private Dictionary<CharacterType, CarbineSound> characterJingles;

		// Token: 0x040003FB RID: 1019
		private bool useOutro;

		// Token: 0x040003FC RID: 1020
		private LevelUpJingler.State state;

		// Token: 0x0200007C RID: 124
		private enum State
		{
			// Token: 0x040003FE RID: 1022
			Playing,
			// Token: 0x040003FF RID: 1023
			Ending,
			// Token: 0x04000400 RID: 1024
			Stopped
		}
	}
}
