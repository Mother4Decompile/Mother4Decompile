using System;
using Mother4.Overworld;
using SFML.System;

namespace Mother4.Actors.Animation
{
	// Token: 0x02000002 RID: 2
	internal struct AnimationContext
	{
		// Token: 0x04000001 RID: 1
		public Vector2f Velocity;

		// Token: 0x04000002 RID: 2
		public int SuggestedDirection;

		// Token: 0x04000003 RID: 3
		public TerrainType TerrainType;

		// Token: 0x04000004 RID: 4
		public bool IsDead;

		// Token: 0x04000005 RID: 5
		public bool IsCrouch;

		// Token: 0x04000006 RID: 6
		public bool IsTalk;

		// Token: 0x04000007 RID: 7
		public bool IsNauseous;
	}
}
