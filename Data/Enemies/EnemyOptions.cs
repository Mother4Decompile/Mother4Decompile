using System;

namespace Mother4.Data.Enemies
{
	// Token: 0x02000027 RID: 39
	internal enum EnemyOptions
	{
		// Token: 0x040001A8 RID: 424
		None,
		// Token: 0x040001A9 RID: 425
		IsBackgroundLayer,
		// Token: 0x040001AA RID: 426
		IsBoss,
		// Token: 0x040001AB RID: 427
		IsGhost = 4,
		// Token: 0x040001AC RID: 428
		IsImmortal = 8,
		// Token: 0x040001AD RID: 429
		IsInsect = 16,
		// Token: 0x040001AE RID: 430
		IsRobot = 32,
		// Token: 0x040001AF RID: 431
		NoChat = 64,
		// Token: 0x040001B0 RID: 432
		NoTelepathy = 128,
		// Token: 0x040001B1 RID: 433
		SelfDestruct = 256
	}
}
