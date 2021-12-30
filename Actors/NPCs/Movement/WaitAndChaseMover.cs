using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x0200000C RID: 12
	internal class WaitAndChaseMover : Mover
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000031BB File Offset: 0x000013BB
		public WaitAndChaseMover(float chaseTreshold, float speed)
		{
			this.chaseThreshold = chaseTreshold;
			this.speed = speed;
			this.oldDirection = -1;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000031D8 File Offset: 0x000013D8
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.oldDirection = direction;
			if (ViewManager.Instance.FollowActor != null)
			{
				float num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				this.mode = ((num < this.chaseThreshold) ? WaitAndChaseMover.Mode.Chase : WaitAndChaseMover.Mode.Wait);
			}
			if (this.mode == WaitAndChaseMover.Mode.Wait)
			{
				velocity = VectorMath.ZERO_VECTOR;
			}
			else if (this.mode == WaitAndChaseMover.Mode.Chase && ViewManager.Instance.FollowActor != null)
			{
				direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
				velocity = VectorMath.DirectionToVector(direction) * this.speed;
			}
			this.changed = (this.oldDirection != direction);
			return this.changed;
		}

		// Token: 0x040000B7 RID: 183
		private float chaseThreshold;

		// Token: 0x040000B8 RID: 184
		private float speed;

		// Token: 0x040000B9 RID: 185
		private WaitAndChaseMover.Mode mode;

		// Token: 0x040000BA RID: 186
		private int oldDirection;

		// Token: 0x040000BB RID: 187
		private bool changed;

		// Token: 0x0200000D RID: 13
		private enum Mode
		{
			// Token: 0x040000BD RID: 189
			Wait,
			// Token: 0x040000BE RID: 190
			Chase
		}
	}
}
