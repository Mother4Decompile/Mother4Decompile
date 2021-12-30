using System;

namespace Carbine.Audio.Stub
{
	// Token: 0x02000011 RID: 17
	internal class StubSound : CarbineSound
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000363A File Offset: 0x0000183A
		public override bool IsPaused
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000363D File Offset: 0x0000183D
		public StubSound(AudioType audioType, uint loopBegin, uint loopEnd, int loopCount, float volume, float pitch) : base(audioType, loopBegin, loopEnd, loopCount, volume, pitch)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000364E File Offset: 0x0000184E
		public override void Play()
		{
			base.HandleSoundCompletion();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003656 File Offset: 0x00001856
		public override void Pause()
		{
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003658 File Offset: 0x00001858
		public override void Resume()
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000365A File Offset: 0x0000185A
		public override void Stop()
		{
			base.HandleSoundCompletion();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003662 File Offset: 0x00001862
		protected override void Dispose(bool disposing)
		{
		}
	}
}
