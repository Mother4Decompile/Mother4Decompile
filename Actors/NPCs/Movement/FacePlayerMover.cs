using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x020000A0 RID: 160
	internal class FacePlayerMover : Mover
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00015BED File Offset: 0x00013DED
		public FacePlayerMover()
		{
			this.oldDirection = -1;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00015BFC File Offset: 0x00013DFC
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.oldDirection = direction;
			if (ViewManager.Instance.FollowActor != null)
			{
				direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
			}
			this.changed = (this.oldDirection != direction);
			return this.changed;
		}

		// Token: 0x040004F6 RID: 1270
		private int oldDirection;

		// Token: 0x040004F7 RID: 1271
		private bool changed;
	}
}
