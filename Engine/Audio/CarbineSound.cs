using System;

namespace Carbine.Audio
{
	// Token: 0x0200000A RID: 10
	public abstract class CarbineSound : IDisposable
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002A2E File Offset: 0x00000C2E
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002A36 File Offset: 0x00000C36
		public virtual uint LoopBegin
		{
			get
			{
				return this.loopBegin;
			}
			set
			{
				this.loopBegin = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002A3F File Offset: 0x00000C3F
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002A47 File Offset: 0x00000C47
		public virtual uint LoopEnd
		{
			get
			{
				return this.loopEnd;
			}
			set
			{
				this.loopEnd = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002A50 File Offset: 0x00000C50
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002A58 File Offset: 0x00000C58
		public virtual int LoopCount
		{
			get
			{
				return this.loopCount;
			}
			set
			{
				this.loopCount = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002A61 File Offset: 0x00000C61
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002A69 File Offset: 0x00000C69
		public virtual float Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				this.volume = Math.Max(0f, Math.Min(1f, value));
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002A86 File Offset: 0x00000C86
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002A8E File Offset: 0x00000C8E
		public virtual float Pitch
		{
			get
			{
				return this.pitch;
			}
			set
			{
				this.pitch = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002A97 File Offset: 0x00000C97
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002A9F File Offset: 0x00000C9F
		public virtual uint Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public AudioType AudioType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public virtual bool IsPaused
		{
			get
			{
				throw new NotImplementedException("IsPaused not implemented.");
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000052 RID: 82 RVA: 0x00002ABC File Offset: 0x00000CBC
		// (remove) Token: 0x06000053 RID: 83 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public event CarbineSound.OnCompleteHandler OnComplete;

		// Token: 0x06000054 RID: 84 RVA: 0x00002B29 File Offset: 0x00000D29
		public CarbineSound(AudioType soundType, uint loopBegin, uint loopEnd, int loopCount, float volume, float pitch)
		{
			this.loopBegin = loopBegin;
			this.loopEnd = loopEnd;
			this.loopCount = loopCount;
			this.volume = volume;
			this.pitch = pitch;
			this.type = soundType;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002B60 File Offset: 0x00000D60
		~CarbineSound()
		{
			this.Dispose(false);
		}

		// Token: 0x06000056 RID: 86
		public abstract void Play();

		// Token: 0x06000057 RID: 87
		public abstract void Pause();

		// Token: 0x06000058 RID: 88
		public abstract void Resume();

		// Token: 0x06000059 RID: 89
		public abstract void Stop();

		// Token: 0x0600005A RID: 90 RVA: 0x00002B90 File Offset: 0x00000D90
		protected void HandleSoundCompletion()
		{
			if (this.OnComplete != null)
			{
				this.OnComplete(this);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002BA6 File Offset: 0x00000DA6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600005C RID: 92
		protected abstract void Dispose(bool disposing);

		// Token: 0x0400002B RID: 43
		protected uint position;

		// Token: 0x0400002C RID: 44
		protected int loopCount;

		// Token: 0x0400002D RID: 45
		protected uint loopBegin;

		// Token: 0x0400002E RID: 46
		protected uint loopEnd;

		// Token: 0x0400002F RID: 47
		protected float volume;

		// Token: 0x04000030 RID: 48
		protected float pitch;

		// Token: 0x04000031 RID: 49
		protected AudioType type;

		// Token: 0x04000032 RID: 50
		protected bool disposed;

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x0600005E RID: 94
		public delegate void OnCompleteHandler(CarbineSound sender);
	}
}
