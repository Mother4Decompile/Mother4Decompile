using System;
using FMOD;

namespace Carbine.Audio.fmod
{
	// Token: 0x0200000F RID: 15
	internal sealed class FmodSound : CarbineSound
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000031E0 File Offset: 0x000013E0
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003218 File Offset: 0x00001418
		public override uint Position
		{
			get
			{
				if (this.channel != null)
				{
					RESULT position = this.channel.getPosition(ref this.position, TIMEUNIT.MS);
					FmodAudioManager.ERRCHECK(position);
					return this.position;
				}
				return 0U;
			}
			set
			{
				this.position = value;
				RESULT result = this.channel.setPosition(this.position, TIMEUNIT.MS);
				FmodAudioManager.ERRCHECK(result);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003248 File Offset: 0x00001448
		public override bool IsPaused
		{
			get
			{
				if (this.channel != null)
				{
					bool result = false;
					RESULT paused = this.channel.getPaused(ref result);
					FmodAudioManager.ERRCHECK(paused);
					return result;
				}
				return false;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003276 File Offset: 0x00001476
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003280 File Offset: 0x00001480
		public override float Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				this.volume = ((this.type == AudioType.Stream) ? AudioManager.Instance.MusicVolume : AudioManager.Instance.EffectsVolume) * value;
				if (this.channel != null)
				{
					RESULT result = this.channel.setVolume(this.volume);
					FmodAudioManager.ERRCHECK(result);
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000032D4 File Offset: 0x000014D4
		// (set) Token: 0x06000077 RID: 119 RVA: 0x000032F8 File Offset: 0x000014F8
		public override int LoopCount
		{
			get
			{
				int result = 0;
				RESULT loopCount = this.sound.getLoopCount(ref result);
				FmodAudioManager.ERRCHECK(loopCount);
				return result;
			}
			set
			{
				RESULT result = this.sound.setLoopCount(value);
				FmodAudioManager.ERRCHECK(result);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003318 File Offset: 0x00001518
		public FmodSound(ref FMOD.System system, string filename, AudioType type, uint loopBegin, uint loopEnd, int loopCount, float volume) : base(type, loopBegin, loopEnd, loopCount, volume, 1f)
		{
			this.system = system;
			this.sound = new Sound();
			switch (this.type)
			{
			case AudioType.Sound:
			{
				RESULT result = system.createSound(filename, (MODE)74U, ref this.sound);
				FmodAudioManager.ERRCHECK(result);
				result = this.sound.setLoopCount(0);
				FmodAudioManager.ERRCHECK(result);
				return;
			}
			case AudioType.Stream:
			{
				RESULT result = system.createSound(filename, (MODE)202U, ref this.sound);
				FmodAudioManager.ERRCHECK(result);
				this.LoopCount = -1;
				return;
			}
			case AudioType.Sound3d:
			{
				RESULT result = system.createSound(filename, (MODE)82U, ref this.sound);
				FmodAudioManager.ERRCHECK(result);
				result = this.sound.setLoopCount(0);
				FmodAudioManager.ERRCHECK(result);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000033E0 File Offset: 0x000015E0
		public override void Play()
		{
			bool flag = false;
			if (this.type == AudioType.Stream && this.channel != null)
			{
				RESULT result = this.channel.isPlaying(ref flag);
			}
			if (!flag)
			{
				RESULT result = this.system.playSound(CHANNELINDEX.FREE, this.sound, true, ref this.channel);
				FmodAudioManager.ERRCHECK(result);
				CHANNEL_CALLBACK callback = (CHANNEL_CALLBACK)Delegate.CreateDelegate(typeof(CHANNEL_CALLBACK), this, "ChannelCallback");
				this.callbackIndex = ((FmodAudioManager)AudioManager.Instance).AddCallback(callback);
				result = this.channel.setCallback(callback);
				FmodAudioManager.ERRCHECK(result);
				result = this.channel.setVolume(this.volume);
				FmodAudioManager.ERRCHECK(result);
				float num = 0f;
				result = this.channel.getFrequency(ref num);
				FmodAudioManager.ERRCHECK(result);
				result = this.channel.setFrequency(num * this.pitch);
				FmodAudioManager.ERRCHECK(result);
				if (this.loopEnd > this.loopBegin)
				{
					result = this.channel.setLoopPoints(this.loopBegin, TIMEUNIT.MS, this.loopEnd, TIMEUNIT.MS);
					FmodAudioManager.ERRCHECK(result);
				}
				result = this.channel.setPaused(false);
				FmodAudioManager.ERRCHECK(result);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003505 File Offset: 0x00001705
		private RESULT ChannelCallback(IntPtr channelraw, CHANNEL_CALLBACKTYPE type, IntPtr commanddata1, IntPtr commanddata2)
		{
			if (type == CHANNEL_CALLBACKTYPE.END)
			{
				if (this.channel != null && this.channel.getRaw() == channelraw)
				{
					this.channel = null;
				}
				base.HandleSoundCompletion();
			}
			return RESULT.OK;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003534 File Offset: 0x00001734
		public override void Pause()
		{
			if (this.channel != null)
			{
				RESULT result = this.channel.setPaused(true);
				FmodAudioManager.ERRCHECK(result);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000355C File Offset: 0x0000175C
		public override void Resume()
		{
			if (this.channel != null)
			{
				RESULT result = this.channel.setPaused(false);
				FmodAudioManager.ERRCHECK(result);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003584 File Offset: 0x00001784
		public override void Stop()
		{
			if (this.channel != null)
			{
				bool flag = false;
				this.channel.isPlaying(ref flag);
				if (flag)
				{
					RESULT result = this.channel.stop();
					FmodAudioManager.ERRCHECK(result);
					this.channel = null;
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000035C8 File Offset: 0x000017C8
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				RESULT result = this.sound.release();
				FmodAudioManager.ERRCHECK(result);
				((FmodAudioManager)AudioManager.Instance).RemoveCallback(this.callbackIndex);
			}
			this.disposed = true;
		}

		// Token: 0x0400003E RID: 62
		private Sound sound;

		// Token: 0x0400003F RID: 63
		private FMOD.System system;

		// Token: 0x04000040 RID: 64
		private Channel channel;

		// Token: 0x04000041 RID: 65
		private int callbackIndex;
	}
}
