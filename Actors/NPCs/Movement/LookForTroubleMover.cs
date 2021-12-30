using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x02000006 RID: 6
	internal class LookForTroubleMover : Mover
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002D37 File Offset: 0x00000F37
		public LookForTroubleMover(float chaseTreshold, float speed)
		{
			this.chaseThreshold = chaseTreshold;
			this.speed = speed;
			this.resetTimer();
			this.oldDirection = -1;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002D5A File Offset: 0x00000F5A
		private void resetTimer()
		{
			this.timerEnd = Engine.Frame + 10L + (long)Engine.Random.Next(20);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002D7C File Offset: 0x00000F7C
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.oldDirection = direction;
			this.changed = false;
			if (ViewManager.Instance.FollowActor != null)
			{
				float num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				this.mode = ((num < this.chaseThreshold) ? LookForTroubleMover.Mode.Chase : LookForTroubleMover.Mode.Wait);
			}
			if (this.mode == LookForTroubleMover.Mode.Wait)
			{
				if (Engine.Frame > this.timerEnd)
				{
					direction = ((Engine.Random.Next(100) < 50) ? 0 : 4);
					this.resetTimer();
				}
				velocity = VectorMath.ZERO_VECTOR;
				this.changed = true;
			}
			else if (this.mode == LookForTroubleMover.Mode.Chase && ViewManager.Instance.FollowActor != null)
			{
				direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
				velocity = VectorMath.DirectionToVector(direction) * this.speed;
				this.changed = true;
			}
			return this.changed;
		}

		// Token: 0x04000096 RID: 150
		private const int TIMER_MAX = 30;

		// Token: 0x04000097 RID: 151
		private const int TIMER_MIN = 10;

		// Token: 0x04000098 RID: 152
		private float chaseThreshold;

		// Token: 0x04000099 RID: 153
		private float speed;

		// Token: 0x0400009A RID: 154
		private long timerEnd;

		// Token: 0x0400009B RID: 155
		private LookForTroubleMover.Mode mode;

		// Token: 0x0400009C RID: 156
		private int oldDirection;

		// Token: 0x0400009D RID: 157
		private bool changed;

		// Token: 0x02000007 RID: 7
		private enum Mode
		{
			// Token: 0x0400009F RID: 159
			Wait,
			// Token: 0x040000A0 RID: 160
			Chase
		}
	}
}
