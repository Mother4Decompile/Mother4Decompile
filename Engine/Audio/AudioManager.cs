using System;
using System.Collections.Generic;
using Carbine.Audio.fmod;

namespace Carbine.Audio
{
	// Token: 0x02000006 RID: 6
	public abstract class AudioManager : IDisposable
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000026C8 File Offset: 0x000008C8
		public static AudioManager Instance
		{
			get
			{
				if (AudioManager.instance == null)
				{
					AudioManager.instance = new FmodAudioManager();
				}
				return AudioManager.instance;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000026E0 File Offset: 0x000008E0
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000026E8 File Offset: 0x000008E8
		public float EffectsVolume
		{
			get
			{
				return this.effectsVolume;
			}
			set
			{
				this.effectsVolume = value;
				this.UpdateSoundVolume();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000026F7 File Offset: 0x000008F7
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000026FF File Offset: 0x000008FF
		public float MusicVolume
		{
			get
			{
				return this.musicVolume;
			}
			set
			{
				this.musicVolume = value;
				this.UpdateSoundVolume();
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000270E File Offset: 0x0000090E
		public CarbineSound BGM
		{
			get
			{
				return this.bgmSound;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002716 File Offset: 0x00000916
		public AudioManager()
		{
			this.instances = new Dictionary<int, int>();
			this.sounds = new Dictionary<int, CarbineSound>();
			this.faders = new List<AudioManager.Fader>();
			this.deadFaders = new List<AudioManager.Fader>();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000274C File Offset: 0x0000094C
		~AudioManager()
		{
			this.Dispose(false);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000277C File Offset: 0x0000097C
		public virtual void Update()
		{
			this.UpdateFaders();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002784 File Offset: 0x00000984
		private void UpdateSoundVolume()
		{
			foreach (CarbineSound carbineSound in this.sounds.Values)
			{
				carbineSound.Volume = ((carbineSound.AudioType == AudioType.Stream) ? this.musicVolume : this.effectsVolume);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027F4 File Offset: 0x000009F4
		private void UpdateFaders()
		{
			for (int i = 0; i < this.faders.Count; i++)
			{
				AudioManager.Fader fader = this.faders[i];
				fader.ticks += 16U;
				float num = fader.ticks / fader.duration;
				fader.sound.Volume = fader.fromVolume + (fader.toVolume - fader.fromVolume) * num;
				if (fader.ticks >= fader.duration)
				{
					fader.sound.Volume = fader.toVolume;
					this.deadFaders.Add(fader);
				}
			}
			for (int j = 0; j < this.deadFaders.Count; j++)
			{
				AudioManager.Fader fader2 = this.deadFaders[j];
				if (fader2.stopOnEnd)
				{
					fader2.sound.Stop();
				}
				this.faders.Remove(fader2);
			}
			this.deadFaders.Clear();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028EC File Offset: 0x00000AEC
		private void Fade(CarbineSound sound, uint duration, float volume, bool stopOnEnd)
		{
			AudioManager.Fader item = new AudioManager.Fader
			{
				sound = sound,
				duration = duration,
				ticks = 0U,
				fromVolume = sound.Volume,
				toVolume = volume,
				stopOnEnd = stopOnEnd
			};
			this.faders.Add(item);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000293D File Offset: 0x00000B3D
		public void Fade(CarbineSound sound, uint duration, float volume)
		{
			this.Fade(sound, duration, volume, false);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002949 File Offset: 0x00000B49
		public void FadeOut(CarbineSound sound, uint duration)
		{
			this.Fade(sound, duration, 0f, true);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002959 File Offset: 0x00000B59
		public void FadeIn(CarbineSound sound, uint duration)
		{
			sound.Play();
			this.Fade(sound, duration, 1f, false);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002970 File Offset: 0x00000B70
		public void SetBGM(string name)
		{
			CarbineSound bgm = this.Use(name, AudioType.Stream);
			this.SetBGM(bgm);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000298D File Offset: 0x00000B8D
		private void SetBGM(CarbineSound newBGM)
		{
			if (this.bgmSound != null)
			{
				this.Unuse(this.bgmSound);
			}
			this.bgmSound = newBGM;
		}

		// Token: 0x0600003D RID: 61
		public abstract void SetSpeakerMode(AudioMode mode);

		// Token: 0x0600003E RID: 62
		public abstract CarbineSound Use(string filename, AudioType type);

		// Token: 0x0600003F RID: 63
		public abstract void Unuse(CarbineSound sound);

		// Token: 0x06000040 RID: 64 RVA: 0x000029AA File Offset: 0x00000BAA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000029BC File Offset: 0x00000BBC
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				foreach (CarbineSound carbineSound in this.sounds.Values)
				{
					carbineSound.Dispose();
				}
			}
			this.disposed = true;
		}

		// Token: 0x04000015 RID: 21
		private static AudioManager instance;

		// Token: 0x04000016 RID: 22
		private List<AudioManager.Fader> faders;

		// Token: 0x04000017 RID: 23
		private List<AudioManager.Fader> deadFaders;

		// Token: 0x04000018 RID: 24
		protected Dictionary<int, int> instances;

		// Token: 0x04000019 RID: 25
		protected Dictionary<int, CarbineSound> sounds;

		// Token: 0x0400001A RID: 26
		protected float musicVolume;

		// Token: 0x0400001B RID: 27
		protected float effectsVolume;

		// Token: 0x0400001C RID: 28
		protected bool disposed;

		// Token: 0x0400001D RID: 29
		protected CarbineSound bgmSound;

		// Token: 0x02000007 RID: 7
		private class Fader
		{
			// Token: 0x0400001E RID: 30
			public CarbineSound sound;

			// Token: 0x0400001F RID: 31
			public uint ticks;

			// Token: 0x04000020 RID: 32
			public uint duration;

			// Token: 0x04000021 RID: 33
			public float fromVolume;

			// Token: 0x04000022 RID: 34
			public float toVolume;

			// Token: 0x04000023 RID: 35
			public bool stopOnEnd;
		}
	}
}
