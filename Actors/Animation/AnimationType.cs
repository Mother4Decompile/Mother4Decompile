using System;

namespace Mother4.Actors.Animation
{
	// Token: 0x02000003 RID: 3
	internal enum AnimationType
	{
		// Token: 0x04000009 RID: 9
		INVALID,
		// Token: 0x0400000A RID: 10
		DIRECTION_MASK = 255,
		// Token: 0x0400000B RID: 11
		STANCE_MASK = 65280,
		// Token: 0x0400000C RID: 12
		EAST = 1,
		// Token: 0x0400000D RID: 13
		NORTHEAST,
		// Token: 0x0400000E RID: 14
		NORTH,
		// Token: 0x0400000F RID: 15
		NORTHWEST,
		// Token: 0x04000010 RID: 16
		WEST,
		// Token: 0x04000011 RID: 17
		SOUTHWEST,
		// Token: 0x04000012 RID: 18
		SOUTH,
		// Token: 0x04000013 RID: 19
		SOUTHEAST,
		// Token: 0x04000014 RID: 20
		STAND = 256,
		// Token: 0x04000015 RID: 21
		WALK = 512,
		// Token: 0x04000016 RID: 22
		RUN = 768,
		// Token: 0x04000017 RID: 23
		CROUCH = 1024,
		// Token: 0x04000018 RID: 24
		DEAD = 1280,
		// Token: 0x04000019 RID: 25
		IDLE = 1536,
		// Token: 0x0400001A RID: 26
		TALK = 1792,
		// Token: 0x0400001B RID: 27
		BLINK = 2048,
		// Token: 0x0400001C RID: 28
		CLIMB = 2304,
		// Token: 0x0400001D RID: 29
		SWIM = 2560,
		// Token: 0x0400001E RID: 30
		FLOAT = 2816,
		// Token: 0x0400001F RID: 31
		NAUSEA = 3072
	}
}
