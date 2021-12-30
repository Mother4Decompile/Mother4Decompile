using System;

namespace Carbine.Audio.Stub
{
	// Token: 0x02000010 RID: 16
	internal class StubAudioManager : AudioManager
	{
		// Token: 0x0600007F RID: 127 RVA: 0x0000360D File Offset: 0x0000180D
		public StubAudioManager()
		{
			Console.WriteLine("STUBBED AUDIO MANAGER");
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000361F File Offset: 0x0000181F
		public override void SetSpeakerMode(AudioMode mode)
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003621 File Offset: 0x00001821
		public override void Update()
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003623 File Offset: 0x00001823
		public override CarbineSound Use(string filename, AudioType type)
		{
			return new StubSound(type, 0U, 0U, 0, 0f, 0f);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003638 File Offset: 0x00001838
		public override void Unuse(CarbineSound sound)
		{
		}
	}
}
